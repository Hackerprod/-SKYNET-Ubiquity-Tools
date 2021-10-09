using Renci.SshNet.Common;
using Renci.SshNet.Messages.Connection;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Renci.SshNet.Channels
{
	internal class ChannelDirectTcpip : ClientChannel
	{
		private EventWaitHandle _channelEof = new AutoResetEvent(initialState: false);

		private EventWaitHandle _channelOpen = new AutoResetEvent(initialState: false);

		private EventWaitHandle _channelData = new AutoResetEvent(initialState: false);

		private Socket _socket;

		public override ChannelTypes ChannelType => ChannelTypes.DirectTcpip;

		public void Open(string remoteHost, uint port, Socket socket)
		{
			_socket = socket;
			IPEndPoint iPEndPoint = socket.RemoteEndPoint as IPEndPoint;
			if (!base.IsConnected)
			{
				throw new SshException("Session is not connected.");
			}
			SendMessage(new ChannelOpenMessage(base.LocalChannelNumber, base.LocalWindowSize, base.LocalPacketSize, new DirectTcpipChannelInfo(remoteHost, port, iPEndPoint.Address.ToString(), (uint)iPEndPoint.Port)));
			WaitOnHandle(_channelOpen);
		}

		public void Bind()
		{
			if (base.IsOpen)
			{
				Exception ex = null;
				try
				{
					byte[] array = new byte[base.RemotePacketSize];
					while (true)
					{
						if (_socket != null && _socket.CanRead())
						{
							try
							{
								int read = 0;
								InternalSocketReceive(array, ref read);
								if (read > 0)
								{
									SendMessage(new ChannelDataMessage(base.RemoteChannelNumber, array.Take(read).ToArray()));
									continue;
								}
							}
							catch (SocketException ex2)
							{
								if (ex2.SocketErrorCode == SocketError.WouldBlock || ex2.SocketErrorCode == SocketError.IOPending || ex2.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
								{
									Thread.Sleep(30);
									continue;
								}
								if (ex2.SocketErrorCode != SocketError.ConnectionAborted && ex2.SocketErrorCode != SocketError.ConnectionReset)
								{
									throw;
								}
							}
						}
						break;
					}
				}
				catch (Exception ex3)
				{
					ex = ex3;
				}
				WaitHandle.WaitAny(new WaitHandle[1]
				{
					_channelEof
				});
				if (_socket != null)
				{
					Extensions.Dispose(_socket);
					_socket = null;
				}
				if (ex != null)
				{
					throw ex;
				}
			}
		}

		public override void Close()
		{
			if (_socket != null)
			{
				Extensions.Dispose(_socket);
				_socket = null;
			}
			SendMessage(new ChannelEofMessage(base.RemoteChannelNumber));
			base.Close();
		}

		protected override void OnData(byte[] data)
		{
			base.OnData(data);
			InternalSocketSend(data);
		}

		protected override void OnOpenConfirmation(uint remoteChannelNumber, uint initialWindowSize, uint maximumPacketSize)
		{
			base.OnOpenConfirmation(remoteChannelNumber, initialWindowSize, maximumPacketSize);
			_channelOpen.Set();
		}

		protected override void OnOpenFailure(uint reasonCode, string description, string language)
		{
			base.OnOpenFailure(reasonCode, description, language);
			_channelOpen.Set();
		}

		protected override void OnEof()
		{
			base.OnEof();
			_channelEof?.Set();
		}

		protected override void OnClose()
		{
			base.OnClose();
			_channelEof?.Set();
		}

		protected override void OnErrorOccured(Exception exp)
		{
			base.OnErrorOccured(exp);
			_channelEof?.Set();
		}

		protected override void OnDisconnected()
		{
			base.OnDisconnected();
			_channelEof?.Set();
		}

		protected override void Dispose(bool disposing)
		{
			if (_socket != null)
			{
				Extensions.Dispose(_socket);
				_socket = null;
			}
			if (_channelEof != null)
			{
				Extensions.Dispose(_channelEof);
				_channelEof = null;
			}
			if (_channelOpen != null)
			{
				Extensions.Dispose(_channelOpen);
				_channelOpen = null;
			}
			if (_channelData != null)
			{
				Extensions.Dispose(_channelData);
				_channelData = null;
			}
			base.Dispose(disposing);
		}

		private void ExecuteThread(Action action)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				action();
			});
		}

		private void InternalSocketReceive(byte[] buffer, ref int read)
		{
			read = _socket.Receive(buffer);
		}

		private void InternalSocketSend(byte[] data)
		{
			_socket.Send(data, 0, data.Length, SocketFlags.None);
		}
	}
}
