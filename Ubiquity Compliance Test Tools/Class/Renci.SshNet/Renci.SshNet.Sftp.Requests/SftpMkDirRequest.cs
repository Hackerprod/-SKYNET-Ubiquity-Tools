using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpMkDirRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.MkDir;

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

		public SftpFileAttributes Attributes
		{
			get;
			private set;
		}

		public SftpMkDirRequest(uint protocolVersion, uint requestId, string path, Encoding encoding, Action<SftpStatusResponse> statusAction)
			: this(protocolVersion, requestId, path, encoding, null, statusAction)
		{
		}

		public SftpMkDirRequest(uint protocolVersion, uint requestId, string path, Encoding encoding, SftpFileAttributes attributes, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Path = path;
			Encoding = encoding;
			Attributes = attributes;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Path = ReadString(Encoding);
			Attributes = ReadAttributes();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Path, Encoding);
			Write(Attributes);
		}
	}
}
