using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpRealPathRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.RealPath;

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

		public SftpRealPathRequest(uint protocolVersion, uint requestId, string path, Encoding encoding, Action<SftpNameResponse> nameAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			if (nameAction == null)
			{
				throw new ArgumentNullException("nameAction");
			}
			if (statusAction == null)
			{
				throw new ArgumentNullException("statusAction");
			}
			Path = path;
			Encoding = encoding;
			SetAction(nameAction);
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Path, Encoding);
		}
	}
}
