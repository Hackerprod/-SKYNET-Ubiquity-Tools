using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class FStatVfsRequest : SftpExtendedRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Extended;

		public override string Name => "fstatvfs@openssh.com";

		public byte[] Handle
		{
			get;
			private set;
		}

		public FStatVfsRequest(uint protocolVersion, uint requestId, byte[] handle, Action<SftpExtendedReplyResponse> extendedAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Handle = handle;
			SetAction(extendedAction);
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteBinaryString(Handle);
		}
	}
}
