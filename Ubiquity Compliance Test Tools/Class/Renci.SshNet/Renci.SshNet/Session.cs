#define TRACE
using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using Renci.SshNet.Compression;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Authentication;
using Renci.SshNet.Messages.Connection;
using Renci.SshNet.Messages.Transport;
using Renci.SshNet.Security;
using Renci.SshNet.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Renci.SshNet
{
	public class Session : IDisposable
	{
		private class MessageMetadata
		{
			public string Name
			{
				get;
				set;
			}

			public byte Number
			{
				get;
				set;
			}

			public bool Enabled
			{
				get;
				set;
			}

			public bool Activated
			{
				get;
				set;
			}

			public Type Type
			{
				get;
				set;
			}
		}

		private const int MaximumSshPacketSize = 68536;

		private const int InitialLocalWindowSize = 2097152;

		private const int LocalChannelDataPacketSize = 65536;

		private static readonly RNGCryptoServiceProvider Randomizer;

		private static readonly Regex ServerVersionRe;

		private static readonly SemaphoreLight AuthenticationConnection;

		private IEnumerable<MessageMetadata> _messagesMetadata;

		private Socket _socket;

		private readonly object _socketLock = new object();

		private EventWaitHandle _messageListenerCompleted;

		private volatile uint _outboundPacketSequence;

		private uint _inboundPacketSequence;

		private EventWaitHandle _serviceAccepted = new AutoResetEvent(initialState: false);

		private EventWaitHandle _exceptionWaitHandle = new ManualResetEvent(initialState: false);

		private EventWaitHandle _keyExchangeCompletedWaitHandle = new ManualResetEvent(initialState: false);

		private bool _keyExchangeInProgress;

		private Exception _exception;

		private bool _isAuthenticated;

		private bool _isDisconnecting;

		private KeyExchange _keyExchange;

		private HashAlgorithm _serverMac;

		private HashAlgorithm _clientMac;

		private Cipher _clientCipher;

		private Cipher _serverCipher;

		private Compressor _serverDecompression;

		private Compressor _clientCompression;

		private SemaphoreLight _sessionSemaphore;

		private bool _isDisconnectMessageSent;

		private uint _nextChannelNumber;

		private Message _clientInitMessage;

		private bool _disposed;

		private readonly TraceSource _log = new TraceSource("SshNet.Logging");

		private static readonly Dictionary<Type, MethodInfo> _handlers;

		public SemaphoreLight SessionSemaphore
		{
			get
			{
				if (_sessionSemaphore == null)
				{
					lock (this)
					{
						if (_sessionSemaphore == null)
						{
							_sessionSemaphore = new SemaphoreLight(ConnectionInfo.MaxSessions);
						}
					}
				}
				return _sessionSemaphore;
			}
		}

		internal uint NextChannelNumber
		{
			get
			{
				lock (this)
				{
					return _nextChannelNumber++;
				}
			}
		}

		public bool IsConnected
		{
			get
			{
				if (_isDisconnectMessageSent || !_isAuthenticated)
				{
					return false;
				}
				if (_messageListenerCompleted == null || _messageListenerCompleted.WaitOne(0))
				{
					return false;
				}
				bool isConnected = false;
				IsSocketConnected(ref isConnected);
				return isConnected;
			}
		}

		public byte[] SessionId
		{
			get;
			private set;
		}

		public Message ClientInitMessage
		{
			get
			{
				if (_clientInitMessage == null)
				{
					_clientInitMessage = new KeyExchangeInitMessage
					{
						KeyExchangeAlgorithms = ConnectionInfo.KeyExchangeAlgorithms.Keys.ToArray(),
						ServerHostKeyAlgorithms = ConnectionInfo.HostKeyAlgorithms.Keys.ToArray(),
						EncryptionAlgorithmsClientToServer = ConnectionInfo.Encryptions.Keys.ToArray(),
						EncryptionAlgorithmsServerToClient = ConnectionInfo.Encryptions.Keys.ToArray(),
						MacAlgorithmsClientToServer = ConnectionInfo.HmacAlgorithms.Keys.ToArray(),
						MacAlgorithmsServerToClient = ConnectionInfo.HmacAlgorithms.Keys.ToArray(),
						CompressionAlgorithmsClientToServer = ConnectionInfo.CompressionAlgorithms.Keys.ToArray(),
						CompressionAlgorithmsServerToClient = ConnectionInfo.CompressionAlgorithms.Keys.ToArray(),
						LanguagesClientToServer = new string[1]
						{
							string.Empty
						},
						LanguagesServerToClient = new string[1]
						{
							string.Empty
						},
						FirstKexPacketFollows = false,
						Reserved = 0
					};
				}
				return _clientInitMessage;
			}
		}

		public string ServerVersion
		{
			get;
			private set;
		}

		public string ClientVersion
		{
			get;
			private set;
		}

		public ConnectionInfo ConnectionInfo
		{
			get;
			private set;
		}

		public event EventHandler<ExceptionEventArgs> ErrorOccured;

		public event EventHandler<EventArgs> Disconnected;

		public event EventHandler<HostKeyEventArgs> HostKeyReceived;

		internal event EventHandler<MessageEventArgs<DisconnectMessage>> DisconnectReceived;

		internal event EventHandler<MessageEventArgs<IgnoreMessage>> IgnoreReceived;

		internal event EventHandler<MessageEventArgs<UnimplementedMessage>> UnimplementedReceived;

		internal event EventHandler<MessageEventArgs<DebugMessage>> DebugReceived;

		internal event EventHandler<MessageEventArgs<ServiceRequestMessage>> ServiceRequestReceived;

		internal event EventHandler<MessageEventArgs<ServiceAcceptMessage>> ServiceAcceptReceived;

		internal event EventHandler<MessageEventArgs<KeyExchangeInitMessage>> KeyExchangeInitReceived;

		internal event EventHandler<MessageEventArgs<NewKeysMessage>> NewKeysReceived;

		internal event EventHandler<MessageEventArgs<RequestMessage>> UserAuthenticationRequestReceived;

		internal event EventHandler<MessageEventArgs<FailureMessage>> UserAuthenticationFailureReceived;

		internal event EventHandler<MessageEventArgs<SuccessMessage>> UserAuthenticationSuccessReceived;

		internal event EventHandler<MessageEventArgs<BannerMessage>> UserAuthenticationBannerReceived;

		internal event EventHandler<MessageEventArgs<GlobalRequestMessage>> GlobalRequestReceived;

		internal event EventHandler<MessageEventArgs<RequestSuccessMessage>> RequestSuccessReceived;

		internal event EventHandler<MessageEventArgs<RequestFailureMessage>> RequestFailureReceived;

		internal event EventHandler<MessageEventArgs<ChannelOpenMessage>> ChannelOpenReceived;

		internal event EventHandler<MessageEventArgs<ChannelOpenConfirmationMessage>> ChannelOpenConfirmationReceived;

		internal event EventHandler<MessageEventArgs<ChannelOpenFailureMessage>> ChannelOpenFailureReceived;

		internal event EventHandler<MessageEventArgs<ChannelWindowAdjustMessage>> ChannelWindowAdjustReceived;

		internal event EventHandler<MessageEventArgs<ChannelDataMessage>> ChannelDataReceived;

		internal event EventHandler<MessageEventArgs<ChannelExtendedDataMessage>> ChannelExtendedDataReceived;

		internal event EventHandler<MessageEventArgs<ChannelEofMessage>> ChannelEofReceived;

		internal event EventHandler<MessageEventArgs<ChannelCloseMessage>> ChannelCloseReceived;

		internal event EventHandler<MessageEventArgs<ChannelRequestMessage>> ChannelRequestReceived;

		internal event EventHandler<MessageEventArgs<ChannelSuccessMessage>> ChannelSuccessReceived;

		internal event EventHandler<MessageEventArgs<ChannelFailureMessage>> ChannelFailureReceived;

		internal event EventHandler<MessageEventArgs<Message>> MessageReceived;

		internal Session(ConnectionInfo connectionInfo)
		{
			if (connectionInfo == null)
			{
				throw new ArgumentNullException("connectionInfo");
			}
			ConnectionInfo = connectionInfo;
			ClientVersion = string.Format(CultureInfo.CurrentCulture, "SSH-2.0-Renci.SshNet.SshClient.0.0.1");
		}

		public void Connect()
		{
			if (!IsConnected)
			{
				try
				{
					AuthenticationConnection.Wait();
					if (!IsConnected)
					{
						lock (this)
						{
							if (!IsConnected)
							{
								Reset();
								_messagesMetadata = GetMessagesMetadata();
								switch (ConnectionInfo.ProxyType)
								{
								case ProxyTypes.None:
									SocketConnect(ConnectionInfo.Host, ConnectionInfo.Port);
									break;
								case ProxyTypes.Socks4:
									SocketConnect(ConnectionInfo.ProxyHost, ConnectionInfo.ProxyPort);
									ConnectSocks4();
									break;
								case ProxyTypes.Socks5:
									SocketConnect(ConnectionInfo.ProxyHost, ConnectionInfo.ProxyPort);
									ConnectSocks5();
									break;
								case ProxyTypes.Http:
									SocketConnect(ConnectionInfo.ProxyHost, ConnectionInfo.ProxyPort);
									ConnectHttp();
									break;
								}
								Match match;
								do
								{
									string response = string.Empty;
									SocketReadLine(ref response);
									ServerVersion = response;
									if (string.IsNullOrEmpty(ServerVersion))
									{
										throw new InvalidOperationException("Server string is null or empty.");
									}
									match = ServerVersionRe.Match(ServerVersion);
								}
								while (!match.Success);
								ConnectionInfo.ServerVersion = ServerVersion;
								ConnectionInfo.ClientVersion = ClientVersion;
								string text = match.Result("${protoversion}");
								match.Result("${softwareversion}");
								if (!text.Equals("2.0") && !text.Equals("1.99"))
								{
									throw new SshConnectionException(string.Format(CultureInfo.CurrentCulture, "Server version '{0}' is not supported.", new object[1]
									{
										text
									}), DisconnectReason.ProtocolVersionNotSupported);
								}
								SocketWrite(Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}\r\n", new object[1]
								{
									ClientVersion
								})));
								RegisterMessage("SSH_MSG_DISCONNECT");
								RegisterMessage("SSH_MSG_IGNORE");
								RegisterMessage("SSH_MSG_UNIMPLEMENTED");
								RegisterMessage("SSH_MSG_DEBUG");
								RegisterMessage("SSH_MSG_SERVICE_ACCEPT");
								RegisterMessage("SSH_MSG_KEXINIT");
								RegisterMessage("SSH_MSG_NEWKEYS");
								RegisterMessage("SSH_MSG_USERAUTH_BANNER");
								_messageListenerCompleted = new ManualResetEvent(initialState: false);
								ExecuteThread(delegate
								{
									try
									{
										MessageListener();
									}
									finally
									{
										_messageListenerCompleted.Set();
									}
								});
								WaitOnHandle(_keyExchangeCompletedWaitHandle);
								if (SessionId == null)
								{
									Disconnect();
								}
								else
								{
									SendMessage(new ServiceRequestMessage(ServiceName.UserAuthentication));
									WaitOnHandle(_serviceAccepted);
									if (string.IsNullOrEmpty(ConnectionInfo.Username))
									{
										throw new SshException("Username is not specified.");
									}
									ConnectionInfo.Authenticate(this);
									_isAuthenticated = true;
									RegisterMessage("SSH_MSG_GLOBAL_REQUEST");
									RegisterMessage("SSH_MSG_REQUEST_SUCCESS");
									RegisterMessage("SSH_MSG_REQUEST_FAILURE");
									RegisterMessage("SSH_MSG_CHANNEL_OPEN_CONFIRMATION");
									RegisterMessage("SSH_MSG_CHANNEL_OPEN_FAILURE");
									RegisterMessage("SSH_MSG_CHANNEL_WINDOW_ADJUST");
									RegisterMessage("SSH_MSG_CHANNEL_EXTENDED_DATA");
									RegisterMessage("SSH_MSG_CHANNEL_REQUEST");
									RegisterMessage("SSH_MSG_CHANNEL_SUCCESS");
									RegisterMessage("SSH_MSG_CHANNEL_FAILURE");
									RegisterMessage("SSH_MSG_CHANNEL_DATA");
									RegisterMessage("SSH_MSG_CHANNEL_EOF");
									RegisterMessage("SSH_MSG_CHANNEL_CLOSE");
									Monitor.Pulse(this);
								}
							}
						}
					}
				}
				finally
				{
					AuthenticationConnection.Release();
				}
			}
		}

		public void Disconnect()
		{
			Disconnect(DisconnectReason.ByApplication, "Connection terminated by the client.");
		}

		private void Disconnect(DisconnectReason reason, string message)
		{
			_isDisconnecting = true;
			SendDisconnect(reason, message);
			SocketDisconnectAndDispose();
			if (_messageListenerCompleted != null)
			{
				_messageListenerCompleted.WaitOne();
				Extensions.Dispose(_messageListenerCompleted);
				_messageListenerCompleted = null;
			}
		}

		internal T CreateClientChannel<T>() where T : ClientChannel, new()
		{
			T result = new T();
			lock (this)
			{
				result.Initialize(this, 2097152u, 65536u);
				return result;
			}
		}

		internal T CreateServerChannel<T>(uint remoteChannelNumber, uint remoteWindowSize, uint remoteChannelDataPacketSize) where T : ServerChannel, new()
		{
			T result = new T();
			lock (this)
			{
				result.Initialize(this, 2097152u, 65536u, remoteChannelNumber, remoteWindowSize, remoteChannelDataPacketSize);
				return result;
			}
		}

		internal void SendKeepAlive()
		{
			SendMessage(new IgnoreMessage());
		}

		internal void WaitOnHandle(WaitHandle waitHandle)
		{
			WaitHandle[] waitHandles = new WaitHandle[3]
			{
				_exceptionWaitHandle,
				_messageListenerCompleted,
				waitHandle
			};
			switch (WaitHandle.WaitAny(waitHandles, ConnectionInfo.Timeout))
			{
			case 0:
				throw _exception;
			case 1:
				if (!_isDisconnecting)
				{
					throw new SshConnectionException("Client not connected.");
				}
				break;
			case 258:
				if (!_isDisconnecting)
				{
					Disconnect(DisconnectReason.ByApplication, "Operation timeout");
					throw new SshOperationTimeoutException("Session operation has timed out");
				}
				break;
			}
		}

		internal void SendMessage(Message message)
		{
			if (_socket == null || !_socket.CanWrite())
			{
				throw new SshConnectionException("Client not connected.");
			}
			if (_keyExchangeInProgress && !(message is IKeyExchangedAllowed))
			{
				WaitOnHandle(_keyExchangeCompletedWaitHandle);
			}
			byte b = (byte)((_clientCipher == null) ? 8 : Math.Max((byte)8, _serverCipher.MinimumSize));
			byte[] array = message.GetBytes();
			if (_clientCompression != null)
			{
				array = _clientCompression.Compress(array);
			}
			int num = array.Length + 4 + 1;
			byte b2 = (byte)(-num & (b - 1));
			if (b2 < b)
			{
				b2 = (byte)(b2 + b);
			}
			byte[] array2 = new byte[5 + array.Length + b2];
			((uint)(array2.Length - 4)).GetBytes().CopyTo(array2, 0);
			array2[4] = b2;
			array.CopyTo(array2, 5);
			byte[] array3 = new byte[b2];
			Randomizer.GetBytes(array3);
			array3.CopyTo(array2, 5 + array.Length);
			lock (_socketLock)
			{
				if (_socket == null || !_socket.Connected)
				{
					throw new SshConnectionException("Client not connected.");
				}
				byte[] array4 = new byte[4 + array2.Length];
				_outboundPacketSequence.GetBytes().CopyTo(array4, 0);
				array2.CopyTo(array4, 4);
				if (_clientCipher != null)
				{
					array2 = _clientCipher.Encrypt(array2);
				}
				if (array2.Length > 68536)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Packet is too big. Maximum packet size is {0} bytes.", new object[1]
					{
						68536
					}));
				}
				if (_clientMac == null)
				{
					SocketWrite(array2);
				}
				else
				{
					byte[] array5 = _clientMac.ComputeHash(array4.ToArray());
					byte[] array6 = new byte[array2.Length + _clientMac.HashSize / 8];
					array2.CopyTo(array6, 0);
					array5.CopyTo(array6, array2.Length);
					SocketWrite(array6);
				}
				_outboundPacketSequence++;
				Monitor.Pulse(_socketLock);
			}
		}

		private static IEnumerable<MessageMetadata> GetMessagesMetadata()
		{
			return new MessageMetadata[36]
			{
				new MessageMetadata
				{
					Name = "SSH_MSG_NEWKEYS",
					Number = 21,
					Type = typeof(NewKeysMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_REQUEST_FAILURE",
					Number = 82,
					Type = typeof(RequestFailureMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_KEXINIT",
					Number = 20,
					Type = typeof(KeyExchangeInitMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_OPEN_FAILURE",
					Number = 92,
					Type = typeof(ChannelOpenFailureMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_FAILURE",
					Number = 100,
					Type = typeof(ChannelFailureMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_EXTENDED_DATA",
					Number = 95,
					Type = typeof(ChannelExtendedDataMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_DATA",
					Number = 94,
					Type = typeof(ChannelDataMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_USERAUTH_REQUEST",
					Number = 50,
					Type = typeof(RequestMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_REQUEST",
					Number = 98,
					Type = typeof(ChannelRequestMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_USERAUTH_BANNER",
					Number = 53,
					Type = typeof(BannerMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_USERAUTH_INFO_RESPONSE",
					Number = 61,
					Type = typeof(InformationResponseMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_USERAUTH_FAILURE",
					Number = 51,
					Type = typeof(FailureMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_DEBUG",
					Number = 4,
					Type = typeof(DebugMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_KEXDH_INIT",
					Number = 30,
					Type = typeof(KeyExchangeDhInitMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_GLOBAL_REQUEST",
					Number = 80,
					Type = typeof(GlobalRequestMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_OPEN",
					Number = 90,
					Type = typeof(ChannelOpenMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_OPEN_CONFIRMATION",
					Number = 91,
					Type = typeof(ChannelOpenConfirmationMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_USERAUTH_INFO_REQUEST",
					Number = 60,
					Type = typeof(InformationRequestMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_UNIMPLEMENTED",
					Number = 3,
					Type = typeof(UnimplementedMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_REQUEST_SUCCESS",
					Number = 81,
					Type = typeof(RequestSuccessMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_SUCCESS",
					Number = 99,
					Type = typeof(ChannelSuccessMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_USERAUTH_PASSWD_CHANGEREQ",
					Number = 60,
					Type = typeof(PasswordChangeRequiredMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_DISCONNECT",
					Number = 1,
					Type = typeof(DisconnectMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_SERVICE_REQUEST",
					Number = 5,
					Type = typeof(ServiceRequestMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_KEX_DH_GEX_REQUEST",
					Number = 34,
					Type = typeof(KeyExchangeDhGroupExchangeRequest)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_KEX_DH_GEX_GROUP",
					Number = 31,
					Type = typeof(KeyExchangeDhGroupExchangeGroup)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_USERAUTH_SUCCESS",
					Number = 52,
					Type = typeof(SuccessMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_USERAUTH_PK_OK",
					Number = 60,
					Type = typeof(PublicKeyMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_IGNORE",
					Number = 2,
					Type = typeof(IgnoreMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_WINDOW_ADJUST",
					Number = 93,
					Type = typeof(ChannelWindowAdjustMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_EOF",
					Number = 96,
					Type = typeof(ChannelEofMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_CHANNEL_CLOSE",
					Number = 97,
					Type = typeof(ChannelCloseMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_SERVICE_ACCEPT",
					Number = 6,
					Type = typeof(ServiceAcceptMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_KEXDH_REPLY",
					Number = 31,
					Type = typeof(KeyExchangeDhReplyMessage)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_KEX_DH_GEX_INIT",
					Number = 32,
					Type = typeof(KeyExchangeDhGroupExchangeInit)
				},
				new MessageMetadata
				{
					Name = "SSH_MSG_KEX_DH_GEX_REPLY",
					Number = 33,
					Type = typeof(KeyExchangeDhGroupExchangeReply)
				}
			};
		}

		private Message ReceiveMessage()
		{
			byte b = (byte)((_serverCipher == null) ? 8 : Math.Max((byte)8, _serverCipher.MinimumSize));
			byte[] array = Read(b);
			if (_serverCipher != null)
			{
				array = _serverCipher.Decrypt(array);
			}
			uint num = (uint)((array[0] << 24) | (array[1] << 16) | (array[2] << 8) | array[3]);
			if (num < Math.Max((byte)16, b) - 4 || num > 68532)
			{
				throw new SshConnectionException(string.Format(CultureInfo.CurrentCulture, "Bad packet length {0}", new object[1]
				{
					num
				}), DisconnectReason.ProtocolError);
			}
			int num2 = (int)(num - (b - 4));
			byte[] array2 = new byte[num2 + b];
			array.CopyTo(array2, 0);
			byte[] array3 = null;
			if (_serverMac != null)
			{
				array3 = new byte[_serverMac.HashSize / 8];
				num2 += array3.Length;
			}
			if (num2 > 0)
			{
				byte[] array4 = Read(num2);
				if (array3 != null)
				{
					Buffer.BlockCopy(array4, array4.Length - array3.Length, array3, 0, array3.Length);
					array4 = array4.Take(array4.Length - array3.Length).ToArray();
				}
				if (array4.Length > 0)
				{
					if (_serverCipher != null)
					{
						array4 = _serverCipher.Decrypt(array4);
					}
					array4.CopyTo(array2, b);
				}
			}
			byte b2 = array2[4];
			byte[] array5 = new byte[num - b2 - 1];
			Buffer.BlockCopy(array2, 5, array5, 0, array5.Length);
			if (_serverDecompression != null)
			{
				array5 = _serverDecompression.Decompress(array5);
			}
			if (_serverMac != null)
			{
				byte[] array6 = new byte[4 + array2.Length];
				byte[] bytes = _inboundPacketSequence.GetBytes();
				bytes.CopyTo(array6, 0);
				array2.CopyTo(array6, 4);
				byte[] second = _serverMac.ComputeHash(array6);
				if (!array3.SequenceEqual(second))
				{
					throw new SshConnectionException("MAC error", DisconnectReason.MacError);
				}
			}
			_inboundPacketSequence++;
			return LoadMessage(array5);
		}

		private void SendDisconnect(DisconnectReason reasonCode, string message)
		{
			if (!_isDisconnectMessageSent && IsConnected)
			{
				DisconnectMessage message2 = new DisconnectMessage(reasonCode, message);
				SendMessage(message2);
				_isDisconnectMessageSent = true;
			}
		}

		private void HandleMessage<T>(T message) where T : Message
		{
			OnMessageReceived(message);
		}

		private void HandleMessage(DisconnectMessage message)
		{
			OnDisconnectReceived(message);
			SocketDisconnectAndDispose();
		}

		private void HandleMessage(IgnoreMessage message)
		{
			OnIgnoreReceived(message);
		}

		private void HandleMessage(UnimplementedMessage message)
		{
			OnUnimplementedReceived(message);
		}

		private void HandleMessage(DebugMessage message)
		{
			OnDebugReceived(message);
		}

		private void HandleMessage(ServiceRequestMessage message)
		{
			OnServiceRequestReceived(message);
		}

		private void HandleMessage(ServiceAcceptMessage message)
		{
			OnServiceAcceptReceived(message);
			_serviceAccepted.Set();
		}

		private void HandleMessage(KeyExchangeInitMessage message)
		{
			OnKeyExchangeInitReceived(message);
		}

		private void HandleMessage(NewKeysMessage message)
		{
			OnNewKeysReceived(message);
		}

		private void HandleMessage(RequestMessage message)
		{
			OnUserAuthenticationRequestReceived(message);
		}

		private void HandleMessage(FailureMessage message)
		{
			OnUserAuthenticationFailureReceived(message);
		}

		private void HandleMessage(SuccessMessage message)
		{
			OnUserAuthenticationSuccessReceived(message);
		}

		private void HandleMessage(BannerMessage message)
		{
			OnUserAuthenticationBannerReceived(message);
		}

		private void HandleMessage(GlobalRequestMessage message)
		{
			OnGlobalRequestReceived(message);
		}

		private void HandleMessage(RequestSuccessMessage message)
		{
			OnRequestSuccessReceived(message);
		}

		private void HandleMessage(RequestFailureMessage message)
		{
			OnRequestFailureReceived(message);
		}

		private void HandleMessage(ChannelOpenMessage message)
		{
			OnChannelOpenReceived(message);
		}

		private void HandleMessage(ChannelOpenConfirmationMessage message)
		{
			OnChannelOpenConfirmationReceived(message);
		}

		private void HandleMessage(ChannelOpenFailureMessage message)
		{
			OnChannelOpenFailureReceived(message);
		}

		private void HandleMessage(ChannelWindowAdjustMessage message)
		{
			OnChannelWindowAdjustReceived(message);
		}

		private void HandleMessage(ChannelDataMessage message)
		{
			OnChannelDataReceived(message);
		}

		private void HandleMessage(ChannelExtendedDataMessage message)
		{
			OnChannelExtendedDataReceived(message);
		}

		private void HandleMessage(ChannelEofMessage message)
		{
			OnChannelEofReceived(message);
		}

		private void HandleMessage(ChannelCloseMessage message)
		{
			OnChannelCloseReceived(message);
		}

		private void HandleMessage(ChannelRequestMessage message)
		{
			OnChannelRequestReceived(message);
		}

		private void HandleMessage(ChannelSuccessMessage message)
		{
			OnChannelSuccessReceived(message);
		}

		private void HandleMessage(ChannelFailureMessage message)
		{
			OnChannelFailureReceived(message);
		}

		protected virtual void OnDisconnectReceived(DisconnectMessage message)
		{
			this.DisconnectReceived?.Invoke(this, new MessageEventArgs<DisconnectMessage>(message));
			this.Disconnected?.Invoke(this, new EventArgs());
		}

		protected virtual void OnIgnoreReceived(IgnoreMessage message)
		{
			this.IgnoreReceived?.Invoke(this, new MessageEventArgs<IgnoreMessage>(message));
		}

		protected virtual void OnUnimplementedReceived(UnimplementedMessage message)
		{
			this.UnimplementedReceived?.Invoke(this, new MessageEventArgs<UnimplementedMessage>(message));
		}

		protected virtual void OnDebugReceived(DebugMessage message)
		{
			this.DebugReceived?.Invoke(this, new MessageEventArgs<DebugMessage>(message));
		}

		protected virtual void OnServiceRequestReceived(ServiceRequestMessage message)
		{
			this.ServiceRequestReceived?.Invoke(this, new MessageEventArgs<ServiceRequestMessage>(message));
		}

		protected virtual void OnServiceAcceptReceived(ServiceAcceptMessage message)
		{
			this.ServiceAcceptReceived?.Invoke(this, new MessageEventArgs<ServiceAcceptMessage>(message));
		}

		protected virtual void OnKeyExchangeInitReceived(KeyExchangeInitMessage message)
		{
			_keyExchangeInProgress = true;
			_keyExchangeCompletedWaitHandle.Reset();
			foreach (MessageMetadata messagesMetadatum in _messagesMetadata)
			{
				if (messagesMetadatum.Activated && messagesMetadatum.Number > 2 && (messagesMetadatum.Number < 20 || messagesMetadatum.Number > 30))
				{
					messagesMetadatum.Enabled = false;
				}
			}
			string text = (from c in ConnectionInfo.KeyExchangeAlgorithms.Keys
			from s in message.KeyExchangeAlgorithms
			where s == c
			select c).FirstOrDefault();
			if (text == null)
			{
				throw new SshConnectionException("Failed to negotiate key exchange algorithm.", DisconnectReason.KeyExchangeFailed);
			}
			_keyExchange = ConnectionInfo.KeyExchangeAlgorithms[text].CreateInstance<KeyExchange>();
			ConnectionInfo.CurrentKeyExchangeAlgorithm = text;
			_keyExchange.HostKeyReceived += KeyExchange_HostKeyReceived;
			_keyExchange.Start(this, message);
			this.KeyExchangeInitReceived?.Invoke(this, new MessageEventArgs<KeyExchangeInitMessage>(message));
		}

		protected virtual void OnNewKeysReceived(NewKeysMessage message)
		{
			if (SessionId == null)
			{
				SessionId = _keyExchange.ExchangeHash;
			}
			if (_serverMac != null)
			{
				_serverMac.Clear();
				_serverMac = null;
			}
			if (_clientMac != null)
			{
				_clientMac.Clear();
				_clientMac = null;
			}
			_serverCipher = _keyExchange.CreateServerCipher();
			_clientCipher = _keyExchange.CreateClientCipher();
			_serverMac = _keyExchange.CreateServerHash();
			_clientMac = _keyExchange.CreateClientHash();
			_clientCompression = _keyExchange.CreateCompressor();
			_serverDecompression = _keyExchange.CreateDecompressor();
			if (_keyExchange != null)
			{
				_keyExchange.HostKeyReceived -= KeyExchange_HostKeyReceived;
				_keyExchange.Dispose();
				_keyExchange = null;
			}
			foreach (MessageMetadata messagesMetadatum in _messagesMetadata)
			{
				if (messagesMetadatum.Activated)
				{
					messagesMetadatum.Enabled = true;
				}
			}
			this.NewKeysReceived?.Invoke(this, new MessageEventArgs<NewKeysMessage>(message));
			_keyExchangeCompletedWaitHandle.Set();
			_keyExchangeInProgress = false;
		}

		internal void OnDisconnecting()
		{
			_isDisconnecting = true;
		}

		protected virtual void OnUserAuthenticationRequestReceived(RequestMessage message)
		{
			this.UserAuthenticationRequestReceived?.Invoke(this, new MessageEventArgs<RequestMessage>(message));
		}

		protected virtual void OnUserAuthenticationFailureReceived(FailureMessage message)
		{
			this.UserAuthenticationFailureReceived?.Invoke(this, new MessageEventArgs<FailureMessage>(message));
		}

		protected virtual void OnUserAuthenticationSuccessReceived(SuccessMessage message)
		{
			this.UserAuthenticationSuccessReceived?.Invoke(this, new MessageEventArgs<SuccessMessage>(message));
		}

		protected virtual void OnUserAuthenticationBannerReceived(BannerMessage message)
		{
			this.UserAuthenticationBannerReceived?.Invoke(this, new MessageEventArgs<BannerMessage>(message));
		}

		protected virtual void OnGlobalRequestReceived(GlobalRequestMessage message)
		{
			this.GlobalRequestReceived?.Invoke(this, new MessageEventArgs<GlobalRequestMessage>(message));
		}

		protected virtual void OnRequestSuccessReceived(RequestSuccessMessage message)
		{
			this.RequestSuccessReceived?.Invoke(this, new MessageEventArgs<RequestSuccessMessage>(message));
		}

		protected virtual void OnRequestFailureReceived(RequestFailureMessage message)
		{
			this.RequestFailureReceived?.Invoke(this, new MessageEventArgs<RequestFailureMessage>(message));
		}

		protected virtual void OnChannelOpenReceived(ChannelOpenMessage message)
		{
			this.ChannelOpenReceived?.Invoke(this, new MessageEventArgs<ChannelOpenMessage>(message));
		}

		protected virtual void OnChannelOpenConfirmationReceived(ChannelOpenConfirmationMessage message)
		{
			this.ChannelOpenConfirmationReceived?.Invoke(this, new MessageEventArgs<ChannelOpenConfirmationMessage>(message));
		}

		protected virtual void OnChannelOpenFailureReceived(ChannelOpenFailureMessage message)
		{
			this.ChannelOpenFailureReceived?.Invoke(this, new MessageEventArgs<ChannelOpenFailureMessage>(message));
		}

		protected virtual void OnChannelWindowAdjustReceived(ChannelWindowAdjustMessage message)
		{
			this.ChannelWindowAdjustReceived?.Invoke(this, new MessageEventArgs<ChannelWindowAdjustMessage>(message));
		}

		protected virtual void OnChannelDataReceived(ChannelDataMessage message)
		{
			this.ChannelDataReceived?.Invoke(this, new MessageEventArgs<ChannelDataMessage>(message));
		}

		protected virtual void OnChannelExtendedDataReceived(ChannelExtendedDataMessage message)
		{
			this.ChannelExtendedDataReceived?.Invoke(this, new MessageEventArgs<ChannelExtendedDataMessage>(message));
		}

		protected virtual void OnChannelEofReceived(ChannelEofMessage message)
		{
			this.ChannelEofReceived?.Invoke(this, new MessageEventArgs<ChannelEofMessage>(message));
		}

		protected virtual void OnChannelCloseReceived(ChannelCloseMessage message)
		{
			this.ChannelCloseReceived?.Invoke(this, new MessageEventArgs<ChannelCloseMessage>(message));
		}

		protected virtual void OnChannelRequestReceived(ChannelRequestMessage message)
		{
			this.ChannelRequestReceived?.Invoke(this, new MessageEventArgs<ChannelRequestMessage>(message));
		}

		protected virtual void OnChannelSuccessReceived(ChannelSuccessMessage message)
		{
			this.ChannelSuccessReceived?.Invoke(this, new MessageEventArgs<ChannelSuccessMessage>(message));
		}

		protected virtual void OnChannelFailureReceived(ChannelFailureMessage message)
		{
			this.ChannelFailureReceived?.Invoke(this, new MessageEventArgs<ChannelFailureMessage>(message));
		}

		protected virtual void OnMessageReceived(Message message)
		{
			this.MessageReceived?.Invoke(this, new MessageEventArgs<Message>(message));
		}

		private void KeyExchange_HostKeyReceived(object sender, HostKeyEventArgs e)
		{
			this.HostKeyReceived?.Invoke(this, e);
		}

		private byte[] Read(int length)
		{
			byte[] buffer = new byte[length];
			SocketRead(length, ref buffer);
			return buffer;
		}

		public void RegisterMessage(string messageName)
		{
			InternalRegisterMessage(messageName);
		}

		public void UnRegisterMessage(string messageName)
		{
			InternalUnRegisterMessage(messageName);
		}

		private Message LoadMessage(byte[] data)
		{
			byte messageType = data[0];
			MessageMetadata messageMetadata = _messagesMetadata.Where(delegate(MessageMetadata m)
			{
				if (m.Number == messageType && m.Enabled)
				{
					return m.Activated;
				}
				return false;
			}).SingleOrDefault();
			if (messageMetadata == null)
			{
				throw new SshException(string.Format(CultureInfo.CurrentCulture, "Message type {0} is not valid.", new object[1]
				{
					messageType
				}));
			}
			Message message = messageMetadata.Type.CreateInstance<Message>();
			message.Load(data);
			return message;
		}

		private void SocketDisconnectAndDispose()
		{
			if (_socket != null)
			{
				lock (_socketLock)
				{
					if (_socket != null)
					{
						SocketDisconnect();
						Extensions.Dispose(_socket);
						_socket = null;
					}
				}
			}
		}

		private void MessageListener()
		{
			try
			{
				while (_socket != null && _socket.Connected)
				{
					Message message = ReceiveMessage();
					HandleMessageCore(message);
				}
			}
			catch (Exception exp)
			{
				RaiseError(exp);
			}
		}

		private byte SocketReadByte()
		{
			byte[] buffer = new byte[1];
			SocketRead(1, ref buffer);
			return buffer[0];
		}

		private void SocketWriteByte(byte data)
		{
			SocketWrite(new byte[1]
			{
				data
			});
		}

		private void ConnectSocks4()
		{
			SocketWriteByte(4);
			SocketWriteByte(1);
			SocketWriteByte((byte)(ConnectionInfo.Port / 255));
			SocketWriteByte((byte)(ConnectionInfo.Port % 255));
			IPAddress iPAddress = ConnectionInfo.Host.GetIPAddress();
			SocketWrite(iPAddress.GetAddressBytes());
			byte[] bytes = new Renci.SshNet.Common.ASCIIEncoding().GetBytes(ConnectionInfo.ProxyUsername);
			SocketWrite(bytes);
			SocketWriteByte(0);
			if (SocketReadByte() != 0)
			{
				throw new ProxyException("SOCKS4: Null is expected.");
			}
			switch (SocketReadByte())
			{
			case 91:
				throw new ProxyException("SOCKS4: Connection rejected.");
			case 92:
				throw new ProxyException("SOCKS4: Client is not running identd or not reachable from the server.");
			case 93:
				throw new ProxyException("SOCKS4: Client's identd could not confirm the user ID string in the request.");
			default:
				throw new ProxyException("SOCKS4: Not valid response.");
			case 90:
			{
				byte[] buffer = new byte[4];
				SocketRead(2, ref buffer);
				SocketRead(4, ref buffer);
				break;
			}
			}
		}

		private void ConnectSocks5()
		{
			SocketWriteByte(5);
			SocketWriteByte(2);
			SocketWriteByte(0);
			SocketWriteByte(2);
			byte b = SocketReadByte();
			if (b != 5)
			{
				throw new ProxyException($"SOCKS Version '{b}' is not supported.");
			}
			switch (SocketReadByte())
			{
			case 2:
			{
				SocketWriteByte(1);
				Renci.SshNet.Common.ASCIIEncoding aSCIIEncoding = new Renci.SshNet.Common.ASCIIEncoding();
				byte[] bytes = aSCIIEncoding.GetBytes(ConnectionInfo.ProxyUsername);
				if (bytes.Length > 255)
				{
					throw new ProxyException("Proxy username is too long.");
				}
				SocketWriteByte((byte)bytes.Length);
				SocketWrite(bytes);
				byte[] bytes2 = aSCIIEncoding.GetBytes(ConnectionInfo.ProxyPassword);
				if (bytes2.Length > 255)
				{
					throw new ProxyException("Proxy password is too long.");
				}
				SocketWriteByte((byte)bytes2.Length);
				SocketWrite(bytes2);
				byte b2 = SocketReadByte();
				if (b2 != 1)
				{
					throw new ProxyException("SOCKS5: Server authentication version is not valid.");
				}
				if (SocketReadByte() != 0)
				{
					throw new ProxyException("SOCKS5: Username/Password authentication failed.");
				}
				break;
			}
			case byte.MaxValue:
				throw new ProxyException("SOCKS5: No acceptable authentication methods were offered.");
			}
			SocketWriteByte(5);
			SocketWriteByte(1);
			SocketWriteByte(0);
			IPAddress iPAddress = ConnectionInfo.Host.GetIPAddress();
			if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
			{
				SocketWriteByte(1);
				byte[] addressBytes = iPAddress.GetAddressBytes();
				SocketWrite(addressBytes);
			}
			else
			{
				if (iPAddress.AddressFamily != AddressFamily.InterNetworkV6)
				{
					throw new ProxyException($"SOCKS5: IP address '{iPAddress}' is not supported.");
				}
				SocketWriteByte(4);
				byte[] addressBytes2 = iPAddress.GetAddressBytes();
				SocketWrite(addressBytes2);
			}
			SocketWriteByte((byte)(ConnectionInfo.Port / 255));
			SocketWriteByte((byte)(ConnectionInfo.Port % 255));
			if (SocketReadByte() != 5)
			{
				throw new ProxyException("SOCKS5: Version 5 is expected.");
			}
			switch (SocketReadByte())
			{
			case 1:
				throw new ProxyException("SOCKS5: General failure.");
			case 2:
				throw new ProxyException("SOCKS5: Connection not allowed by ruleset.");
			case 3:
				throw new ProxyException("SOCKS5: Network unreachable.");
			case 4:
				throw new ProxyException("SOCKS5: Host unreachable.");
			case 5:
				throw new ProxyException("SOCKS5: Connection refused by destination host.");
			case 6:
				throw new ProxyException("SOCKS5: TTL expired.");
			case 7:
				throw new ProxyException("SOCKS5: Command not supported or protocol error.");
			case 8:
				throw new ProxyException("SOCKS5: Address type not supported.");
			default:
				throw new ProxyException("SOCKS4: Not valid response.");
			case 0:
			{
				if (SocketReadByte() != 0)
				{
					throw new ProxyException("SOCKS5: 0 byte is expected.");
				}
				byte b3 = SocketReadByte();
				byte[] buffer = new byte[16];
				switch (b3)
				{
				case 1:
					SocketRead(4, ref buffer);
					break;
				case 4:
					SocketRead(16, ref buffer);
					break;
				default:
					throw new ProxyException($"Address type '{b3}' is not supported.");
				}
				byte[] buffer2 = new byte[2];
				SocketRead(2, ref buffer2);
				break;
			}
			}
		}

		private void ConnectHttp()
		{
			Regex regex = new Regex("HTTP/(?<version>\\d[.]\\d) (?<statusCode>\\d{3}) (?<reasonPhrase>.+)$");
			Regex regex2 = new Regex("(?<fieldName>[^\\[\\]()<>@,;:\\\"/?={} \\t]+):(?<fieldValue>.+)?");
			Renci.SshNet.Common.ASCIIEncoding aSCIIEncoding = new Renci.SshNet.Common.ASCIIEncoding();
			SocketWrite(aSCIIEncoding.GetBytes($"CONNECT {ConnectionInfo.Host}:{ConnectionInfo.Port} HTTP/1.0\r\n"));
			if (!string.IsNullOrEmpty(ConnectionInfo.ProxyUsername))
			{
				string s = $"Proxy-Authorization: Basic {Convert.ToBase64String(aSCIIEncoding.GetBytes($"{ConnectionInfo.ProxyUsername}:{ConnectionInfo.ProxyPassword}"))}\r\n";
				SocketWrite(aSCIIEncoding.GetBytes(s));
			}
			SocketWrite(aSCIIEncoding.GetBytes("\r\n"));
			HttpStatusCode? httpStatusCode = null;
			string response = string.Empty;
			int num = 0;
			while (true)
			{
				SocketReadLine(ref response);
				if (!httpStatusCode.HasValue)
				{
					Match match = regex.Match(response);
					if (match.Success)
					{
						string text = match.Result("${statusCode}");
						httpStatusCode = (HttpStatusCode)int.Parse(text);
						if (httpStatusCode != HttpStatusCode.OK)
						{
							string arg = match.Result("${reasonPhrase}");
							throw new ProxyException($"HTTP: Status code {text}, \"{arg}\"");
						}
						continue;
					}
				}
				Match match2 = regex2.Match(response);
				if (match2.Success)
				{
					string text2 = match2.Result("${fieldName}");
					if (text2.Equals("Content-Length", StringComparison.InvariantCultureIgnoreCase))
					{
						num = int.Parse(match2.Result("${fieldValue}"));
					}
				}
				else if (response.Length == 0)
				{
					break;
				}
			}
			if (num > 0)
			{
				byte[] buffer = new byte[num];
				SocketRead(num, ref buffer);
			}
		}

		private void RaiseError(Exception exp)
		{
			SshConnectionException ex = exp as SshConnectionException;
			if (_isDisconnecting)
			{
				if (ex != null)
				{
					return;
				}
				SocketException ex2 = exp as SocketException;
				if (ex2 != null && ex2.SocketErrorCode == SocketError.TimedOut)
				{
					return;
				}
			}
			_exception = exp;
			_exceptionWaitHandle.Set();
			this.ErrorOccured?.Invoke(this, new ExceptionEventArgs(exp));
			if (ex != null && ex.DisconnectReason != DisconnectReason.ConnectionLost)
			{
				Disconnect(ex.DisconnectReason, exp.ToString());
			}
		}

		private void Reset()
		{
			if (_exceptionWaitHandle != null)
			{
				_exceptionWaitHandle.Reset();
			}
			if (_keyExchangeCompletedWaitHandle != null)
			{
				_keyExchangeCompletedWaitHandle.Reset();
			}
			if (_messageListenerCompleted != null)
			{
				_messageListenerCompleted.Reset();
			}
			SessionId = null;
			_isDisconnectMessageSent = false;
			_isDisconnecting = false;
			_isAuthenticated = false;
			_exception = null;
			_keyExchangeInProgress = false;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Disconnect();
					if (_serviceAccepted != null)
					{
						Extensions.Dispose(_serviceAccepted);
						_serviceAccepted = null;
					}
					if (_exceptionWaitHandle != null)
					{
						Extensions.Dispose(_exceptionWaitHandle);
						_exceptionWaitHandle = null;
					}
					if (_keyExchangeCompletedWaitHandle != null)
					{
						Extensions.Dispose(_keyExchangeCompletedWaitHandle);
						_keyExchangeCompletedWaitHandle = null;
					}
					if (_serverMac != null)
					{
						_serverMac.Clear();
						_serverMac = null;
					}
					if (_clientMac != null)
					{
						_clientMac.Clear();
						_clientMac = null;
					}
					if (_keyExchange != null)
					{
						_keyExchange.HostKeyReceived -= KeyExchange_HostKeyReceived;
						_keyExchange.Dispose();
						_keyExchange = null;
					}
				}
				_disposed = true;
			}
		}

		~Session()
		{
			Dispose(disposing: false);
		}

		private void IsSocketConnected(ref bool isConnected)
		{
			isConnected = (_socket != null && _socket.Connected);
			if (isConnected)
			{
				isConnected = (!_socket.Poll(1000, SelectMode.SelectRead) || _socket.Available != 0);
			}
		}

		private void SocketConnect(string host, int port)
		{
            try
            {
                IPAddress iPAddress = host.GetIPAddress();
                IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
                _socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, optionValue: true);
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 137072);
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 137072);
                IAsyncResult asyncResult = _socket.BeginConnect(iPEndPoint, null, null);
                if (!asyncResult.AsyncWaitHandle.WaitOne(ConnectionInfo.Timeout, exitContext: false))
                {
                    throw new SshOperationTimeoutException("Connection Could Not Be Established");
                }
                _socket.EndConnect(asyncResult);
            }
            catch (Exception)
            {

            }
		}

		private void SocketDisconnect()
		{
			_socket.Disconnect(reuseSocket: true);
		}

		private void SocketReadLine(ref string response)
		{
			Renci.SshNet.Common.ASCIIEncoding aSCIIEncoding = new Renci.SshNet.Common.ASCIIEncoding();
			List<byte> list = new List<byte>();
			byte[] array = new byte[1];
			do
			{
                if (_socket.Connected)
                {
                    IAsyncResult asyncResult = _socket.BeginReceive(array, 0, array.Length, SocketFlags.None, null, null);
                    if (!asyncResult.AsyncWaitHandle.WaitOne(ConnectionInfo.Timeout))
                    {
                        throw new SshOperationTimeoutException("Socket read operation has timed out");
                    }
                    if (_socket.EndReceive(asyncResult) == 0)
                    {
                        break;
                    }
                    list.Add(array[0]);
                }
			}
			while (list.Count <= 0 || (list[list.Count - 1] != 10 && list[list.Count - 1] != 0));
			if (list.Count > 0 && list[list.Count - 1] == 0)
			{
				response = string.Empty;
			}
			else if (list.Count > 1 && list[list.Count - 2] == 13)
			{
				response = aSCIIEncoding.GetString(list.Take(list.Count - 2).ToArray());
			}
			else
			{
				response = aSCIIEncoding.GetString(list.Take(list.Count - 1).ToArray());
			}
		}

		private void SocketRead(int length, ref byte[] buffer)
		{
			int num = 0;
			do
			{
				try
				{
					int num2 = _socket.Receive(buffer, num, length - num, SocketFlags.None);
					if (num2 <= 0)
					{
						if (_isDisconnecting)
						{
							throw new SshConnectionException("An established connection was aborted by the software in your host machine.", DisconnectReason.ConnectionLost);
						}
						throw new SshConnectionException("An established connection was aborted by the server.", DisconnectReason.ConnectionLost);
					}
					num += num2;
				}
				catch (SocketException ex)
				{
					if (ex.SocketErrorCode == SocketError.ConnectionAborted)
					{
						buffer = new byte[length];
						Disconnect();
						return;
					}
					if (ex.SocketErrorCode != SocketError.WouldBlock && ex.SocketErrorCode != SocketError.IOPending && ex.SocketErrorCode != SocketError.NoBufferSpaceAvailable)
					{
						throw;
					}
					Thread.Sleep(30);
				}
			}
			while (num < length);
		}

		private void SocketWrite(byte[] data)
		{
			int num = 0;
			int num2 = data.Length;
			do
			{
				try
				{
					num += _socket.Send(data, num, num2 - num, SocketFlags.None);
				}
				catch (SocketException ex)
				{
					if (ex.SocketErrorCode != SocketError.WouldBlock && ex.SocketErrorCode != SocketError.IOPending && ex.SocketErrorCode != SocketError.NoBufferSpaceAvailable)
					{
						throw;
					}
					Thread.Sleep(30);
				}
			}
			while (num < num2);
		}

		[Conditional("DEBUG")]
		private void Log(string text)
		{
			_log.TraceEvent(TraceEventType.Verbose, 1, text);
		}

		static Session()
		{
			Randomizer = new RNGCryptoServiceProvider();
			ServerVersionRe = new Regex("^SSH-(?<protoversion>[^-]+)-(?<softwareversion>.+)( SP.+)?$", RegexOptions.Compiled);
			AuthenticationConnection = new SemaphoreLight(3);
			_handlers = new Dictionary<Type, MethodInfo>();
			foreach (MethodInfo item in from x in typeof(Session).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
			where x.Name == "HandleMessage"
			select x)
			{
				if (!item.IsGenericMethod)
				{
					ParameterInfo[] parameters = item.GetParameters();
					if (parameters.Length == 1)
					{
						Type parameterType = parameters[0].ParameterType;
						if (parameterType.IsSubclassOf(typeof(Message)))
						{
							_handlers.Add(parameterType, item);
						}
					}
				}
			}
		}

		private void HandleMessageCore(Message message)
		{
			if (_handlers.TryGetValue(message.GetType(), out MethodInfo value))
			{
				try
				{
					value.Invoke(this, new object[1]
					{
						message
					});
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException ?? ex;
				}
			}
			else
			{
				HandleMessage(message);
			}
		}

		private void ExecuteThread(Action action)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				action();
			});
		}

		private void InternalRegisterMessage(string messageName)
		{
			lock (_messagesMetadata)
			{
				foreach (MessageMetadata item in from m in _messagesMetadata
				where m.Name == messageName
				select m)
				{
					item.Enabled = true;
					item.Activated = true;
				}
			}
		}

		private void InternalUnRegisterMessage(string messageName)
		{
			lock (_messagesMetadata)
			{
				foreach (MessageMetadata item in from m in _messagesMetadata
				where m.Name == messageName
				select m)
				{
					item.Enabled = false;
					item.Activated = false;
				}
			}
		}
	}
}
