namespace Renci.SshNet.Messages.Connection
{
	internal class ExitSignalRequestInfo : RequestInfo
	{
		public const string NAME = "exit-signal";

		public override string RequestName => "exit-signal";

		public string SignalName
		{
			get;
			private set;
		}

		public bool CoreDumped
		{
			get;
			private set;
		}

		public string ErrorMessage
		{
			get;
			private set;
		}

		public string Language
		{
			get;
			private set;
		}

		public ExitSignalRequestInfo()
		{
			base.WantReply = false;
		}

		public ExitSignalRequestInfo(string signalName, bool coreDumped, string errorMessage, string language)
			: this()
		{
			SignalName = signalName;
			CoreDumped = coreDumped;
			ErrorMessage = errorMessage;
			Language = language;
		}

		protected override void LoadData()
		{
			base.LoadData();
			SignalName = ReadAsciiString();
			CoreDumped = ReadBoolean();
			ErrorMessage = ReadString();
			Language = ReadString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteAscii(SignalName);
			Write(CoreDumped);
			Write(ErrorMessage);
			Write(Language);
		}
	}
}
