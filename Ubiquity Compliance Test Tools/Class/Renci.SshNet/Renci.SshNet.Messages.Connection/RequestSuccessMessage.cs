namespace Renci.SshNet.Messages.Connection
{
	[Message("SSH_MSG_REQUEST_SUCCESS", 81)]
	public class RequestSuccessMessage : Message
	{
		public uint? BoundPort
		{
			get;
			private set;
		}

		public RequestSuccessMessage()
		{
		}

		public RequestSuccessMessage(uint boundPort)
		{
			BoundPort = boundPort;
		}

		protected override void LoadData()
		{
			if (!base.IsEndOfData)
			{
				BoundPort = ReadUInt32();
			}
		}

		protected override void SaveData()
		{
			if (BoundPort.HasValue)
			{
				Write(BoundPort.Value);
			}
		}
	}
}
