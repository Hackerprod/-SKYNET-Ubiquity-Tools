namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_KEXECDH_REPLY", 31)]
	public class KeyExchangeEcdhReplyMessage : Message
	{
		public byte[] KS
		{
			get;
			private set;
		}

		public byte[] QS
		{
			get;
			private set;
		}

		public byte[] Signature
		{
			get;
			private set;
		}

		protected override void LoadData()
		{
			ResetReader();
			KS = ReadBinaryString();
			QS = ReadBinaryString();
			Signature = ReadBinaryString();
		}

		protected override void SaveData()
		{
			WriteBinaryString(KS);
			WriteBinaryString(QS);
			WriteBinaryString(Signature);
		}
	}
}
