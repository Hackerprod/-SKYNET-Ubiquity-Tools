namespace Renci.SshNet.Messages.Connection
{
	internal class XonXoffRequestInfo : RequestInfo
	{
		public const string NAME = "xon-xoff";

		public override string RequestName => "xon-xoff";

		public bool ClientCanDo
		{
			get;
			set;
		}

		public XonXoffRequestInfo()
		{
			base.WantReply = false;
		}

		public XonXoffRequestInfo(bool clientCanDo)
			: this()
		{
			ClientCanDo = clientCanDo;
		}

		protected override void LoadData()
		{
			base.LoadData();
			ClientCanDo = ReadBoolean();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(ClientCanDo);
		}
	}
}
