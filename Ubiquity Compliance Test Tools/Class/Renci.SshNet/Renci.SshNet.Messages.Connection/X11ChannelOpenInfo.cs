namespace Renci.SshNet.Messages.Connection
{
	internal class X11ChannelOpenInfo : ChannelOpenInfo
	{
		public const string NAME = "x11";

		public override string ChannelType => "x11";

		public string OriginatorAddress
		{
			get;
			private set;
		}

		public uint OriginatorPort
		{
			get;
			private set;
		}

		protected override void LoadData()
		{
			base.LoadData();
			OriginatorAddress = ReadString();
			OriginatorPort = ReadUInt32();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(OriginatorAddress);
			Write(OriginatorPort);
		}
	}
}
