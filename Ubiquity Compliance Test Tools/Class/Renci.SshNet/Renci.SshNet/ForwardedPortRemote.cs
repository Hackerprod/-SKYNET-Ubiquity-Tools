using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using Renci.SshNet.Messages.Connection;
using System;
using System.Globalization;
using System.Net;
using System.Threading;

namespace Renci.SshNet
{
	public class ForwardedPortRemote : ForwardedPort, IDisposable
	{
		private bool _requestStatus;

		private EventWaitHandle _globalRequestResponse = new AutoResetEvent(initialState: false);

		private bool _isDisposed;

		public IPAddress BoundHostAddress
		{
			get;
			protected set;
		}

		public string BoundHost => BoundHostAddress.ToString();

		public uint BoundPort
		{
			get;
			protected set;
		}

		public IPAddress HostAddress
		{
			get;
			protected set;
		}

		public string Host => HostAddress.ToString();

		public uint Port
		{
			get;
			protected set;
		}

		public ForwardedPortRemote(IPAddress boundHostAddress, uint boundPort, IPAddress hostAddress, uint port)
		{
			if (boundHostAddress == null)
			{
				throw new ArgumentNullException("boundHostAddress");
			}
			if (hostAddress == null)
			{
				throw new ArgumentNullException("hostAddress");
			}
			if (!boundPort.IsValidPort())
			{
				throw new ArgumentOutOfRangeException("boundPort");
			}
			if (!port.IsValidPort())
			{
				throw new ArgumentOutOfRangeException("port");
			}
			BoundHostAddress = boundHostAddress;
			BoundPort = boundPort;
			HostAddress = hostAddress;
			Port = port;
		}

		public override void Start()
		{
			base.Start();
			if (!base.IsStarted)
			{
				base.Session.RegisterMessage("SSH_MSG_REQUEST_FAILURE");
				base.Session.RegisterMessage("SSH_MSG_REQUEST_SUCCESS");
				base.Session.RegisterMessage("SSH_MSG_CHANNEL_OPEN");
				base.Session.RequestSuccessReceived += Session_RequestSuccess;
				base.Session.RequestFailureReceived += Session_RequestFailure;
				base.Session.ChannelOpenReceived += Session_ChannelOpening;
				base.Session.SendMessage(new GlobalRequestMessage(GlobalRequestName.TcpIpForward, wantReply: true, BoundHost, BoundPort));
				base.Session.WaitOnHandle(_globalRequestResponse);
				if (!_requestStatus)
				{
					base.Session.ChannelOpenReceived -= Session_ChannelOpening;
					throw new SshException(string.Format(CultureInfo.CurrentCulture, "Port forwarding for '{0}' port '{1}' failed to start.", new object[2]
					{
						Host,
						Port
					}));
				}
				base.IsStarted = true;
			}
		}

		public override void Stop()
		{
			base.Stop();
			if (base.IsStarted)
			{
				base.Session.SendMessage(new GlobalRequestMessage(GlobalRequestName.CancelTcpIpForward, wantReply: true, BoundHost, BoundPort));
				base.Session.WaitOnHandle(_globalRequestResponse);
				base.Session.RequestSuccessReceived -= Session_RequestSuccess;
				base.Session.RequestFailureReceived -= Session_RequestFailure;
				base.Session.ChannelOpenReceived -= Session_ChannelOpening;
				base.IsStarted = false;
			}
		}

		private void Session_ChannelOpening(object sender, MessageEventArgs<ChannelOpenMessage> e)
		{
			ChannelOpenMessage channelOpenMessage = e.Message;
			ForwardedTcpipChannelInfo info = channelOpenMessage.Info as ForwardedTcpipChannelInfo;
			if (info != null && info.ConnectedAddress == BoundHost && info.ConnectedPort == BoundPort)
			{
				ExecuteThread(delegate
				{
					try
					{
						RaiseRequestReceived(info.OriginatorAddress, info.OriginatorPort);
						ChannelForwardedTcpip channelForwardedTcpip = base.Session.CreateServerChannel<ChannelForwardedTcpip>(channelOpenMessage.LocalChannelNumber, channelOpenMessage.InitialWindowSize, channelOpenMessage.MaximumPacketSize);
						channelForwardedTcpip.Bind(HostAddress, Port);
					}
					catch (Exception execption)
					{
						RaiseExceptionEvent(execption);
					}
				});
			}
		}

		private void Session_RequestFailure(object sender, EventArgs e)
		{
			_requestStatus = false;
			_globalRequestResponse.Set();
		}

		private void Session_RequestSuccess(object sender, MessageEventArgs<RequestSuccessMessage> e)
		{
			_requestStatus = true;
			if (BoundPort == 0)
			{
				BoundPort = (e.Message.BoundPort.HasValue ? e.Message.BoundPort.Value : 0);
			}
			_globalRequestResponse.Set();
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
				if (disposing && _globalRequestResponse != null)
				{
					Extensions.Dispose(_globalRequestResponse);
					_globalRequestResponse = null;
				}
				_isDisposed = true;
			}
		}

		~ForwardedPortRemote()
		{
			Dispose(disposing: false);
		}

		public ForwardedPortRemote(uint boundPort, string host, uint port)
			: this(string.Empty, boundPort, host, port)
		{
		}

		public ForwardedPortRemote(string boundHost, uint boundPort, string host, uint port)
			: this(Dns.GetHostEntry(boundHost).AddressList[0], boundPort, Dns.GetHostEntry(host).AddressList[0], port)
		{
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
