using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal class SftpReadRequest : SftpRequest
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Read;

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

		public uint Length
		{
			get;
			private set;
		}

		public SftpReadRequest(uint protocolVersion, uint requestId, byte[] handle, ulong offset, uint length, Action<SftpDataResponse> dataAction, Action<SftpStatusResponse> statusAction)
			: base(protocolVersion, requestId, statusAction)
		{
			Handle = handle;
			Offset = offset;
			Length = length;
			SetAction(dataAction);
		}

		protected override void LoadData()
		{
			base.LoadData();
			Handle = ReadBinaryString();
			Offset = ReadUInt64();
			Length = ReadUInt32();
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
