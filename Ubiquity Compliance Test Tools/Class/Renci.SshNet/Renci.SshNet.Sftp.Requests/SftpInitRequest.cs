namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpInitRequest : SftpMessage
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Init;

		public uint Version
		{
			get;
			private set;
		}

		public SftpInitRequest(uint version)
		{
			Version = version;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Version = ReadUInt32();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Version);
		}
	}
}
