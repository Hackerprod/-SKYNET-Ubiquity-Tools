using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpReadDirRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.ReadDir;

		public byte[] Handle
		{
			get;
			private set;
		}

		public SftpReadDirRequest(uint protocolVersion, uint requestId, byte[] handle, Action<SftpNameResponse> nameAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Handle = handle;
			SetAction(nameAction);
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
