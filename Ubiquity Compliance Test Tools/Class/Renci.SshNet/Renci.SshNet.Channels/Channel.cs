using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Connection;
using System;
using System.Globalization;
using System.Threading;

namespace Renci.SshNet.Channels
{
	internal abstract class Channel : IDisposable
	{
		private EventWaitHandle _channelClosedWaitHandle = new ManualResetEvent(initialState: false);

		private EventWaitHandle _channelServerWindowAdjustWaitHandle = new ManualResetEvent(initialState: false);

		private EventWaitHandle _errorOccuredWaitHandle = new ManualResetEvent(initialState: false);

		private EventWaitHandle _disconnectedWaitHandle = new ManualResetEvent(initialState: false);

		private readonly object _serverWindowSizeLock = new object();

		private bool _closeMessageSent;

		private uint _initialWindowSize;

		private uint? _remoteWindowSize;

		private uint? _remoteChannelNumber;

		private uint? _remotePacketSize;

		private Session _session;

		private bool _isDisposed;

		protected Session Session => _session;

		public abstract ChannelTypes ChannelType
		{
			get;
		}

		public uint LocalChannelNumber
		{
			get;
			private set;
		}

		public uint LocalPacketSize
		{
			get;
			private set;
		}

		public uint LocalWindowSize
		{
			get;
			private set;
		}

		public uint RemoteChannelNumber
		{
			get
			{
				if (!_remoteChannelNumber.HasValue)
				{
					throw CreateRemoteChannelInfoNotAvailableException();
				}
				return _remoteChannelNumber.Value;
			}
			private set
			{
				_remoteChannelNumber = value;
			}
		}

		public uint RemotePacketSize
		{
			get
			{
				if (!_remotePacketSize.HasValue)
				{
					throw CreateRemoteChannelInfoNotAvailableException();
				}
				return _remotePacketSize.Value;
			}
			private set
			{
				_remotePacketSize = value;
			}
		}

		public uint RemoteWindowSize
		{
			get
			{
				if (!_remoteWindowSize.HasValue)
				{
					throw CreateRemoteChannelInfoNotAvailableException();
				}
				return _remoteWindowSize.Value;
			}
			private set
			{
				_remoteWindowSize = value;
			}
		}

		public bool IsOpen
		{
			get;
			protected set;
		}

		protected bool IsConnected => _session.IsConnected;

		protected ConnectionInfo ConnectionInfo => _session.ConnectionInfo;

		protected SemaphoreLight SessionSemaphore => _session.SessionSemaphore;

		public event EventHandler<ChannelDataEventArgs> DataReceived;

		public event EventHandler<ChannelDataEventArgs> ExtendedDataReceived;

		public event EventHandler<ChannelEventArgs> EndOfData;

		public event EventHandler<ChannelEventArgs> Closed;

		public event EventHandler<ChannelRequestEventArgs> RequestReceived;

		public event EventHandler<ChannelEventArgs> RequestSuccessed;

		public event EventHandler<ChannelEventArgs> RequestFailed;

		internal virtual void Initialize(Session session, uint localWindowSize, uint localPacketSize)
		{
			_session = session;
			_initialWindowSize = localWindowSize;
			LocalPacketSize = localPacketSize;
			LocalWindowSize = localWindowSize;
			LocalChannelNumber = session.NextChannelNumber;
			_session.ChannelWindowAdjustReceived += OnChannelWindowAdjust;
			_session.ChannelDataReceived += OnChannelData;
			_session.ChannelExtendedDataReceived += OnChannelExtendedData;
			_session.ChannelEofReceived += OnChannelEof;
			_session.ChannelCloseReceived += OnChannelClose;
			_session.ChannelRequestReceived += OnChannelRequest;
			_session.ChannelSuccessReceived += OnChannelSuccess;
			_session.ChannelFailureReceived += OnChannelFailure;
			_session.ErrorOccured += Session_ErrorOccured;
			_session.Disconnected += Session_Disconnected;
		}

		protected void InitializeRemoteInfo(uint remoteChannelNumber, uint remoteWindowSize, uint remotePacketSize)
		{
			RemoteChannelNumber = remoteChannelNumber;
			RemoteWindowSize = remoteWindowSize;
			RemotePacketSize = remotePacketSize;
		}

		internal void SendEof()
		{
			SendMessage(new ChannelEofMessage(RemoteChannelNumber));
		}

		internal void SendData(byte[] buffer)
		{
			SendMessage(new ChannelDataMessage(RemoteChannelNumber, buffer));
		}

		public virtual void Close()
		{
			Close(wait: true);
		}

		protected virtual void OnWindowAdjust(uint bytesToAdd)
		{
			lock (_serverWindowSizeLock)
			{
				RemoteWindowSize += bytesToAdd;
			}
			_channelServerWindowAdjustWaitHandle.Set();
		}

		protected virtual void OnData(byte[] data)
		{
			AdjustDataWindow(data);
			this.DataReceived?.Invoke(this, new ChannelDataEventArgs(LocalChannelNumber, data));
		}

		protected virtual void OnExtendedData(byte[] data, uint dataTypeCode)
		{
			AdjustDataWindow(data);
			this.ExtendedDataReceived?.Invoke(this, new ChannelDataEventArgs(LocalChannelNumber, data, dataTypeCode));
		}

		protected virtual void OnEof()
		{
			this.EndOfData?.Invoke(this, new ChannelEventArgs(LocalChannelNumber));
		}

		protected virtual void OnClose()
		{
			Close(wait: false);
			this.Closed?.Invoke(this, new ChannelEventArgs(LocalChannelNumber));
		}

		protected virtual void OnRequest(RequestInfo info)
		{
			this.RequestReceived?.Invoke(this, new ChannelRequestEventArgs(info));
		}

		protected virtual void OnSuccess()
		{
			this.RequestSuccessed?.Invoke(this, new ChannelEventArgs(LocalChannelNumber));
		}

		protected virtual void OnFailure()
		{
			this.RequestFailed?.Invoke(this, new ChannelEventArgs(LocalChannelNumber));
		}

		protected void SendMessage(Message message)
		{
			if (IsOpen)
			{
				_session.SendMessage(message);
			}
		}

		private void SendMessage(ChannelCloseMessage message)
		{
			if (IsOpen)
			{
				_session.SendMessage(message);
				IsOpen = false;
			}
		}

		protected void SendMessage(ChannelDataMessage message)
		{
			if (IsOpen)
			{
				int num = message.Data.Length;
				int num2 = 0;
				int dataLengthThatCanBeSentInMessage;
				for (int num3 = num; num3 > 0; num3 -= dataLengthThatCanBeSentInMessage)
				{
					dataLengthThatCanBeSentInMessage = GetDataLengthThatCanBeSentInMessage(num3);
					if (dataLengthThatCanBeSentInMessage == num)
					{
						_session.SendMessage(message);
					}
					else
					{
						byte[] array = new byte[dataLengthThatCanBeSentInMessage];
						Array.Copy(message.Data, num2, array, 0, dataLengthThatCanBeSentInMessage);
						_session.SendMessage(new ChannelDataMessage(message.LocalChannelNumber, array));
					}
					num2 += dataLengthThatCanBeSentInMessage;
				}
			}
		}

		protected void SendMessage(ChannelExtendedDataMessage message)
		{
			if (IsOpen)
			{
				int num = message.Data.Length;
				int num2 = 0;
				int dataLengthThatCanBeSentInMessage;
				for (int num3 = num; num3 > 0; num3 -= dataLengthThatCanBeSentInMessage)
				{
					dataLengthThatCanBeSentInMessage = GetDataLengthThatCanBeSentInMessage(num3);
					if (dataLengthThatCanBeSentInMessage == num)
					{
						_session.SendMessage(message);
					}
					else
					{
						byte[] array = new byte[dataLengthThatCanBeSentInMessage];
						Array.Copy(message.Data, num2, array, 0, dataLengthThatCanBeSentInMessage);
						_session.SendMessage(new ChannelExtendedDataMessage(message.LocalChannelNumber, message.DataTypeCode, array));
					}
					num2 += dataLengthThatCanBeSentInMessage;
				}
			}
		}

		protected void WaitOnHandle(WaitHandle waitHandle)
		{
			_session.WaitOnHandle(waitHandle);
		}

		protected virtual void Close(bool wait)
		{
			if (!_closeMessageSent && IsConnected)
			{
				lock (this)
				{
					if (!_closeMessageSent)
					{
						SendMessage(new ChannelCloseMessage(RemoteChannelNumber));
						_closeMessageSent = true;
					}
				}
			}
			else
			{
				IsOpen = false;
			}
			if (wait)
			{
				WaitOnHandle(_channelClosedWaitHandle);
			}
		}

		protected virtual void OnDisconnected()
		{
		}

		protected virtual void OnErrorOccured(Exception exp)
		{
		}

		private void Session_Disconnected(object sender, EventArgs e)
		{
			OnDisconnected();
			if (!_isDisposed)
			{
				_disconnectedWaitHandle?.Set();
			}
		}

		private void Session_ErrorOccured(object sender, ExceptionEventArgs e)
		{
			OnErrorOccured(e.Exception);
			if (!_isDisposed)
			{
				_errorOccuredWaitHandle?.Set();
			}
		}

		private void OnChannelWindowAdjust(object sender, MessageEventArgs<ChannelWindowAdjustMessage> e)
		{
			if (e.Message.LocalChannelNumber == LocalChannelNumber)
			{
				OnWindowAdjust(e.Message.BytesToAdd);
			}
		}

		private void OnChannelData(object sender, MessageEventArgs<ChannelDataMessage> e)
		{
			if (e.Message.LocalChannelNumber == LocalChannelNumber)
			{
				OnData(e.Message.Data);
			}
		}

		private void OnChannelExtendedData(object sender, MessageEventArgs<ChannelExtendedDataMessage> e)
		{
			if (e.Message.LocalChannelNumber == LocalChannelNumber)
			{
				OnExtendedData(e.Message.Data, e.Message.DataTypeCode);
			}
		}

		private void OnChannelEof(object sender, MessageEventArgs<ChannelEofMessage> e)
		{
			if (e.Message.LocalChannelNumber == LocalChannelNumber)
			{
				OnEof();
			}
		}

		private void OnChannelClose(object sender, MessageEventArgs<ChannelCloseMessage> e)
		{
			if (e.Message.LocalChannelNumber == LocalChannelNumber)
			{
				OnClose();
				_channelClosedWaitHandle?.Set();
			}
		}

		private void OnChannelRequest(object sender, MessageEventArgs<ChannelRequestMessage> e)
		{
			if (e.Message.LocalChannelNumber == LocalChannelNumber)
			{
				if (!_session.ConnectionInfo.ChannelRequests.ContainsKey(e.Message.RequestName))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Request '{0}' is not supported.", new object[1]
					{
						e.Message.RequestName
					}));
				}
				RequestInfo requestInfo = _session.ConnectionInfo.ChannelRequests[e.Message.RequestName];
				requestInfo.Load(e.Message.RequestData);
				OnRequest(requestInfo);
			}
		}

		private void OnChannelSuccess(object sender, MessageEventArgs<ChannelSuccessMessage> e)
		{
			if (e.Message.LocalChannelNumber == LocalChannelNumber)
			{
				OnSuccess();
			}
		}

		private void OnChannelFailure(object sender, MessageEventArgs<ChannelFailureMessage> e)
		{
			if (e.Message.LocalChannelNumber == LocalChannelNumber)
			{
				OnFailure();
			}
		}

		private void AdjustDataWindow(byte[] messageData)
		{
			LocalWindowSize -= (uint)messageData.Length;
			if (LocalWindowSize < LocalPacketSize)
			{
				SendMessage(new ChannelWindowAdjustMessage(RemoteChannelNumber, _initialWindowSize - LocalWindowSize));
				LocalWindowSize = _initialWindowSize;
			}
		}

		private int GetDataLengthThatCanBeSentInMessage(int messageLength)
		{
			while (true)
			{
				lock (_serverWindowSizeLock)
				{
					uint remoteWindowSize = RemoteWindowSize;
					if (remoteWindowSize != 0)
					{
						uint num = Math.Min(Math.Min(RemotePacketSize, (uint)messageLength), remoteWindowSize);
						RemoteWindowSize -= num;
						return (int)num;
					}
					_channelServerWindowAdjustWaitHandle.Reset();
				}
				WaitOnHandle(_channelServerWindowAdjustWaitHandle);
			}
		}

		private InvalidOperationException CreateRemoteChannelInfoNotAvailableException()
		{
			throw new InvalidOperationException("The channel has not been opened, or the open has not yet been confirmed.");
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
					Close(wait: false);
					if (_channelClosedWaitHandle != null)
					{
						Extensions.Dispose(_channelClosedWaitHandle);
						_channelClosedWaitHandle = null;
					}
					if (_channelServerWindowAdjustWaitHandle != null)
					{
						Extensions.Dispose(_channelServerWindowAdjustWaitHandle);
						_channelServerWindowAdjustWaitHandle = null;
					}
					if (_errorOccuredWaitHandle != null)
					{
						Extensions.Dispose(_errorOccuredWaitHandle);
						_errorOccuredWaitHandle = null;
					}
					if (_disconnectedWaitHandle != null)
					{
						Extensions.Dispose(_disconnectedWaitHandle);
						_disconnectedWaitHandle = null;
					}
				}
				_session.ChannelWindowAdjustReceived -= OnChannelWindowAdjust;
				_session.ChannelDataReceived -= OnChannelData;
				_session.ChannelExtendedDataReceived -= OnChannelExtendedData;
				_session.ChannelEofReceived -= OnChannelEof;
				_session.ChannelCloseReceived -= OnChannelClose;
				_session.ChannelRequestReceived -= OnChannelRequest;
				_session.ChannelSuccessReceived -= OnChannelSuccess;
				_session.ChannelFailureReceived -= OnChannelFailure;
				_session.ErrorOccured -= Session_ErrorOccured;
				_session.Disconnected -= Session_Disconnected;
				_isDisposed = true;
			}
		}

		~Channel()
		{
			Dispose(disposing: false);
		}
	}
}
