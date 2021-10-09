namespace Renci.SshNet.Sftp.Responses
{
	internal class SftpHandleResponse : SftpResponse
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Handle;

		public byte[] Handle
		{
			get;
			private set;
		}

		public SftpHandleResponse(uint protocolVersion)
			: base(protocolVersion)
		{
		}

		protected override void LoadData()
		{
			base.LoadData();
			Handle = ReadBinaryString();
		}
	}
}
