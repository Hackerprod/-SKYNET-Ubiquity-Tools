namespace Renci.SshNet.Messages.Connection
{
	internal class ForwardedTcpipChannelInfo : ChannelOpenInfo
	{
		public const string NAME = "forwarded-tcpip";

		public override string ChannelType => "forwarded-tcpip";

		public string ConnectedAddress
		{
			get;
			private set;
		}

		public uint ConnectedPort
		{
			get;
			private set;
		}

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
			ConnectedAddress = ReadString();
			ConnectedPort = ReadUInt32();
			OriginatorAddress = ReadString();
			OriginatorPort = ReadUInt32();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(ConnectedAddress);
			Write(ConnectedPort);
			Write(OriginatorAddress);
			Write(OriginatorPort);
		}
	}
}
