using System;
using System.Text;

namespace Renci.SshNet.Messages.Connection
{
	internal class ExecRequestInfo : RequestInfo
	{
		public const string NAME = "exec";

		public override string RequestName => "exec";

		public string Command
		{
			get;
			private set;
		}

		public Encoding Encoding
		{
			get;
			private set;
		}

		public ExecRequestInfo()
		{
			base.WantReply = true;
		}

		public ExecRequestInfo(string command, Encoding encoding)
			: this()
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			Command = command;
			Encoding = encoding;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Command = ReadString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Command, Encoding);
		}
	}
}
