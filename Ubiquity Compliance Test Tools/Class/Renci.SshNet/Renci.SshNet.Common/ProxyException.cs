using System;

namespace Renci.SshNet.Common
{
	public class ProxyException : SshException
	{
		public ProxyException()
		{
		}

		public ProxyException(string message)
			: base(message)
		{
		}

		public ProxyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
