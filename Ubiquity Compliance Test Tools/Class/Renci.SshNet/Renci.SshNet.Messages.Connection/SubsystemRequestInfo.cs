namespace Renci.SshNet.Messages.Connection
{
	internal class SubsystemRequestInfo : RequestInfo
	{
		public const string NAME = "subsystem";

		public override string RequestName => "subsystem";

		public string SubsystemName
		{
			get;
			private set;
		}

		public SubsystemRequestInfo()
		{
			base.WantReply = true;
		}

		public SubsystemRequestInfo(string subsystem)
			: this()
		{
			SubsystemName = subsystem;
		}

		protected override void LoadData()
		{
			base.LoadData();
			SubsystemName = ReadAsciiString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteAscii(SubsystemName);
		}
	}
}
