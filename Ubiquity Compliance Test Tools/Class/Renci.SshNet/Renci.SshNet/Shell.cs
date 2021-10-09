using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Renci.SshNet
{
	public class Shell : IDisposable
	{
		private readonly Session _session;

		private ChannelSession _channel;

		private EventWaitHandle _channelClosedWaitHandle;

		private Stream _input;

		private readonly string _terminalName;

		private readonly uint _columns;

		private readonly uint _rows;

		private readonly uint _width;

		private readonly uint _height;

		private readonly IDictionary<TerminalModes, uint> _terminalModes;

		private EventWaitHandle _dataReaderTaskCompleted;

		private readonly Stream _outputStream;

		private readonly Stream _extendedOutputStream;

		private readonly int _bufferSize;

		private bool _disposed;

		public bool IsStarted
		{
			get;
			private set;
		}

		public event EventHandler<EventArgs> Starting;

		public event EventHandler<EventArgs> Started;

		public event EventHandler<EventArgs> Stopping;

		public event EventHandler<EventArgs> Stopped;

		public event EventHandler<ExceptionEventArgs> ErrorOccurred;

		internal Shell(Session session, Stream input, Stream output, Stream extendedOutput, string terminalName, uint columns, uint rows, uint width, uint height, IDictionary<TerminalModes, uint> terminalModes, int bufferSize)
		{
			_session = session;
			_input = input;
			_outputStream = output;
			_extendedOutputStream = extendedOutput;
			_terminalName = terminalName;
			_columns = columns;
			_rows = rows;
			_width = width;
			_height = height;
			_terminalModes = terminalModes;
			_bufferSize = bufferSize;
		}

		public void Start()
		{
			if (IsStarted)
			{
				throw new SshException("Shell is started.");
			}
			if (this.Starting != null)
			{
				this.Starting(this, new EventArgs());
			}
			_channel = _session.CreateClientChannel<ChannelSession>();
			_channel.DataReceived += Channel_DataReceived;
			_channel.ExtendedDataReceived += Channel_ExtendedDataReceived;
			_channel.Closed += Channel_Closed;
			_session.Disconnected += Session_Disconnected;
			_session.ErrorOccured += Session_ErrorOccured;
			_channel.Open();
			_channel.SendPseudoTerminalRequest(_terminalName, _columns, _rows, _width, _height, _terminalModes);
			_channel.SendShellRequest();
			_channelClosedWaitHandle = new AutoResetEvent(initialState: false);
			_dataReaderTaskCompleted = new ManualResetEvent(initialState: false);
			Shell shell;
			byte[] buffer;
			ExecuteThread(delegate
			{
				try
				{
					shell = this;
					buffer = new byte[_bufferSize];
					while (_channel.IsOpen)
					{
						IAsyncResult asyncResult = _input.BeginRead(buffer, 0, buffer.Length, delegate(IAsyncResult result)
						{
							if (shell._input != null)
							{
								int num = shell._input.EndRead(result);
								if (num > 0)
								{
									shell._channel.SendData(buffer.Take(num).ToArray());
								}
							}
						}, null);
						WaitHandle.WaitAny(new WaitHandle[2]
						{
							asyncResult.AsyncWaitHandle,
							_channelClosedWaitHandle
						});
						if (!asyncResult.IsCompleted)
						{
							break;
						}
					}
				}
				catch (Exception exception)
				{
					RaiseError(new ExceptionEventArgs(exception));
				}
				finally
				{
					_dataReaderTaskCompleted.Set();
				}
			});
			IsStarted = true;
			if (this.Started != null)
			{
				this.Started(this, new EventArgs());
			}
		}

		public void Stop()
		{
			if (!IsStarted)
			{
				throw new SshException("Shell is not started.");
			}
			if (_channel != null && _channel.IsOpen)
			{
				_channel.SendEof();
				_channel.Close();
			}
		}

		private void Session_ErrorOccured(object sender, ExceptionEventArgs e)
		{
			RaiseError(e);
		}

		private void RaiseError(ExceptionEventArgs e)
		{
			this.ErrorOccurred?.Invoke(this, e);
		}

		private void Session_Disconnected(object sender, EventArgs e)
		{
			Stop();
		}

		private void Channel_ExtendedDataReceived(object sender, ChannelDataEventArgs e)
		{
			if (_extendedOutputStream != null)
			{
				_extendedOutputStream.Write(e.Data, 0, e.Data.Length);
			}
		}

		private void Channel_DataReceived(object sender, ChannelDataEventArgs e)
		{
			if (_outputStream != null)
			{
				_outputStream.Write(e.Data, 0, e.Data.Length);
			}
		}

		private void Channel_Closed(object sender, ChannelEventArgs e)
		{
			if (this.Stopping != null)
			{
				ExecuteThread(delegate
				{
					this.Stopping(this, new EventArgs());
				});
			}
			if (_channel.IsOpen)
			{
				_channel.SendEof();
				_channel.Close();
			}
			_channelClosedWaitHandle.Set();
			_input.Dispose();
			_input = null;
			_dataReaderTaskCompleted.WaitOne(_session.ConnectionInfo.Timeout);
			Extensions.Dispose(_dataReaderTaskCompleted);
			_dataReaderTaskCompleted = null;
			_channel.DataReceived -= Channel_DataReceived;
			_channel.ExtendedDataReceived -= Channel_ExtendedDataReceived;
			_channel.Closed -= Channel_Closed;
			_session.Disconnected -= Session_Disconnected;
			_session.ErrorOccured -= Session_ErrorOccured;
			if (this.Stopped != null)
			{
				ExecuteThread(delegate
				{
					this.Stopped(this, new EventArgs());
				});
			}
			_channel = null;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					if (_channelClosedWaitHandle != null)
					{
						Extensions.Dispose(_channelClosedWaitHandle);
						_channelClosedWaitHandle = null;
					}
					if (_channel != null)
					{
						_channel.Dispose();
						_channel = null;
					}
					if (_dataReaderTaskCompleted != null)
					{
						Extensions.Dispose(_dataReaderTaskCompleted);
						_dataReaderTaskCompleted = null;
					}
				}
				_disposed = true;
			}
		}

		~Shell()
		{
			Dispose(disposing: false);
		}

		private void ExecuteThread(Action action)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				action();
			});
		}
	}
}
