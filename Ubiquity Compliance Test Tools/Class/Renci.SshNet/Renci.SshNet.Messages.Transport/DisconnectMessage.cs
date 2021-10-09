namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_DISCONNECT", 1)]
	public class DisconnectMessage : Message, IKeyExchangedAllowed
	{
		public DisconnectReason ReasonCode
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}

		public string Language
		{
			get;
			private set;
		}

		public DisconnectMessage()
		{
		}

		public DisconnectMessage(DisconnectReason reasonCode, string message)
		{
			ReasonCode = reasonCode;
			Description = message;
		}

		protected override void LoadData()
		{
			ReasonCode = (DisconnectReason)ReadUInt32();
			Description = ReadString();
			Language = ReadString();
		}

		protected override void SaveData()
		{
			Write((uint)ReasonCode);
			Write(Description);
			Write(Language ?? "en");
		}
	}
}
