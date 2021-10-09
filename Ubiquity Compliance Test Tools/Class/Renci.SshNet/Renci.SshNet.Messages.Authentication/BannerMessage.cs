namespace Renci.SshNet.Messages.Authentication
{
	[Message("SSH_MSG_USERAUTH_BANNER", 53)]
	public class BannerMessage : Message
	{
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
			Message = ReadString();
			Language = ReadString();
		}

		protected override void SaveData()
		{
			Write(Message);
			Write(Language);
		}
	}
}
