using Renci.SshNet.Messages.Connection;
using System;

namespace Renci.SshNet.Common
{
	internal class ChannelRequestEventArgs : EventArgs
	{
		public RequestInfo Info
		{
			get;
			private set;
		}

		public ChannelRequestEventArgs(RequestInfo info)
		{
			Info = info;
		}
	}
}
