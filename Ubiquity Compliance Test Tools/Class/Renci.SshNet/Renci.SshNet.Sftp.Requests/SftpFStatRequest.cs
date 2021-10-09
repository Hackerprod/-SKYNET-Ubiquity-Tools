using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpFStatRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.FStat;

		public byte[] Handle
		{
			get;
			private set;
		}

		public SftpFStatRequest(uint protocolVersion, uint requestId, byte[] handle, Action<SftpAttrsResponse> attrsAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Handle = handle;
			SetAction(attrsAction);
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
