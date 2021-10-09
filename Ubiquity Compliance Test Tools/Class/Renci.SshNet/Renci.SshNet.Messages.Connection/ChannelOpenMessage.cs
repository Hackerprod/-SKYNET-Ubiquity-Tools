using System;
using System.Globalization;

namespace Renci.SshNet.Messages.Connection
{
	[Message("SSH_MSG_CHANNEL_OPEN", 90)]
	public class ChannelOpenMessage : ChannelMessage
	{
		public string ChannelType => Info.ChannelType;

		public uint InitialWindowSize
		{
			get;
			private set;
		}

		public uint MaximumPacketSize
		{
			get;
			private set;
		}

		public ChannelOpenInfo Info
		{
			get;
			private set;
		}

		public ChannelOpenMessage()
		{
		}

		public ChannelOpenMessage(uint channelNumber, uint initialWindowSize, uint maximumPacketSize, ChannelOpenInfo info)
		{
			base.LocalChannelNumber = channelNumber;
			InitialWindowSize = initialWindowSize;
			MaximumPacketSize = maximumPacketSize;
			Info = info;
		}

		protected override void LoadData()
		{
			string text = ReadAsciiString();
			base.LocalChannelNumber = ReadUInt32();
			InitialWindowSize = ReadUInt32();
			MaximumPacketSize = ReadUInt32();
			byte[] value = ReadBytes();
			if (text == "session")
			{
				Info = new SessionChannelOpenInfo();
			}
			else if (text == "x11")
			{
				Info = new X11ChannelOpenInfo();
			}
			else if (text == "direct-tcpip")
			{
				Info = new DirectTcpipChannelInfo();
			}
			else
			{
				if (!(text == "forwarded-tcpip"))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Channel type '{0}' is not supported.", new object[1]
					{
						text
					}));
				}
				Info = new ForwardedTcpipChannelInfo();
			}
			Info.Load(value);
		}

		protected override void SaveData()
		{
			WriteAscii(ChannelType);
			Write(base.LocalChannelNumber);
			Write(InitialWindowSize);
			Write(MaximumPacketSize);
			Write(Info.GetBytes());
		}
	}
}
