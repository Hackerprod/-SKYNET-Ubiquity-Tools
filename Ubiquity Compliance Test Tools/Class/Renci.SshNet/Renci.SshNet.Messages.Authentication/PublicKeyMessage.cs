namespace Renci.SshNet.Messages.Authentication
{
	[Message("SSH_MSG_USERAUTH_PK_OK", 60)]
	internal class PublicKeyMessage : Message
	{
		public string PublicKeyAlgorithmName
		{
			get;
			private set;
		}

		public byte[] PublicKeyData
		{
			get;
			private set;
		}

		protected override void LoadData()
		{
			PublicKeyAlgorithmName = ReadAsciiString();
			PublicKeyData = ReadBinaryString();
		}

		protected override void SaveData()
		{
			WriteAscii(PublicKeyAlgorithmName);
			WriteBinaryString(PublicKeyData);
		}
	}
}
