using System;

namespace Renci.SshNet.Common
{
	public class PortForwardEventArgs : EventArgs
	{
		public string OriginatorHost
		{
			get;
			private set;
		}

		public uint OriginatorPort
		{
			get;
			private set;
		}

		internal PortForwardEventArgs(string host, uint port)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (!host.IsValidHost())
			{
				throw new ArgumentException("host");
			}
			if (!port.IsValidPort())
			{
				throw new ArgumentOutOfRangeException("port");
			}
			OriginatorHost = host;
			OriginatorPort = port;
		}
	}
}
