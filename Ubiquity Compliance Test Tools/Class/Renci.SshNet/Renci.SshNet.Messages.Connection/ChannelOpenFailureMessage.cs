namespace Renci.SshNet.Messages.Connection
{
	[Message("SSH_MSG_CHANNEL_OPEN_FAILURE", 92)]
	public class ChannelOpenFailureMessage : ChannelMessage
	{
		public uint ReasonCode
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

		public ChannelOpenFailureMessage()
		{
		}

		public ChannelOpenFailureMessage(uint localChannelNumber, string description, uint reasonCode)
		{
			base.LocalChannelNumber = localChannelNumber;
			Description = description;
			ReasonCode = reasonCode;
		}

		protected override void LoadData()
		{
			base.LoadData();
			ReasonCode = ReadUInt32();
			Description = ReadString();
			Language = ReadString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(ReasonCode);
			Write(Description ?? string.Empty);
			Write(Language ?? "en");
		}
	}
}
