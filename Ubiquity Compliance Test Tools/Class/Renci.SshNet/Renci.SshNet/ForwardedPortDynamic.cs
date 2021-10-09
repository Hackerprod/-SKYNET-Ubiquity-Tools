using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Renci.SshNet
{
	public class ForwardedPortDynamic : ForwardedPort, IDisposable
	{
		private EventWaitHandle _listenerTaskCompleted;

		private bool _isDisposed;

		private TcpListener _listener;

		private readonly object _listenerLocker = new object();

		public string BoundHost
		{
			get;
			protected set;
		}

		public uint BoundPort
		{
			get;
			protected set;
		}

		public ForwardedPortDynamic(uint port)
			: this(string.Empty, port)
		{
		}

		public ForwardedPortDynamic(string host, uint port)
		{
			BoundHost = host;
			BoundPort = port;
		}

		public override void Start()
		{
			InternalStart();
		}

		public override void Stop()
		{
			base.Stop();
			InternalStop();
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
				InternalStop();
				if (disposing && _listenerTaskCompleted != null)
				{
					Extensions.Dispose(_listenerTaskCompleted);
					_listenerTaskCompleted = null;
				}
				_isDisposed = true;
			}
		}

		~ForwardedPortDynamic()
		{
			Dispose(disposing: false);
		}

		private void InternalStart()
		{
			if (!base.IsStarted)
			{
				IPAddress address = IPAddress.Any;
				if (!string.IsNullOrEmpty(BoundHost))
				{
					address = BoundHost.GetIPAddress();
				}
				IPEndPoint localEP = new IPEndPoint(address, (int)BoundPort);
				_listener = new TcpListener(localEP);
				_listener.Start();
				_listenerTaskCompleted = new ManualResetEvent(initialState: false);
				ExecuteThread(delegate
				{
					try
					{
						while (true)
						{
							ForwardedPortDynamic forwardedPortDynamic = this;
							lock (_listenerLocker)
							{
								if (_listener == null)
								{
									return;
								}
							}
							Socket socket = _listener.AcceptSocket();
							ExecuteThread(delegate
							{
								try
								{
									using (ChannelDirectTcpip channelDirectTcpip = forwardedPortDynamic.Session.CreateClientChannel<ChannelDirectTcpip>())
									{
										byte[] array = new byte[1];
										socket.Receive(array);
										if (array[0] == 4)
										{
											forwardedPortDynamic.HandleSocks4(socket, channelDirectTcpip);
										}
										else
										{
											if (array[0] != 5)
											{
												throw new NotSupportedException($"SOCKS version {array} is not supported.");
											}
											forwardedPortDynamic.HandleSocks5(socket, channelDirectTcpip);
										}
										channelDirectTcpip.Bind();
										channelDirectTcpip.Close();
									}
								}
								catch (Exception execption2)
								{
									forwardedPortDynamic.RaiseExceptionEvent(execption2);
								}
							});
						}
					}
					catch (SocketException ex)
					{
						if (ex.SocketErrorCode != SocketError.Interrupted)
						{
							RaiseExceptionEvent(ex);
						}
					}
					catch (Exception execption)
					{
						RaiseExceptionEvent(execption);
					}
					finally
					{
						_listenerTaskCompleted.Set();
					}
				});
				base.IsStarted = true;
			}
		}

		private void InternalStop()
		{
			if (base.IsStarted)
			{
				lock (_listenerLocker)
				{
					_listener.Stop();
					_listener = null;
				}
				_listenerTaskCompleted.WaitOne(base.Session.ConnectionInfo.Timeout);
				Extensions.Dispose(_listenerTaskCompleted);
				_listenerTaskCompleted = null;
				base.IsStarted = false;
			}
		}

		private void HandleSocks4(Socket socket, ChannelDirectTcpip channel)
		{
			using (NetworkStream networkStream = new NetworkStream(socket))
			{
				networkStream.ReadByte();
				byte[] array = new byte[2];
				networkStream.Read(array, 0, array.Length);
				uint port = (uint)(array[0] * 256 + array[1]);
				byte[] array2 = new byte[4];
				networkStream.Read(array2, 0, array2.Length);
				IPAddress iPAddress = new IPAddress(array2);
				ReadString(networkStream);
				string text = iPAddress.ToString();
				RaiseRequestReceived(text, port);
				channel.Open(text, port, socket);
				networkStream.WriteByte(0);
				if (channel.IsOpen)
				{
					networkStream.WriteByte(90);
				}
				else
				{
					networkStream.WriteByte(91);
				}
				networkStream.Write(array, 0, array.Length);
				networkStream.Write(array2, 0, array2.Length);
			}
		}

		private void HandleSocks5(Socket socket, ChannelDirectTcpip channel)
		{
			using (NetworkStream networkStream = new NetworkStream(socket))
			{
				int num = networkStream.ReadByte();
				byte[] array = new byte[num];
				networkStream.Read(array, 0, array.Length);
				networkStream.WriteByte(5);
				if (array.Min() == 0)
				{
					networkStream.WriteByte(0);
				}
				else
				{
					networkStream.WriteByte(byte.MaxValue);
				}
				int num2 = networkStream.ReadByte();
				if (num2 != 5)
				{
					throw new ProxyException("SOCKS5: Version 5 is expected.");
				}
				networkStream.ReadByte();
				if (networkStream.ReadByte() != 0)
				{
					throw new ProxyException("SOCKS5: 0 is expected.");
				}
				int num3 = networkStream.ReadByte();
				IPAddress iPAddress;
				switch (num3)
				{
				case 1:
				{
					byte[] array2 = new byte[4];
					networkStream.Read(array2, 0, 4);
					iPAddress = new IPAddress(array2);
					break;
				}
				case 3:
				{
					int num4 = networkStream.ReadByte();
					byte[] array2 = new byte[num4];
					networkStream.Read(array2, 0, array2.Length);
					iPAddress = IPAddress.Parse(new Renci.SshNet.Common.ASCIIEncoding().GetString(array2));
					break;
				}
				case 4:
				{
					byte[] array2 = new byte[16];
					networkStream.Read(array2, 0, 16);
					iPAddress = new IPAddress(array2);
					break;
				}
				default:
					throw new ProxyException($"SOCKS5: Address type '{num3}' is not supported.");
				}
				byte[] array3 = new byte[2];
				networkStream.Read(array3, 0, array3.Length);
				uint port = (uint)(array3[0] * 256 + array3[1]);
				string text = iPAddress.ToString();
				RaiseRequestReceived(text, port);
				channel.Open(text, port, socket);
				networkStream.WriteByte(5);
				if (channel.IsOpen)
				{
					networkStream.WriteByte(0);
				}
				else
				{
					networkStream.WriteByte(1);
				}
				networkStream.WriteByte(0);
				byte[] addressBytes = iPAddress.GetAddressBytes();
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					networkStream.WriteByte(1);
				}
				else
				{
					if (iPAddress.AddressFamily != AddressFamily.InterNetwork)
					{
						throw new NotSupportedException("Not supported address family.");
					}
					networkStream.WriteByte(4);
				}
				networkStream.Write(addressBytes, 0, addressBytes.Length);
				networkStream.Write(array3, 0, array3.Length);
			}
		}

		private static string ReadString(NetworkStream stream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (char c = (char)stream.ReadByte(); c != 0; c = (char)stream.ReadByte())
			{
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		private void ExecuteThread(Action action)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				action();
			});
		}
	}
}
