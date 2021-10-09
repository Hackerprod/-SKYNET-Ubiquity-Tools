using Renci.SshNet.Common;

namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_KEX_DH_GEX_INIT", 32)]
	internal class KeyExchangeDhGroupExchangeInit : Message, IKeyExchangedAllowed
	{
		public BigInteger E
		{
			get;
			private set;
		}

		public KeyExchangeDhGroupExchangeInit(BigInteger clientExchangeValue)
		{
			E = clientExchangeValue;
		}

		protected override void LoadData()
		{
			E = ReadBigInt();
		}

		protected override void SaveData()
		{
			Write(E);
		}
	}
}
