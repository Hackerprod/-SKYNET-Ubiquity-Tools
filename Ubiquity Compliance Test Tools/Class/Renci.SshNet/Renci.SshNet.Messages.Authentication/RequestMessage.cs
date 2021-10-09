using System;

namespace Renci.SshNet.Messages.Authentication
{
	[Message("SSH_MSG_USERAUTH_REQUEST", 50)]
	public class RequestMessage : Message
	{
		public string Username
		{
			get;
			private set;
		}

		public ServiceName ServiceName
		{
			get;
			private set;
		}

		public virtual string MethodName => "none";

		public RequestMessage(ServiceName serviceName, string username)
		{
			ServiceName = serviceName;
			Username = username;
		}

		protected override void LoadData()
		{
			throw new InvalidOperationException("Load data is not supported.");
		}

		protected override void SaveData()
		{
			Write(Username);
			switch (ServiceName)
			{
			case ServiceName.UserAuthentication:
				WriteAscii("ssh-userauth");
				break;
			case ServiceName.Connection:
				WriteAscii("ssh-connection");
				break;
			default:
				throw new NotSupportedException("Not supported service name");
			}
			WriteAscii(MethodName);
		}
	}
}
