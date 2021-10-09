namespace Renci.SshNet.Messages.Connection
{
	[Message("SSH_MSG_CHANNEL_EXTENDED_DATA", 95)]
	public class ChannelExtendedDataMessage : ChannelMessage
	{
		public uint DataTypeCode
		{
			get;
			private set;
		}

		public byte[] Data
		{
			get;
			private set;
		}

		public ChannelExtendedDataMessage()
		{
		}

		public ChannelExtendedDataMessage(uint localChannelNumber, uint dataTypeCode, byte[] data)
		{
			base.LocalChannelNumber = localChannelNumber;
			DataTypeCode = dataTypeCode;
			Data = data;
		}

		protected override void LoadData()
		{
			base.LoadData();
			DataTypeCode = ReadUInt32();
			Data = ReadBinaryString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(DataTypeCode);
			WriteBinaryString(Data);
		}
	}
}
