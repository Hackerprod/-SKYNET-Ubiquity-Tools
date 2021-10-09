using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Connection;
using Renci.SshNet.Messages.Transport;
using SKYNET;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Renci.SshNet
{
	public class SshCommand : IDisposable
	{
		private readonly Session _session;

		private ChannelSession _channel;

		private CommandAsyncResult _asyncResult;

		private AsyncCallback _callback;

		private EventWaitHandle _sessionErrorOccuredWaitHandle = new AutoResetEvent(initialState: false);

		private Exception _exception;

		private bool _hasError;

		private readonly object _endExecuteLock = new object();

		private StringBuilder _result;

		private StringBuilder _error;

		private bool _isDisposed;

		public string CommandText
		{
			get;
			private set;
		}

		public TimeSpan CommandTimeout
		{
			get;
			set;
		}

		public int ExitStatus
		{
			get;
			private set;
		}

		public Stream OutputStream
		{
			get;
			private set;
		}

		public Stream ExtendedOutputStream
		{
			get;
			private set;
		}

		public string Result
		{
			get
			{
				if (_result == null)
				{
					_result = new StringBuilder();
				}
				if (OutputStream != null && OutputStream.Length > 0)
				{
					using (StreamReader streamReader = new StreamReader(OutputStream, _session.ConnectionInfo.Encoding))
					{
						_result.Append(streamReader.ReadToEnd());
					}
				}
				return _result.ToString();
			}
		}

		public string Error
		{
			get
			{
				if (_hasError)
				{
					if (_error == null)
					{
						_error = new StringBuilder();
					}
					if (ExtendedOutputStream != null && ExtendedOutputStream.Length > 0)
					{
						using (StreamReader streamReader = new StreamReader(ExtendedOutputStream, _session.ConnectionInfo.Encoding))
						{
							_error.Append(streamReader.ReadToEnd());
						}
					}
					return _error.ToString();
				}
				return string.Empty;
			}
		}

		internal SshCommand(Session session, string commandText)
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			if (commandText == null)
			{
				throw new ArgumentNullException("commandText");
			}
			_session = session;
			CommandText = commandText;
			CommandTimeout = new TimeSpan(0, 0, 0, 0, -1);
			_session.Disconnected += Session_Disconnected;
			_session.ErrorOccured += Session_ErrorOccured;
		}

		public IAsyncResult BeginExecute()
		{
			return BeginExecute(null, null);
		}

		public IAsyncResult BeginExecute(AsyncCallback callback)
		{
			return BeginExecute(callback, null);
		}

		public IAsyncResult BeginExecute(AsyncCallback callback, object state)
		{
			if (_asyncResult != null)
			{
				throw new InvalidOperationException("Asynchronous operation is already in progress.");
			}
			_asyncResult = new CommandAsyncResult(this)
			{
				AsyncWaitHandle = new ManualResetEvent(initialState: false),
				IsCompleted = false,
				AsyncState = state
			};
			if (_channel != null)
			{
				throw new SshException("Invalid operation.");
			}
			CreateChannel();
			if (string.IsNullOrEmpty(CommandText))
			{
				return null;
			}
			_callback = callback;
			_channel.Open();
			_channel.SendExecRequest(CommandText);
			return _asyncResult;
		}

		public IAsyncResult BeginExecute(string commandText, AsyncCallback callback, object state)
		{
			CommandText = commandText;
			return BeginExecute(callback, state);
		}

		public string EndExecute(IAsyncResult asyncResult)
		{
			if (_asyncResult == asyncResult && _asyncResult != null)
			{
				lock (_endExecuteLock)
				{
					if (_asyncResult != null)
					{
						WaitOnHandle(_asyncResult.AsyncWaitHandle);
						if (_channel.IsOpen)
						{
							_channel.SendEof();
							_channel.Close();
						}
						_channel = null;
						_asyncResult = null;
						return Result;
					}
				}
			}
			throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndExecute was called multiple times with the same IAsyncResult.");
		}

		public string Execute()
		{
			return EndExecute(BeginExecute(null, null));
		}

		public void CancelAsync()
		{
			if (_channel != null && _channel.IsOpen && _asyncResult != null)
			{
				_channel.Close();
			}
		}

		public string Execute(string commandText)
		{
			CommandText = commandText;
			return Execute();
		}

		private void CreateChannel()
		{
			_channel = _session.CreateClientChannel<ChannelSession>();
			_channel.DataReceived += Channel_DataReceived;
			_channel.ExtendedDataReceived += Channel_ExtendedDataReceived;
			_channel.RequestReceived += Channel_RequestReceived;
			_channel.Closed += Channel_Closed;
			if (OutputStream != null)
			{
				OutputStream.Dispose();
				OutputStream = null;
			}
			if (ExtendedOutputStream != null)
			{
				ExtendedOutputStream.Dispose();
				ExtendedOutputStream = null;
			}
			OutputStream = new PipeStream();
			ExtendedOutputStream = new PipeStream();
			_result = null;
			_error = null;
		}

		private void Session_Disconnected(object sender, EventArgs e)
		{
			if (!_isDisposed)
			{
				_exception = new SshConnectionException("An established connection was aborted by the software in your host machine.", DisconnectReason.ConnectionLost);
				_sessionErrorOccuredWaitHandle.Set();
			}
		}

		private void Session_ErrorOccured(object sender, ExceptionEventArgs e)
		{
			if (!_isDisposed)
			{
				_exception = e.Exception;
				_sessionErrorOccuredWaitHandle.Set();
			}
		}

		private void Channel_Closed(object sender, ChannelEventArgs e)
		{
			if (OutputStream != null)
			{
				OutputStream.Flush();
			}
			if (ExtendedOutputStream != null)
			{
				ExtendedOutputStream.Flush();
			}
			_asyncResult.IsCompleted = true;
			if (_callback != null)
			{
				ExecuteThread(delegate
				{
					_callback(_asyncResult);
				});
			}
			((EventWaitHandle)_asyncResult.AsyncWaitHandle).Set();
		}

		private void Channel_RequestReceived(object sender, ChannelRequestEventArgs e)
		{
			Message message = new ChannelFailureMessage(_channel.LocalChannelNumber);
			if (e.Info is ExitStatusRequestInfo)
			{
				ExitStatusRequestInfo exitStatusRequestInfo = e.Info as ExitStatusRequestInfo;
				ExitStatus = (int)exitStatusRequestInfo.ExitStatus;
				message = new ChannelSuccessMessage(_channel.LocalChannelNumber);
			}
			if (e.Info.WantReply)
			{
				_session.SendMessage(message);
			}
		}

		private void Channel_ExtendedDataReceived(object sender, ChannelDataEventArgs e)
		{
			if (ExtendedOutputStream != null)
			{
				ExtendedOutputStream.Write(e.Data, 0, e.Data.Length);
				ExtendedOutputStream.Flush();
			}
			if (e.DataTypeCode == 1)
			{
				_hasError = true;
			}
		}

		private void Channel_DataReceived(object sender, ChannelDataEventArgs e)
		{
			if (OutputStream != null)
			{
				OutputStream.Write(e.Data, 0, e.Data.Length);
				OutputStream.Flush();
			}
			if (_asyncResult != null)
			{
				lock (_asyncResult)
				{
					_asyncResult.BytesReceived += e.Data.Length;
				}
			}
		}

		private void WaitOnHandle(WaitHandle waitHandle)
		{
			WaitHandle[] waitHandles = new WaitHandle[2]
			{
				_sessionErrorOccuredWaitHandle,
				waitHandle
			};
			switch (WaitHandle.WaitAny(waitHandles, CommandTimeout))
			{
			case 0:
                    frmMain.frm.Write("An established connection was aborted by the server", SKYNET.LOG.MessageType.WARN);
                    throw new SshOperationTimeoutException(string.Format(CultureInfo.CurrentCulture, "An established connection was aborted by the server", new object[1]
                    {
                        ""
                    }));

            case 258:
				throw new SshOperationTimeoutException(string.Format(CultureInfo.CurrentCulture, "Command '{0}' has timed out.", new object[1]
				{
					CommandText
				}));
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					_session.Disconnected -= Session_Disconnected;
					_session.ErrorOccured -= Session_ErrorOccured;
					if (OutputStream != null)
					{
						OutputStream.Dispose();
						OutputStream = null;
					}
					if (ExtendedOutputStream != null)
					{
						ExtendedOutputStream.Dispose();
						ExtendedOutputStream = null;
					}
					if (_sessionErrorOccuredWaitHandle != null)
					{
						Extensions.Dispose(_sessionErrorOccuredWaitHandle);
						_sessionErrorOccuredWaitHandle = null;
					}
					if (_channel != null)
					{
						_channel.DataReceived -= Channel_DataReceived;
						_channel.ExtendedDataReceived -= Channel_ExtendedDataReceived;
						_channel.RequestReceived -= Channel_RequestReceived;
						_channel.Closed -= Channel_Closed;
						_channel.Dispose();
						_channel = null;
					}
				}
				_isDisposed = true;
			}
		}

		~SshCommand()
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
