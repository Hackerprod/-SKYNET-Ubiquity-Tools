using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal abstract class SftpExtendedRequest : SftpRequest
	{
		public const string NAME = "posix-rename@openssh.com";

		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Extended;

		public abstract string Name
		{
			get;
		}

		public SftpExtendedRequest(uint protocolVersion, uint requestId, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Name);
		}
	}
}
