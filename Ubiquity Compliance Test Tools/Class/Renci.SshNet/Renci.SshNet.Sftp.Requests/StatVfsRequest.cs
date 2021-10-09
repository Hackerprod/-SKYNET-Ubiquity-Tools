using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class StatVfsRequest : SftpExtendedRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Extended;

		public override string Name => "statvfs@openssh.com";

		public string Path
		{
			get;
			private set;
		}

		public Encoding Encoding
		{
			get;
			private set;
		}

		public StatVfsRequest(uint protocolVersion, uint requestId, string path, Encoding encoding, Action<SftpExtendedReplyResponse> extendedAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Path = path;
			Encoding = encoding;
			SetAction(extendedAction);
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Path, Encoding);
		}
	}
}
