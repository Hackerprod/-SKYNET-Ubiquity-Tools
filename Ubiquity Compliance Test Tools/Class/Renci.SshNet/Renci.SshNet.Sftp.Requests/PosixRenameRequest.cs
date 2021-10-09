using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class PosixRenameRequest : SftpExtendedRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Extended;

		public override string Name => "posix-rename@openssh.com";

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

		public Encoding Encoding
		{
			get;
			private set;
		}

		public PosixRenameRequest(uint protocolVersion, uint requestId, string oldPath, string newPath, Encoding encoding, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			OldPath = oldPath;
			NewPath = newPath;
			Encoding = encoding;
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(OldPath, Encoding);
			Write(NewPath, Encoding);
		}
	}
}
