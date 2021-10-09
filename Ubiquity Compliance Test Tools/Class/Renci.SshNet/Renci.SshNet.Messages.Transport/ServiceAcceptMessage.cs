using System;

namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_SERVICE_ACCEPT", 6)]
	public class ServiceAcceptMessage : Message
	{
		public ServiceName ServiceName
		{
			get;
			private set;
		}

		protected override void LoadData()
		{
			string text = ReadAsciiString();
			string a;
			if ((a = text) != null)
			{
				if (!(a == "ssh-userauth"))
				{
					if (a == "ssh-connection")
					{
						ServiceName = ServiceName.Connection;
					}
				}
				else
				{
					ServiceName = ServiceName.UserAuthentication;
				}
			}
		}

		protected override void SaveData()
		{
			throw new InvalidOperationException("Save data is not supported.");
		}
	}
}
