using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpCloseRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Close;

		public byte[] Handle
		{
			get;
			private set;
		}

		public SftpCloseRequest(uint protocolVersion, uint requestId, byte[] handle, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Handle = handle;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Handle = ReadBinaryString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteBinaryString(Handle);
		}
	}
}
