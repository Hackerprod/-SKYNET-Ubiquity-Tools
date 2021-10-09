using Renci.SshNet.Common;
using Renci.SshNet.Messages.Authentication;
using Renci.SshNet.Messages.Connection;
using Renci.SshNet.Security;
using Renci.SshNet.Security.Cryptography;
using Renci.SshNet.Security.Cryptography.Ciphers;
using Renci.SshNet.Security.Cryptography.Ciphers.Modes;
using SKYNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renci.SshNet
{
	public class ConnectionInfo
	{
		internal static int DEFAULT_PORT = 22;

		public IDictionary<string, Type> KeyExchangeAlgorithms
		{
			get;
			private set;
		}

		public IDictionary<string, CipherInfo> Encryptions
		{
			get;
			private set;
		}

		public IDictionary<string, HashInfo> HmacAlgorithms
		{
			get;
			private set;
		}

		public IDictionary<string, Func<byte[], KeyHostAlgorithm>> HostKeyAlgorithms
		{
			get;
			private set;
		}

		public IEnumerable<AuthenticationMethod> AuthenticationMethods
		{
			get;
			private set;
		}

		public IDictionary<string, Type> CompressionAlgorithms
		{
			get;
			private set;
		}

		public IDictionary<string, RequestInfo> ChannelRequests
		{
			get;
			private set;
		}

		public bool IsAuthenticated
		{
			get;
			private set;
		}

		public string Host
		{
			get;
			private set;
		}

		public int Port
		{
			get;
			private set;
		}

		public string Username
		{
			get;
			private set;
		}

		public ProxyTypes ProxyType
		{
			get;
			private set;
		}

		public string ProxyHost
		{
			get;
			private set;
		}

		public int ProxyPort
		{
			get;
			private set;
		}

		public string ProxyUsername
		{
			get;
			private set;
		}

		public string ProxyPassword
		{
			get;
			private set;
		}

		public TimeSpan Timeout
		{
			get;
			set;
		}

		public Encoding Encoding
		{
			get;
			set;
		}

		public int RetryAttempts
		{
			get;
			set;
		}

		public int MaxSessions
		{
			get;
			set;
		}

		public string CurrentKeyExchangeAlgorithm
		{
			get;
			internal set;
		}

		public string CurrentServerEncryption
		{
			get;
			internal set;
		}

		public string CurrentClientEncryption
		{
			get;
			internal set;
		}

		public string CurrentServerHmacAlgorithm
		{
			get;
			internal set;
		}

		public string CurrentClientHmacAlgorithm
		{
			get;
			internal set;
		}

		public string CurrentHostKeyAlgorithm
		{
			get;
			internal set;
		}

		public string CurrentServerCompressionAlgorithm
		{
			get;
			internal set;
		}

		public string ServerVersion
		{
			get;
			internal set;
		}

		public string ClientVersion
		{
			get;
			internal set;
		}

		public string CurrentClientCompressionAlgorithm
		{
			get;
			internal set;
		}

		public event EventHandler<AuthenticationBannerEventArgs> AuthenticationBanner;

		public ConnectionInfo(string host, string username, params AuthenticationMethod[] authenticationMethods)
			: this(host, DEFAULT_PORT, username, ProxyTypes.None, null, 0, null, null, authenticationMethods)
		{
		}

		public ConnectionInfo(string host, int port, string username, params AuthenticationMethod[] authenticationMethods)
			: this(host, port, username, ProxyTypes.None, null, 0, null, null, authenticationMethods)
		{
		}

		public ConnectionInfo(string host, int port, string username, ProxyTypes proxyType, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword, params AuthenticationMethod[] authenticationMethods)
		{
			if (!host.IsValidHost())
			{
				throw new ArgumentException("host");
			}
			if (proxyType != 0)
			{
				if (string.IsNullOrEmpty(proxyHost) && !proxyHost.IsValidHost())
				{
					throw new ArgumentException("proxyHost");
				}
				if (!proxyPort.IsValidPort())
				{
					throw new ArgumentOutOfRangeException("proxyPort");
				}
			}
			if (!port.IsValidPort())
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (username.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("username");
			}
			if (authenticationMethods == null || authenticationMethods.Length < 1)
			{
				throw new ArgumentException("authenticationMethods");
			}
			Timeout = TimeSpan.FromSeconds(30.0);
			RetryAttempts = 10;
			MaxSessions = 10;
			Encoding = Encoding.UTF8;
			KeyExchangeAlgorithms = new Dictionary<string, Type>
			{
				{
					"diffie-hellman-group-exchange-sha256",
					typeof(KeyExchangeDiffieHellmanGroupExchangeSha256)
				},
				{
					"diffie-hellman-group-exchange-sha1",
					typeof(KeyExchangeDiffieHellmanGroupExchangeSha1)
				},
				{
					"diffie-hellman-group14-sha1",
					typeof(KeyExchangeDiffieHellmanGroup14Sha1)
				},
				{
					"diffie-hellman-group1-sha1",
					typeof(KeyExchangeDiffieHellmanGroup1Sha1)
				}
			};
			Encryptions = new Dictionary<string, CipherInfo>
			{
				{
					"aes256-ctr",
					new CipherInfo(256, (byte[] key, byte[] iv) => new AesCipher(key, new CtrCipherMode(iv), null))
				},
				{
					"3des-cbc",
					new CipherInfo(192, (byte[] key, byte[] iv) => new TripleDesCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"aes128-cbc",
					new CipherInfo(128, (byte[] key, byte[] iv) => new AesCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"aes192-cbc",
					new CipherInfo(192, (byte[] key, byte[] iv) => new AesCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"aes256-cbc",
					new CipherInfo(256, (byte[] key, byte[] iv) => new AesCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"blowfish-cbc",
					new CipherInfo(128, (byte[] key, byte[] iv) => new BlowfishCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"twofish-cbc",
					new CipherInfo(256, (byte[] key, byte[] iv) => new TwofishCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"twofish192-cbc",
					new CipherInfo(192, (byte[] key, byte[] iv) => new TwofishCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"twofish128-cbc",
					new CipherInfo(128, (byte[] key, byte[] iv) => new TwofishCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"twofish256-cbc",
					new CipherInfo(256, (byte[] key, byte[] iv) => new TwofishCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"arcfour",
					new CipherInfo(128, (byte[] key, byte[] iv) => new Arc4Cipher(key, dischargeFirstBytes: false))
				},
				{
					"arcfour128",
					new CipherInfo(128, (byte[] key, byte[] iv) => new Arc4Cipher(key, dischargeFirstBytes: true))
				},
				{
					"arcfour256",
					new CipherInfo(256, (byte[] key, byte[] iv) => new Arc4Cipher(key, dischargeFirstBytes: true))
				},
				{
					"cast128-cbc",
					new CipherInfo(128, (byte[] key, byte[] iv) => new CastCipher(key, new CbcCipherMode(iv), null))
				},
				{
					"aes128-ctr",
					new CipherInfo(128, (byte[] key, byte[] iv) => new AesCipher(key, new CtrCipherMode(iv), null))
				},
				{
					"aes192-ctr",
					new CipherInfo(192, (byte[] key, byte[] iv) => new AesCipher(key, new CtrCipherMode(iv), null))
				}
			};
			HmacAlgorithms = new Dictionary<string, HashInfo>
			{
				{
					"hmac-md5",
					new HashInfo(128, (byte[] key) => new HMac<MD5Hash>(key))
				},
				{
					"hmac-sha1",
					new HashInfo(160, (byte[] key) => new HMac<SHA1Hash>(key))
				},
				{
					"hmac-sha2-256",
					new HashInfo(256, (byte[] key) => new HMac<SHA256Hash>(key))
				},
				{
					"hmac-sha2-256-96",
					new HashInfo(256, (byte[] key) => new HMac<SHA256Hash>(key, 96))
				},
				{
					"hmac-ripemd160",
					new HashInfo(160, (byte[] key) => new HMac<RIPEMD160Hash>(key))
				},
				{
					"hmac-ripemd160@openssh.com",
					new HashInfo(160, (byte[] key) => new HMac<RIPEMD160Hash>(key))
				},
				{
					"hmac-md5-96",
					new HashInfo(128, (byte[] key) => new HMac<MD5Hash>(key, 96))
				},
				{
					"hmac-sha1-96",
					new HashInfo(160, (byte[] key) => new HMac<SHA1Hash>(key, 96))
				}
			};
			HostKeyAlgorithms = new Dictionary<string, Func<byte[], KeyHostAlgorithm>>
			{
				{
					"ssh-rsa",
					(byte[] data) => new KeyHostAlgorithm("ssh-rsa", new RsaKey(), data)
				},
				{
					"ssh-dss",
					(byte[] data) => new KeyHostAlgorithm("ssh-dss", new DsaKey(), data)
				}
			};
			CompressionAlgorithms = new Dictionary<string, Type>
			{
				{
					"none",
					null
				}
			};
			ChannelRequests = new Dictionary<string, RequestInfo>
			{
				{
					"env",
					new EnvironmentVariableRequestInfo()
				},
				{
					"exec",
					new ExecRequestInfo()
				},
				{
					"exit-signal",
					new ExitSignalRequestInfo()
				},
				{
					"exit-status",
					new ExitStatusRequestInfo()
				},
				{
					"pty-req",
					new PseudoTerminalRequestInfo()
				},
				{
					"shell",
					new ShellRequestInfo()
				},
				{
					"signal",
					new SignalRequestInfo()
				},
				{
					"subsystem",
					new SubsystemRequestInfo()
				},
				{
					"window-change",
					new WindowChangeRequestInfo()
				},
				{
					"x11-req",
					new X11ForwardingRequestInfo()
				},
				{
					"xon-xoff",
					new XonXoffRequestInfo()
				},
				{
					"eow@openssh.com",
					new EndOfWriteRequestInfo()
				},
				{
					"keepalive@openssh.com",
					new KeepAliveRequestInfo()
				}
			};
			Host = host;
			Port = port;
			Username = username;
			ProxyType = proxyType;
			ProxyHost = proxyHost;
			ProxyPort = proxyPort;
			ProxyUsername = proxyUsername;
			ProxyPassword = proxyPassword;
			AuthenticationMethods = authenticationMethods;
		}

		public void Authenticate(Session session)
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			session.RegisterMessage("SSH_MSG_USERAUTH_FAILURE");
			session.RegisterMessage("SSH_MSG_USERAUTH_SUCCESS");
			session.RegisterMessage("SSH_MSG_USERAUTH_BANNER");
			session.UserAuthenticationBannerReceived += Session_UserAuthenticationBannerReceived;
			try
			{
				SshAuthenticationException authenticationException = null;
				NoneAuthenticationMethod noneAuthenticationMethod = new NoneAuthenticationMethod(Username);
				AuthenticationResult authenticationResult = noneAuthenticationMethod.Authenticate(session);
				if (authenticationResult != 0)
				{
					List<AuthenticationMethod> failedAuthenticationMethods = new List<AuthenticationMethod>();
					if (TryAuthenticate(session, noneAuthenticationMethod.AllowedAuthentications.ToList(), failedAuthenticationMethods, ref authenticationException))
					{
						authenticationResult = AuthenticationResult.Success;
					}
				}
				IsAuthenticated = (authenticationResult == AuthenticationResult.Success);
				if (!IsAuthenticated)
				{
				    frmMain.frm.Write("No se ha podido autentificar... usuario o contraseña incorrecta.", SKYNET.LOG.MessageType.ERROR);
                    frmMain.frm.Connected = false;
				}
			}
			finally
			{
				session.UserAuthenticationBannerReceived -= Session_UserAuthenticationBannerReceived;
				session.UnRegisterMessage("SSH_MSG_USERAUTH_FAILURE");
				session.UnRegisterMessage("SSH_MSG_USERAUTH_SUCCESS");
				session.UnRegisterMessage("SSH_MSG_USERAUTH_BANNER");
			}
		}

		private bool TryAuthenticate(Session session, IList<string> allowedAuthenticationMethods, IList<AuthenticationMethod> failedAuthenticationMethods, ref SshAuthenticationException authenticationException)
		{
			if (!allowedAuthenticationMethods.Any())
			{
				authenticationException = new SshAuthenticationException("No authentication methods defined on SSH server.");
				return false;
			}
			List<AuthenticationMethod> list = (from a in AuthenticationMethods
			where allowedAuthenticationMethods.Contains(a.Name)
			select a).ToList();
			if (!list.Any())
			{
				authenticationException = new SshAuthenticationException(string.Format("No suitable authentication method found to complete authentication ({0}).", string.Join(",", allowedAuthenticationMethods.ToArray())));
				return false;
			}
			foreach (AuthenticationMethod item in list)
			{
				if (!failedAuthenticationMethods.Contains(item))
				{
					AuthenticationResult authenticationResult = item.Authenticate(session);
					switch (authenticationResult)
					{
					case AuthenticationResult.PartialSuccess:
						if (TryAuthenticate(session, item.AllowedAuthentications.ToList(), failedAuthenticationMethods, ref authenticationException))
						{
							authenticationResult = AuthenticationResult.Success;
						}
						break;
					case AuthenticationResult.Failure:
						failedAuthenticationMethods.Add(item);
						authenticationException = new SshAuthenticationException($"Permission denied ({item.Name}).");
						break;
					case AuthenticationResult.Success:
						authenticationException = null;
						break;
					}
					if (authenticationResult == AuthenticationResult.Success)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void Session_UserAuthenticationBannerReceived(object sender, MessageEventArgs<BannerMessage> e)
		{
			if (this.AuthenticationBanner != null)
			{
				this.AuthenticationBanner(this, new AuthenticationBannerEventArgs(Username, e.Message.Message, e.Message.Language));
			}
		}
	}
}
