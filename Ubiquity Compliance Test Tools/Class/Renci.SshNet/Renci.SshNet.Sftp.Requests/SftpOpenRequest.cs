using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpOpenRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Open;

		public string Filename
		{
			get;
			private set;
		}

		public Flags Flags
		{
			get;
			private set;
		}

		public SftpFileAttributes Attributes
		{
			get;
			private set;
		}

		public Encoding Encoding
		{
			get;
			private set;
		}

		public SftpOpenRequest(uint protocolVersion, uint requestId, string fileName, Encoding encoding, Flags flags, Action<SftpHandleResponse> handleAction, Action<SftpStatusResponse> statusAction)
			: this(protocolVersion, requestId, fileName, encoding, flags, new SftpFileAttributes(), handleAction, statusAction)
		{
		}

		public SftpOpenRequest(uint protocolVersion, uint requestId, string fileName, Encoding encoding, Flags flags, SftpFileAttributes attributes, Action<SftpHandleResponse> handleAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Filename = fileName;
			Flags = flags;
			Attributes = attributes;
			Encoding = encoding;
			SetAction(handleAction);
		}

		protected override void LoadData()
		{
			base.LoadData();
			throw new NotSupportedException();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Filename, Encoding);
			Write((uint)Flags);
			Write(Attributes);
		}
	}
}
