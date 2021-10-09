namespace Renci.SshNet.Messages.Connection
{
	internal class SignalRequestInfo : RequestInfo
	{
		public const string NAME = "signal";

		public override string RequestName => "signal";

		public string SignalName
		{
			get;
			private set;
		}

		public SignalRequestInfo()
		{
			base.WantReply = false;
		}

		public SignalRequestInfo(string signalName)
			: this()
		{
			SignalName = signalName;
		}

		protected override void LoadData()
		{
			base.LoadData();
			SignalName = ReadAsciiString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteAscii(SignalName);
		}
	}
}
