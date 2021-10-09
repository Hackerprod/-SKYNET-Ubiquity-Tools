using Renci.SshNet.Common;
using System;
using System.Linq;

namespace Renci.SshNet
{
	public class KeyboardInteractiveConnectionInfo : ConnectionInfo, IDisposable
	{
		private bool _isDisposed;

		public event EventHandler<AuthenticationPromptEventArgs> AuthenticationPrompt;

		public KeyboardInteractiveConnectionInfo(string host, string username)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, ProxyTypes.None, string.Empty, 0, string.Empty, string.Empty)
		{
		}

		public KeyboardInteractiveConnectionInfo(string host, int port, string username)
			: this(host, port, username, ProxyTypes.None, string.Empty, 0, string.Empty, string.Empty)
		{
		}

		public KeyboardInteractiveConnectionInfo(string host, int port, string username, ProxyTypes proxyType, string proxyHost, int proxyPort)
			: this(host, port, username, proxyType, proxyHost, proxyPort, string.Empty, string.Empty)
		{
		}

		public KeyboardInteractiveConnectionInfo(string host, int port, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername)
			: this(host, port, username, proxyType, proxyHost, proxyPort, proxyUsername, string.Empty)
		{
		}

		public KeyboardInteractiveConnectionInfo(string host, string username, ProxyTypes proxyType, string proxyHost, int proxyPort)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, proxyType, proxyHost, proxyPort, string.Empty, string.Empty)
		{
		}

		public KeyboardInteractiveConnectionInfo(string host, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, proxyType, proxyHost, proxyPort, proxyUsername, string.Empty)
		{
		}

		public KeyboardInteractiveConnectionInfo(string host, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword)
		{
		}

		public KeyboardInteractiveConnectionInfo(string host, int port, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword)
			: base(host, port, username, proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword, new KeyboardInteractiveAuthenticationMethod(username))
		{
			foreach (KeyboardInteractiveAuthenticationMethod item in base.AuthenticationMethods.OfType<KeyboardInteractiveAuthenticationMethod>())
			{
				item.AuthenticationPrompt += AuthenticationMethod_AuthenticationPrompt;
			}
		}

		private void AuthenticationMethod_AuthenticationPrompt(object sender, AuthenticationPromptEventArgs e)
		{
			if (this.AuthenticationPrompt != null)
			{
				this.AuthenticationPrompt(sender, e);
			}
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
				if (disposing && base.AuthenticationMethods != null)
				{
					foreach (IDisposable item in base.AuthenticationMethods.OfType<IDisposable>())
					{
						item.Dispose();
					}
				}
				_isDisposed = true;
			}
		}

		~KeyboardInteractiveConnectionInfo()
		{
			Dispose(disposing: false);
		}
	}
}
