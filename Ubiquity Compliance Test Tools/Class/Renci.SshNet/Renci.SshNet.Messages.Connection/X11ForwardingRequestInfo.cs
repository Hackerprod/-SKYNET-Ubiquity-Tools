namespace Renci.SshNet.Messages.Connection
{
	internal class X11ForwardingRequestInfo : RequestInfo
	{
		public const string NAME = "x11-req";

		public override string RequestName => "x11-req";

		public bool IsSingleConnection
		{
			get;
			set;
		}

		public string AuthenticationProtocol
		{
			get;
			set;
		}

		public byte[] AuthenticationCookie
		{
			get;
			set;
		}

		public uint ScreenNumber
		{
			get;
			set;
		}

		public X11ForwardingRequestInfo()
		{
			base.WantReply = true;
		}

		public X11ForwardingRequestInfo(bool isSingleConnection, string protocol, byte[] cookie, uint screenNumber)
			: this()
		{
			IsSingleConnection = isSingleConnection;
			AuthenticationProtocol = protocol;
			AuthenticationCookie = cookie;
			ScreenNumber = screenNumber;
		}

		protected override void LoadData()
		{
			base.LoadData();
			IsSingleConnection = ReadBoolean();
			AuthenticationProtocol = ReadAsciiString();
			AuthenticationCookie = ReadBinaryString();
			ScreenNumber = ReadUInt32();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(IsSingleConnection);
			WriteAscii(AuthenticationProtocol);
			WriteBinaryString(AuthenticationCookie);
			Write(ScreenNumber);
		}
	}
}
