using System;

namespace Renci.SshNet.Common
{
	public abstract class AuthenticationEventArgs : EventArgs
	{
		public string Username
		{
			get;
			private set;
		}

		public AuthenticationEventArgs(string username)
		{
			Username = username;
		}
	}
}
