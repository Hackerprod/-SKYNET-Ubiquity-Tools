using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Renci.SshNet.Sftp
{
	public abstract class SubsystemSession : IDisposable
	{
		private readonly Session _session;

		private readonly string _subsystemName;

		private ChannelSession _channel;

		private Exception _exception;

		private EventWaitHandle _errorOccuredWaitHandle = new ManualResetEvent(initialState: false);

		private EventWaitHandle _channelClosedWaitHandle = new ManualResetEvent(initialState: false);

		protected TimeSpan _operationTimeout;

		private bool _isDisposed;

		internal ChannelSession Channel => _channel;

		protected Encoding Encoding
		{
			get;
			private set;
		}

		public event EventHandler<ExceptionEventArgs> ErrorOccurred;

		public event EventHandler<EventArgs> Disconnected;

		protected SubsystemSession(Session session, string subsystemName, TimeSpan operationTimeout, Encoding encoding)
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			if (subsystemName == null)
			{
				throw new ArgumentNullException("subsystemName");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			_session = session;
			_subsystemName = subsystemName;
			_operationTimeout = operationTimeout;
			Encoding = encoding;
		}

		public void Connect()
		{
			_channel = _session.CreateClientChannel<ChannelSession>();
			_session.ErrorOccured += Session_ErrorOccured;
			_session.Disconnected += Session_Disconnected;
			_channel.DataReceived += Channel_DataReceived;
			_channel.Closed += Channel_Closed;
			_channel.Open();
			_channel.SendSubsystemRequest(_subsystemName);
			OnChannelOpen();
		}

		public void Disconnect()
		{
			_channel.SendEof();
			_channel.Close();
		}

		public void SendData(byte[] data)
		{
			_channel.SendData(data);
		}

		protected abstract void OnChannelOpen();

		protected abstract void OnDataReceived(uint dataTypeCode, byte[] data);

		protected void RaiseError(Exception error)
		{
			_exception = error;
			_errorOccuredWaitHandle?.Set();
			SignalErrorOccurred(error);
		}

		private void Channel_DataReceived(object sender, ChannelDataEventArgs e)
		{
			OnDataReceived(e.DataTypeCode, e.Data);
		}

		private void Channel_Closed(object sender, ChannelEventArgs e)
		{
			_channelClosedWaitHandle?.Set();
		}

		internal void WaitOnHandle(WaitHandle waitHandle, TimeSpan operationTimeout)
		{
			WaitHandle[] waitHandles = new WaitHandle[3]
			{
				_errorOccuredWaitHandle,
				_channelClosedWaitHandle,
				waitHandle
			};
			switch (WaitHandle.WaitAny(waitHandles, operationTimeout))
			{
			case 0:
				throw _exception;
			case 1:
				throw new SshException("Channel was closed.");
			case 258:
				throw new SshOperationTimeoutException(string.Format(CultureInfo.CurrentCulture, "Operation has timed out."));
			}
		}

		private void Session_Disconnected(object sender, EventArgs e)
		{
			SignalDisconnected();
			RaiseError(new SshException("Connection was lost"));
		}

		private void Session_ErrorOccured(object sender, ExceptionEventArgs e)
		{
			RaiseError(e.Exception);
		}

		private void SignalErrorOccurred(Exception error)
		{
			this.ErrorOccurred?.Invoke(this, new ExceptionEventArgs(error));
		}

		private void SignalDisconnected()
		{
			this.Disconnected?.Invoke(this, new EventArgs());
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
				if (_channel != null)
				{
					_channel.DataReceived -= Channel_DataReceived;
					_channel.Closed -= Channel_Closed;
					_channel.Dispose();
					_channel = null;
				}
				_session.ErrorOccured -= Session_ErrorOccured;
				_session.Disconnected -= Session_Disconnected;
				if (disposing)
				{
					if (_errorOccuredWaitHandle != null)
					{
						Extensions.Dispose(_errorOccuredWaitHandle);
						_errorOccuredWaitHandle = null;
					}
					if (_channelClosedWaitHandle != null)
					{
						Extensions.Dispose(_channelClosedWaitHandle);
						_channelClosedWaitHandle = null;
					}
				}
				_isDisposed = true;
			}
		}

		~SubsystemSession()
		{
			Dispose(disposing: false);
		}
	}
}
