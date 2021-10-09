using Renci.SshNet.Common;
using System;
using System.Linq;
using System.Text;

namespace Renci.SshNet
{
	public class PasswordConnectionInfo : ConnectionInfo, IDisposable
	{
		private bool _isDisposed;

		public event EventHandler<AuthenticationPasswordChangeEventArgs> PasswordExpired;

		public PasswordConnectionInfo(string host, string username, string password)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, Encoding.UTF8.GetBytes(password))
		{
		}

		public PasswordConnectionInfo(string host, int port, string username, string password)
			: this(host, port, username, Encoding.UTF8.GetBytes(password), ProxyTypes.None, string.Empty, 0, string.Empty, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, int port, string username, string password, ProxyTypes proxyType, string proxyHost, int proxyPort)
			: this(host, port, username, Encoding.UTF8.GetBytes(password), proxyType, proxyHost, proxyPort, string.Empty, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, int port, string username, string password, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername)
			: this(host, port, username, Encoding.UTF8.GetBytes(password), proxyType, proxyHost, proxyPort, proxyUsername, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, string username, string password, ProxyTypes proxyType, string proxyHost, int proxyPort)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, Encoding.UTF8.GetBytes(password), proxyType, proxyHost, proxyPort, string.Empty, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, string username, string password, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, Encoding.UTF8.GetBytes(password), proxyType, proxyHost, proxyPort, proxyUsername, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, string username, string password, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, Encoding.UTF8.GetBytes(password), proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword)
		{
		}

		public PasswordConnectionInfo(string host, string username, byte[] password)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, password)
		{
		}

		public PasswordConnectionInfo(string host, int port, string username, byte[] password)
			: this(host, port, username, password, ProxyTypes.None, string.Empty, 0, string.Empty, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, int port, string username, byte[] password, ProxyTypes proxyType, string proxyHost, int proxyPort)
			: this(host, port, username, password, proxyType, proxyHost, proxyPort, string.Empty, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, int port, string username, byte[] password, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername)
			: this(host, port, username, password, proxyType, proxyHost, proxyPort, proxyUsername, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, string username, byte[] password, ProxyTypes proxyType, string proxyHost, int proxyPort)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, password, proxyType, proxyHost, proxyPort, string.Empty, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, string username, byte[] password, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, password, proxyType, proxyHost, proxyPort, proxyUsername, string.Empty)
		{
		}

		public PasswordConnectionInfo(string host, string username, byte[] password, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, password, proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword)
		{
		}

		public PasswordConnectionInfo(string host, int port, string username, byte[] password, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword)
			: base(host, port, username, proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword, new PasswordAuthenticationMethod(username, password))
		{
			foreach (PasswordAuthenticationMethod item in base.AuthenticationMethods.OfType<PasswordAuthenticationMethod>())
			{
				item.PasswordExpired += AuthenticationMethod_PasswordExpired;
			}
		}

		private void AuthenticationMethod_PasswordExpired(object sender, AuthenticationPasswordChangeEventArgs e)
		{
			if (this.PasswordExpired != null)
			{
				this.PasswordExpired(sender, e);
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

		~PasswordConnectionInfo()
		{
			Dispose(disposing: false);
		}
	}
}
