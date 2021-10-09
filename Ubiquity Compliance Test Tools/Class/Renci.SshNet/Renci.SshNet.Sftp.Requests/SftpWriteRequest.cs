using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpWriteRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Write;

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

		public byte[] Data
		{
			get;
			private set;
		}

		public SftpWriteRequest(uint protocolVersion, uint requestId, byte[] handle, ulong offset, byte[] data, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Handle = handle;
			Offset = offset;
			Data = data;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Handle = ReadBinaryString();
			Offset = ReadUInt64();
			Data = ReadBinaryString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteBinaryString(Handle);
			Write(Offset);
			WriteBinaryString(Data);
		}
	}
}
