namespace Renci.SshNet.Messages.Connection
{
	[Message("SSH_MSG_GLOBAL_REQUEST", 80)]
	public class GlobalRequestMessage : Message
	{
		public GlobalRequestName RequestName
		{
			get;
			private set;
		}

		public bool WantReply
		{
			get;
			private set;
		}

		public string AddressToBind
		{
			get;
			private set;
		}

		public uint PortToBind
		{
			get;
			private set;
		}

		public GlobalRequestMessage()
		{
		}

		public GlobalRequestMessage(GlobalRequestName requestName, bool wantReply)
		{
			RequestName = requestName;
			WantReply = wantReply;
		}

		public GlobalRequestMessage(GlobalRequestName requestName, bool wantReply, string addressToBind, uint portToBind)
			: this(requestName, wantReply)
		{
			AddressToBind = addressToBind;
			PortToBind = portToBind;
		}

		protected override void LoadData()
		{
			string text = ReadAsciiString();
			WantReply = ReadBoolean();
			string a;
			if ((a = text) != null)
			{
				if (!(a == "tcpip-forward"))
				{
					if (a == "cancel-tcpip-forward")
					{
						RequestName = GlobalRequestName.CancelTcpIpForward;
						AddressToBind = ReadString();
						PortToBind = ReadUInt32();
					}
				}
				else
				{
					RequestName = GlobalRequestName.TcpIpForward;
					AddressToBind = ReadString();
					PortToBind = ReadUInt32();
				}
			}
		}

		protected override void SaveData()
		{
			switch (RequestName)
			{
			case GlobalRequestName.TcpIpForward:
				WriteAscii("tcpip-forward");
				break;
			case GlobalRequestName.CancelTcpIpForward:
				WriteAscii("cancel-tcpip-forward");
				break;
			}
			Write(WantReply);
			switch (RequestName)
			{
			case GlobalRequestName.TcpIpForward:
			case GlobalRequestName.CancelTcpIpForward:
				Write(AddressToBind);
				Write(PortToBind);
				break;
			}
		}
	}
}
