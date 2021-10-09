using System;
using System.Collections.Generic;

namespace Renci.SshNet.Sftp.Responses
{
	internal class SftpVersionResponse : SftpMessage
	{
		public override SftpMessageTypes SftpMessageType => SftpMessageTypes.Version;

		public uint Version
		{
			get;
			private set;
		}

		public IDictionary<string, string> Extentions
		{
			get;
			private set;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Version = ReadUInt32();
			Extentions = ReadExtensionPair();
		}

		protected override void SaveData()
		{
			throw new InvalidOperationException();
		}
	}
}
