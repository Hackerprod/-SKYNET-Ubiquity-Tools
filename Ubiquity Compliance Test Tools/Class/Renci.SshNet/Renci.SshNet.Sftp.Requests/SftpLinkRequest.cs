using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpLinkRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Link;

		public string NewLinkPath
		{
			get;
			private set;
		}

		public string ExistingPath
		{
			get;
			private set;
		}

		public bool IsSymLink
		{
			get;
			private set;
		}

		public SftpLinkRequest(uint protocolVersion, uint requestId, string newLinkPath, string existingPath, bool isSymLink, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			NewLinkPath = newLinkPath;
			ExistingPath = existingPath;
			IsSymLink = isSymLink;
		}

		protected override void LoadData()
		{
			base.LoadData();
			NewLinkPath = ReadString();
			ExistingPath = ReadString();
			IsSymLink = ReadBoolean();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(NewLinkPath);
			Write(ExistingPath);
			Write(IsSymLink);
		}
	}
}
