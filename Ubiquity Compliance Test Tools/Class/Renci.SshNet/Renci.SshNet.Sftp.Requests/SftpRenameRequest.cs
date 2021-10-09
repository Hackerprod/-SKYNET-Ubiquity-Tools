using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpRenameRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Rename;

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

		public SftpRenameRequest(uint protocolVersion, uint requestId, string oldPath, string newPath, Encoding encoding, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			OldPath = oldPath;
			NewPath = newPath;
			Encoding = encoding;
		}

		protected override void LoadData()
		{
			base.LoadData();
			OldPath = ReadString(Encoding);
			NewPath = ReadString(Encoding);
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(OldPath, Encoding);
			Write(NewPath, Encoding);
		}
	}
}
