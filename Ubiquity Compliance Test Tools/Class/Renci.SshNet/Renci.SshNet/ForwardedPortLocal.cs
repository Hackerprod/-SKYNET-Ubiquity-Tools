using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Renci.SshNet
{
	public class ForwardedPortLocal : ForwardedPort, IDisposable
	{
		private EventWaitHandle _listenerTaskCompleted;

		private bool _isDisposed;

		private TcpListener _listener;

		private readonly object _listenerLocker = new object();

		public string BoundHost
		{
			get;
			protected set;
		}

		public uint BoundPort
		{
			get;
			protected set;
		}

		public string Host
		{
			get;
			protected set;
		}

		public uint Port
		{
			get;
			protected set;
		}

		public ForwardedPortLocal(uint boundPort, string host, uint port)
			: this(string.Empty, boundPort, host, port)
		{
		}

		public ForwardedPortLocal(string boundHost, string host, uint port)
			: this(boundHost, 0u, host, port)
		{
		}

		public ForwardedPortLocal(string boundHost, uint boundPort, string host, uint port)
		{
			if (boundHost == null)
			{
				throw new ArgumentNullException("boundHost");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (!boundHost.IsValidHost())
			{
				throw new ArgumentException("boundHost");
			}
			if (!boundPort.IsValidPort())
			{
				throw new ArgumentOutOfRangeException("boundPort");
			}
			if (!host.IsValidHost())
			{
				throw new ArgumentException("host");
			}
			if (!port.IsValidPort())
			{
				throw new ArgumentOutOfRangeException("port");
			}
			BoundHost = boundHost;
			BoundPort = boundPort;
			Host = host;
			Port = port;
		}

		public override void Start()
		{
			InternalStart();
		}

		public override void Stop()
		{
			base.Stop();
			InternalStop();
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
				InternalStop();
				if (disposing && _listenerTaskCompleted != null)
				{
					Extensions.Dispose(_listenerTaskCompleted);
					_listenerTaskCompleted = null;
				}
				_isDisposed = true;
			}
		}

		~ForwardedPortLocal()
		{
			Dispose(disposing: false);
		}

		private void InternalStart()
		{
			if (!base.IsStarted)
			{
				IPAddress iPAddress = BoundHost.GetIPAddress();
				IPEndPoint localEP = new IPEndPoint(iPAddress, (int)BoundPort);
				_listener = new TcpListener(localEP);
				_listener.Start();
				BoundPort = (uint)((IPEndPoint)_listener.LocalEndpoint).Port;
				base.Session.ErrorOccured += Session_ErrorOccured;
				base.Session.Disconnected += Session_Disconnected;
				_listenerTaskCompleted = new ManualResetEvent(initialState: false);
				ExecuteThread(delegate
				{
					try
					{
						while (true)
						{
							ForwardedPortLocal forwardedPortLocal = this;
							lock (_listenerLocker)
							{
								if (_listener == null)
								{
									return;
								}
							}
							Socket socket = _listener.AcceptSocket();
							ExecuteThread(delegate
							{
								try
								{
									IPEndPoint iPEndPoint = socket.RemoteEndPoint as IPEndPoint;
									forwardedPortLocal.RaiseRequestReceived(iPEndPoint.Address.ToString(), (uint)iPEndPoint.Port);
									using (ChannelDirectTcpip channelDirectTcpip = forwardedPortLocal.Session.CreateClientChannel<ChannelDirectTcpip>())
									{
										channelDirectTcpip.Open(forwardedPortLocal.Host, forwardedPortLocal.Port, socket);
										channelDirectTcpip.Bind();
										channelDirectTcpip.Close();
									}
								}
								catch (Exception execption2)
								{
									forwardedPortLocal.RaiseExceptionEvent(execption2);
								}
							});
						}
					}
					catch (SocketException ex)
					{
						if (ex.SocketErrorCode != SocketError.Interrupted)
						{
							RaiseExceptionEvent(ex);
						}
					}
					catch (Exception execption)
					{
						RaiseExceptionEvent(execption);
					}
					finally
					{
						_listenerTaskCompleted.Set();
					}
				});
				base.IsStarted = true;
			}
		}

		private void InternalStop()
		{
			if (base.IsStarted)
			{
				base.Session.Disconnected -= Session_Disconnected;
				base.Session.ErrorOccured -= Session_ErrorOccured;
				StopListener();
				_listenerTaskCompleted.WaitOne(base.Session.ConnectionInfo.Timeout);
				Extensions.Dispose(_listenerTaskCompleted);
				_listenerTaskCompleted = null;
				base.IsStarted = false;
			}
		}

		private void StopListener()
		{
			lock (_listenerLocker)
			{
				if (_listener != null)
				{
					_listener.Stop();
					_listener = null;
				}
			}
		}

		private void Session_ErrorOccured(object sender, ExceptionEventArgs e)
		{
			StopListener();
		}

		private void Session_Disconnected(object sender, EventArgs e)
		{
			StopListener();
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
