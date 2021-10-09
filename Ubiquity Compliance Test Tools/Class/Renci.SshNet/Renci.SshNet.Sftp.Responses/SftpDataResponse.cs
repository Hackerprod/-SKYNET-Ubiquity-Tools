namespace Renci.SshNet.Sftp.Responses
{
	internal class SftpDataResponse : SftpResponse
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Data;

		public byte[] Data
		{
			get;
			set;
		}

		public bool IsEof
		{
			get;
			set;
		}

		public SftpDataResponse(uint protocolVersion)
			: base(protocolVersion)
		{
		}

		protected override void LoadData()
		{
			base.LoadData();
			Data = ReadBinaryString();
			if (!base.IsEndOfData)
			{
				IsEof = ReadBoolean();
			}
		}
	}
}
