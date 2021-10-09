using Renci.SshNet.Common;
using System;
using System.Threading;

namespace Renci.SshNet
{
	public abstract class BaseClient : IDisposable
	{
		private static readonly TimeSpan Infinite = new TimeSpan(0, 0, 0, 0, -1);

		private readonly bool _ownsConnectionInfo;

		private TimeSpan _keepAliveInterval;

		private Timer _keepAliveTimer;

		private ConnectionInfo _connectionInfo;

		private bool _isDisposed;

		protected Session Session
		{
			get;
			private set;
		}

		public ConnectionInfo ConnectionInfo
		{
			get
			{
				CheckDisposed();
				return _connectionInfo;
			}
			private set
			{
				_connectionInfo = value;
			}
		}

		public bool IsConnected
		{
			get
			{
				CheckDisposed();
				if (Session != null)
				{
					return Session.IsConnected;
				}
				return false;
			}
		}

		public TimeSpan KeepAliveInterval
		{
			get
			{
				CheckDisposed();
				return _keepAliveInterval;
			}
			set
			{
				CheckDisposed();
				if (!(value == _keepAliveInterval))
				{
					if (value == Infinite)
					{
						StopKeepAliveTimer();
					}
					else if (_keepAliveTimer != null)
					{
						_keepAliveTimer.Change(value, value);
					}
					_keepAliveInterval = value;
				}
			}
		}

		public event EventHandler<ExceptionEventArgs> ErrorOccurred;

		public event EventHandler<HostKeyEventArgs> HostKeyReceived;

		protected BaseClient(ConnectionInfo connectionInfo, bool ownsConnectionInfo)
		{
			if (connectionInfo == null)
			{
				throw new ArgumentNullException("connectionInfo");
			}
			ConnectionInfo = connectionInfo;
			_ownsConnectionInfo = ownsConnectionInfo;
			_keepAliveInterval = Infinite;
		}

		public void Connect()
		{
			CheckDisposed();
			if (Session != null && Session.IsConnected)
			{
				return;
			}
			OnConnecting();
			Session = new Session(ConnectionInfo);
			Session.HostKeyReceived += Session_HostKeyReceived;
			Session.ErrorOccured += Session_ErrorOccured;
			Session.Connect();
			StartKeepAliveTimer();
			OnConnected();
		}

		public void Disconnect()
		{
            return;
			CheckDisposed();
			OnDisconnecting();
			StopKeepAliveTimer();
			if (Session != null)
			{
				Session.Disconnect();
			}
			OnDisconnected();
		}

		public void SendKeepAlive()
		{
			CheckDisposed();
			if (Session != null && Session.IsConnected)
			{
				Session.SendKeepAlive();
			}
		}

		protected virtual void OnConnecting()
		{
		}

		protected virtual void OnConnected()
		{
		}

		protected virtual void OnDisconnecting()
		{
			if (Session != null)
			{
				Session.OnDisconnecting();
			}
		}

		protected virtual void OnDisconnected()
		{
		}

		private void Session_ErrorOccured(object sender, ExceptionEventArgs e)
		{
			this.ErrorOccurred?.Invoke(this, e);
		}

		private void Session_HostKeyReceived(object sender, HostKeyEventArgs e)
		{
			this.HostKeyReceived?.Invoke(this, e);
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
					StopKeepAliveTimer();
					if (Session != null)
					{
						Session.ErrorOccured -= Session_ErrorOccured;
						Session.HostKeyReceived -= Session_HostKeyReceived;
						Session.Dispose();
						Session = null;
					}
					if (_ownsConnectionInfo && _connectionInfo != null)
					{
						(_connectionInfo as IDisposable)?.Dispose();
						_connectionInfo = null;
					}
				}
				_isDisposed = true;
			}
		}

		protected void CheckDisposed()
		{
			if (_isDisposed)
			{
				//throw new ObjectDisposedException(GetType().FullName);
			}
		}

		~BaseClient()
		{
			Dispose(disposing: false);
		}

		private void StopKeepAliveTimer()
		{
			if (_keepAliveTimer != null)
			{
				ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
				_keepAliveTimer.Dispose(manualResetEvent);
				manualResetEvent.WaitOne();
				Extensions.Dispose(manualResetEvent);
				_keepAliveTimer = null;
			}
		}

		private void StartKeepAliveTimer()
		{
			if (!(_keepAliveInterval == Infinite))
			{
				if (_keepAliveTimer == null)
				{
					_keepAliveTimer = new Timer(delegate
					{
						SendKeepAlive();
					});
				}
				_keepAliveTimer.Change(_keepAliveInterval, _keepAliveInterval);
			}
		}
	}
}
