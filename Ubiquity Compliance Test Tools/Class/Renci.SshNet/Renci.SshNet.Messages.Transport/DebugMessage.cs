namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_DEBUG", 4)]
	public class DebugMessage : Message
	{
		public bool IsAlwaysDisplay
		{
			get;
			private set;
		}

		public string Message
		{
			get;
			private set;
		}

		public string Language
		{
			get;
			private set;
		}

		protected override void LoadData()
		{
			IsAlwaysDisplay = ReadBoolean();
			Message = ReadString();
			Language = ReadString();
		}

		protected override void SaveData()
		{
			Write(IsAlwaysDisplay);
			Write(Message);
			Write(Language);
		}
	}
}
