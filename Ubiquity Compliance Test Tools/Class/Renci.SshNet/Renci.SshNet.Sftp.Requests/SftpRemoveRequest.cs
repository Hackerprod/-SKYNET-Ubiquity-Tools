using Renci.SshNet.Sftp.Responses;
using System;
using System.Text;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpRemoveRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Remove;

		public string Filename
		{
			get;
			private set;
		}

		public Encoding Encoding
		{
			get;
			private set;
		}

		public SftpRemoveRequest(uint protocolVersion, uint requestId, string filename, Encoding encoding, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Filename = filename;
			Encoding = encoding;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Filename = ReadString(Encoding);
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Filename, Encoding);
		}
	}
}
