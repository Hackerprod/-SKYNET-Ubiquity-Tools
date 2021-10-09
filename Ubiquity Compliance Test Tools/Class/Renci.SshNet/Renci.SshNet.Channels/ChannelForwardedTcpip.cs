using Renci.SshNet.Common;
using Renci.SshNet.Messages.Connection;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Renci.SshNet.Channels
{
	internal class ChannelForwardedTcpip : ServerChannel
	{
		private Socket _socket;

		public override ChannelTypes ChannelType => ChannelTypes.ForwardedTcpip;

		public void Bind(IPAddress connectedHost, uint connectedPort)
		{
			if (!base.IsConnected)
			{
				throw new SshException("Session is not connected.");
			}
			byte[] array;
			try
			{
				array = new byte[base.RemotePacketSize];
				OpenSocket(connectedHost, connectedPort);
				SendMessage(new ChannelOpenConfirmationMessage(base.RemoteChannelNumber, base.LocalWindowSize, base.LocalPacketSize, base.LocalChannelNumber));
			}
			catch (Exception ex)
			{
				SendMessage(new ChannelOpenFailureMessage(base.RemoteChannelNumber, ex.ToString(), 2u));
				throw;
			}
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
						if (ex2.SocketErrorCode != SocketError.ConnectionAborted)
						{
							throw;
						}
					}
				}
				break;
			}
			Close();
		}

		public override void Close()
		{
			SendMessage(new ChannelEofMessage(base.RemoteChannelNumber));
			base.Close();
		}

		protected override void OnData(byte[] data)
		{
			base.OnData(data);
			InternalSocketSend(data);
		}

		protected override void Dispose(bool disposing)
		{
			if (_socket != null)
			{
				Extensions.Dispose(_socket);
				_socket = null;
			}
			base.Dispose(disposing);
		}

		private void OpenSocket(IPAddress connectedHost, uint connectedPort)
		{
			IPEndPoint iPEndPoint = new IPEndPoint(connectedHost, (int)connectedPort);
			_socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			_socket.Connect(iPEndPoint);
			_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, 1);
		}

		private void InternalSocketReceive(byte[] buffer, ref int read)
		{
			read = _socket.Receive(buffer);
		}

		private void InternalSocketSend(byte[] data)
		{
			_socket.Send(data);
		}
	}
}
