using Renci.SshNet.Common;

namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_KEX_DH_GEX_REPLY", 33)]
	internal class KeyExchangeDhGroupExchangeReply : Message
	{
		public byte[] HostKey
		{
			get;
			private set;
		}

		public BigInteger F
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
			HostKey = ReadBinaryString();
			F = ReadBigInt();
			Signature = ReadBinaryString();
		}

		protected override void SaveData()
		{
			WriteBinaryString(HostKey);
			Write(F);
			WriteBinaryString(Signature);
		}
	}
}
