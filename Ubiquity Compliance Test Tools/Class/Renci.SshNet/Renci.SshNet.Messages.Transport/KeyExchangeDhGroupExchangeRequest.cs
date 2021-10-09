namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_KEX_DH_GEX_REQUEST", 34)]
	internal class KeyExchangeDhGroupExchangeRequest : Message, IKeyExchangedAllowed
	{
		public uint Minimum
		{
			get;
			private set;
		}

		public uint Preferred
		{
			get;
			private set;
		}

		public uint Maximum
		{
			get;
			private set;
		}

		public KeyExchangeDhGroupExchangeRequest(uint minimum, uint preferred, uint maximum)
		{
			Minimum = minimum;
			Preferred = preferred;
			Maximum = maximum;
		}

		protected override void LoadData()
		{
			Minimum = ReadUInt32();
			Preferred = ReadUInt32();
			Maximum = ReadUInt32();
		}

		protected override void SaveData()
		{
			Write(Minimum);
			Write(Preferred);
			Write(Maximum);
		}
	}
}
