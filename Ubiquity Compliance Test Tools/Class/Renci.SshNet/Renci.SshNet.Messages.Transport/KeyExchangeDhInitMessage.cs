using Renci.SshNet.Common;

namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_KEXDH_INIT", 30)]
	internal class KeyExchangeDhInitMessage : Message, IKeyExchangedAllowed
	{
		public BigInteger E
		{
			get;
			private set;
		}

		public KeyExchangeDhInitMessage(BigInteger clientExchangeValue)
		{
			E = clientExchangeValue;
		}

		protected override void LoadData()
		{
			ResetReader();
			E = ReadBigInt();
		}

		protected override void SaveData()
		{
			Write(E);
		}
	}
}
