namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_IGNORE", 2)]
	public class IgnoreMessage : Message
	{
		public byte[] Data
		{
			get;
			private set;
		}

		public IgnoreMessage()
		{
			Data = new byte[0];
		}

		public IgnoreMessage(byte[] data)
		{
			Data = data;
		}

		protected override void LoadData()
		{
			Data = ReadBinaryString();
		}

		protected override void SaveData()
		{
			WriteBinaryString(Data);
		}
	}
}
