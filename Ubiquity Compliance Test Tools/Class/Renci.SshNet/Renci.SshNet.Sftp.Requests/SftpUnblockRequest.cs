using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpUnblockRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Block;

		public byte[] Handle
		{
			get;
			private set;
		}

		public ulong Offset
		{
			get;
			private set;
		}

		public ulong Length
		{
			get;
			private set;
		}

		public SftpUnblockRequest(uint protocolVersion, uint requestId, byte[] handle, ulong offset, ulong length, uint lockMask, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Handle = handle;
			Offset = offset;
			Length = length;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Handle = ReadBinaryString();
			Offset = ReadUInt64();
			Length = ReadUInt64();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteBinaryString(Handle);
			Write(Offset);
			Write(Length);
		}
	}
}
