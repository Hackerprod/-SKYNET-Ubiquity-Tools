namespace Renci.SshNet.Messages.Authentication
{
	internal class RequestMessageKeyboardInteractive : RequestMessage
	{
		public override string MethodName => "keyboard-interactive";

		public string Language
		{
			get;
			private set;
		}

		public string SubMethods
		{
			get;
			private set;
		}

		public RequestMessageKeyboardInteractive(ServiceName serviceName, string username)
			: base(serviceName, username)
		{
			Language = string.Empty;
			SubMethods = string.Empty;
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Language);
			Write(SubMethods);
		}
	}
}
