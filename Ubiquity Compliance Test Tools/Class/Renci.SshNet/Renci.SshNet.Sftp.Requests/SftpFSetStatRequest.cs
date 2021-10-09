using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpFSetStatRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.FSetStat;

		public byte[] Handle
		{
			get;
			private set;
		}

		public SftpFileAttributes Attributes
		{
			get;
			private set;
		}

		public SftpFSetStatRequest(uint protocolVersion, uint requestId, byte[] handle, SftpFileAttributes attributes, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Handle = handle;
			Attributes = attributes;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Handle = ReadBinaryString();
			Attributes = ReadAttributes();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteBinaryString(Handle);
			Write(Attributes);
		}
	}
}
