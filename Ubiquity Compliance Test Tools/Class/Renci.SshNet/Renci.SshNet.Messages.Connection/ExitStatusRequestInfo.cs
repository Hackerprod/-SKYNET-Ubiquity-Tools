namespace Renci.SshNet.Messages.Connection
{
	internal class ExitStatusRequestInfo : RequestInfo
	{
		public const string NAME = "exit-status";

		public override string RequestName => "exit-status";

		public uint ExitStatus
		{
			get;
			private set;
		}

		public ExitStatusRequestInfo()
		{
			base.WantReply = false;
		}

		public ExitStatusRequestInfo(uint exitStatus)
			: this()
		{
			ExitStatus = exitStatus;
		}

		protected override void LoadData()
		{
			base.LoadData();
			ExitStatus = ReadUInt32();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(ExitStatus);
		}
	}
}
