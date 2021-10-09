using Renci.SshNet.Common;

namespace Renci.SshNet.Messages.Connection
{
	public abstract class RequestInfo : SshData
	{
		public abstract string RequestName
		{
			get;
		}

		public bool WantReply
		{
			get;
			protected set;
		}

		protected override void LoadData()
		{
			WantReply = ReadBoolean();
		}

		protected override void SaveData()
		{
			Write(WantReply);
		}
	}
}
