using Renci.SshNet.NetConf;
using System;
using System.Xml;

namespace Renci.SshNet
{
	public class NetConfClient : BaseClient
	{
		private NetConfSession _netConfSession;

		public TimeSpan OperationTimeout
		{
			get;
			set;
		}

		public XmlDocument ServerCapabilities => _netConfSession.ServerCapabilities;

		public XmlDocument ClientCapabilities => _netConfSession.ClientCapabilities;

		public bool AutomaticMessageIdHandling
		{
			get;
			set;
		}

		public NetConfClient(ConnectionInfo connectionInfo)
			: this(connectionInfo, ownsConnectionInfo: false)
		{
		}

		public NetConfClient(string host, int port, string username, string password)
			: this(new PasswordConnectionInfo(host, port, username, password), ownsConnectionInfo: true)
		{
		}

		public NetConfClient(string host, string username, string password)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, password)
		{
		}

		public NetConfClient(string host, int port, string username, params PrivateKeyFile[] keyFiles)
			: this(new PrivateKeyConnectionInfo(host, port, username, keyFiles), ownsConnectionInfo: true)
		{
		}

		public NetConfClient(string host, string username, params PrivateKeyFile[] keyFiles)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, keyFiles)
		{
		}

		private NetConfClient(ConnectionInfo connectionInfo, bool ownsConnectionInfo)
			: base(connectionInfo, ownsConnectionInfo)
		{
			OperationTimeout = new TimeSpan(0, 0, 0, 0, -1);
			AutomaticMessageIdHandling = true;
		}

		public XmlDocument SendReceiveRpc(XmlDocument rpc)
		{
			return _netConfSession.SendReceiveRpc(rpc, AutomaticMessageIdHandling);
		}

		public XmlDocument SendReceiveRpc(string xml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			return SendReceiveRpc(xmlDocument);
		}

		public XmlDocument SendCloseRpc()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><rpc message-id=\"6666\" xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\"><close-session/></rpc>");
			return _netConfSession.SendReceiveRpc(xmlDocument, AutomaticMessageIdHandling);
		}

		protected override void OnConnected()
		{
			base.OnConnected();
			_netConfSession = new NetConfSession(base.Session, OperationTimeout);
			_netConfSession.Connect();
		}

		protected override void OnDisconnecting()
		{
			base.OnDisconnecting();
			_netConfSession.Disconnect();
		}

		protected override void Dispose(bool disposing)
		{
			if (_netConfSession != null)
			{
				_netConfSession.Dispose();
				_netConfSession = null;
			}
			base.Dispose(disposing);
		}
	}
}
