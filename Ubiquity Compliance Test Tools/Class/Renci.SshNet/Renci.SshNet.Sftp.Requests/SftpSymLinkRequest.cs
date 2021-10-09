using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpSymLinkRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.SymLink;

		public string NewLinkPath
		{
			get;
			set;
		}

		public string ExistingPath
		{
			get;
			set;
		}

		public Encoding Encoding
		{
			get;
			set;
		}

		public SftpSymLinkRequest(uint protocolVersion, uint requestId, string newLinkPath, string existingPath, Encoding encoding, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			NewLinkPath = newLinkPath;
			ExistingPath = existingPath;
			Encoding = encoding;
		}

		protected override void LoadData()
		{
			base.LoadData();
			NewLinkPath = ReadString(Encoding);
			ExistingPath = ReadString(Encoding);
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(NewLinkPath, Encoding);
			Write(ExistingPath, Encoding);
		}
	}
}
