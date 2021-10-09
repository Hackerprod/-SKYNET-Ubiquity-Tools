using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpLStatRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.LStat;

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

		public SftpLStatRequest(uint protocolVersion, uint requestId, string path, Encoding encoding, Action<SftpAttrsResponse> attrsAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Path = path;
			Encoding = encoding;
			SetAction(attrsAction);
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
