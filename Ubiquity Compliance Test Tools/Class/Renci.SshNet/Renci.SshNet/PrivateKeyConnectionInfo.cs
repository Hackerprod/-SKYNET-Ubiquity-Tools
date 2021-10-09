using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Renci.SshNet
{
	public class PrivateKeyConnectionInfo : ConnectionInfo, IDisposable
	{
		private bool _isDisposed;

		public ICollection<PrivateKeyFile> KeyFiles
		{
			get;
			private set;
		}

		public PrivateKeyConnectionInfo(string host, string username, params PrivateKeyFile[] keyFiles)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, ProxyTypes.None, string.Empty, 0, string.Empty, string.Empty, keyFiles)
		{
		}

		public PrivateKeyConnectionInfo(string host, int port, string username, params PrivateKeyFile[] keyFiles)
			: this(host, port, username, ProxyTypes.None, string.Empty, 0, string.Empty, string.Empty, keyFiles)
		{
		}

		public PrivateKeyConnectionInfo(string host, int port, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, params PrivateKeyFile[] keyFiles)
			: this(host, port, username, proxyType, proxyHost, proxyPort, string.Empty, string.Empty, keyFiles)
		{
		}

		public PrivateKeyConnectionInfo(string host, int port, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, params PrivateKeyFile[] keyFiles)
			: this(host, port, username, proxyType, proxyHost, proxyPort, proxyUsername, string.Empty, keyFiles)
		{
		}

		public PrivateKeyConnectionInfo(string host, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, params PrivateKeyFile[] keyFiles)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, proxyType, proxyHost, proxyPort, string.Empty, string.Empty, keyFiles)
		{
		}

		public PrivateKeyConnectionInfo(string host, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, params PrivateKeyFile[] keyFiles)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, proxyType, proxyHost, proxyPort, proxyUsername, string.Empty, keyFiles)
		{
		}

		public PrivateKeyConnectionInfo(string host, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword, params PrivateKeyFile[] keyFiles)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword, keyFiles)
		{
		}

		public PrivateKeyConnectionInfo(string host, int port, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword, params PrivateKeyFile[] keyFiles)
			: base(host, port, username, proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword, new PrivateKeyAuthenticationMethod(username, keyFiles))
		{
			KeyFiles = new Collection<PrivateKeyFile>(keyFiles);
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

		~PrivateKeyConnectionInfo()
		{
			Dispose(disposing: false);
		}
	}
}
