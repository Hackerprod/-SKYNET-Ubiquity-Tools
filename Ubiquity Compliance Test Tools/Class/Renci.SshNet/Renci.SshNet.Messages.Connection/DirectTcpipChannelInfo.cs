namespace Renci.SshNet.Messages.Connection
{
	internal class DirectTcpipChannelInfo : ChannelOpenInfo
	{
		public const string NAME = "direct-tcpip";

		public override string ChannelType => "direct-tcpip";

		public string HostToConnect
		{
			get;
			private set;
		}

		public uint PortToConnect
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

		public DirectTcpipChannelInfo()
		{
		}

		public DirectTcpipChannelInfo(string hostToConnect, uint portToConnect, string originatorAddress, uint originatorPort)
		{
			HostToConnect = hostToConnect;
			PortToConnect = portToConnect;
			OriginatorAddress = originatorAddress;
			OriginatorPort = originatorPort;
		}

		protected override void LoadData()
		{
			base.LoadData();
			HostToConnect = ReadString();
			PortToConnect = ReadUInt32();
			OriginatorAddress = ReadString();
			OriginatorPort = ReadUInt32();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(HostToConnect);
			Write(PortToConnect);
			Write(OriginatorAddress);
			Write(OriginatorPort);
		}
	}
}
