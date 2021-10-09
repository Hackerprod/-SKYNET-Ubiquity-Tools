using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpOpenDirRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.OpenDir;

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

		public SftpOpenDirRequest(uint protocolVersion, uint requestId, string path, Encoding encoding, Action<SftpHandleResponse> handleAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Path = path;
			Encoding = encoding;
			SetAction(handleAction);
		}

		protected override void LoadData()
		{
			base.LoadData();
			Path = ReadString(Encoding);
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Path, Encoding);
		}
	}
}
