namespace Renci.SshNet.Messages.Connection
{
	[Message("SSH_MSG_CHANNEL_REQUEST", 98)]
	public class ChannelRequestMessage : ChannelMessage
	{
		public string RequestName
		{
			get;
			private set;
		}

		public byte[] RequestData
		{
			get;
			private set;
		}

		public ChannelRequestMessage()
		{
		}

		public ChannelRequestMessage(uint localChannelName, RequestInfo info)
		{
			base.LocalChannelNumber = localChannelName;
			RequestName = info.RequestName;
			RequestData = info.GetBytes();
		}

		protected override void LoadData()
		{
			base.LoadData();
			RequestName = ReadAsciiString();
			RequestData = ReadBytes();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteAscii(RequestName);
			Write(RequestData);
		}
	}
}
