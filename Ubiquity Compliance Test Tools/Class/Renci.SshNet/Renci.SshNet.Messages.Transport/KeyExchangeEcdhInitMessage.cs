using Renci.SshNet.Common;
using System.Collections.Generic;
using System.Linq;

namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_KEXECDH_INIT", 30)]
	internal class KeyExchangeEcdhInitMessage : Message, IKeyExchangedAllowed
	{
		public byte[] QC
		{
			get;
			private set;
		}

		public KeyExchangeEcdhInitMessage(BigInteger d, BigInteger q)
		{
			List<byte> list = new List<byte>();
			list.Add(4);
			list.AddRange(d.ToByteArray().Reverse());
			list.AddRange(q.ToByteArray().Reverse());
			QC = list.ToArray();
		}

		protected override void LoadData()
		{
			ResetReader();
			QC = ReadBinaryString();
		}

		protected override void SaveData()
		{
			WriteBinaryString(QC);
		}
	}
}
