using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class HardLinkRequest : SftpExtendedRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Extended;

		public override string Name => "hardlink@openssh.com";

		public string OldPath
		{
			get;
			private set;
		}

		public string NewPath
		{
			get;
			private set;
		}

		public HardLinkRequest(uint protocolVersion, uint requestId, string oldPath, string newPath, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			OldPath = oldPath;
			NewPath = newPath;
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(OldPath);
			Write(NewPath);
		}
	}
}
