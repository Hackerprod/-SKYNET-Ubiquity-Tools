using Renci.SshNet.Common;

namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_KEXDH_REPLY", 31)]
	public class KeyExchangeDhReplyMessage : Message
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
			ResetReader();
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
