namespace Renci.SshNet.Messages.Connection
{
	[Message("SSH_MSG_CHANNEL_DATA", 94)]
	public class ChannelDataMessage : ChannelMessage
	{
		public byte[] Data
		{
			get;
			protected set;
		}

		public ChannelDataMessage()
		{
		}

		public ChannelDataMessage(uint localChannelNumber, byte[] data)
		{
			base.LocalChannelNumber = localChannelNumber;
			Data = data;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Data = ReadBinaryString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteBinaryString(Data);
		}
	}
}
