using System;

namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_SERVICE_REQUEST", 5)]
	public class ServiceRequestMessage : Message
	{
		public ServiceName ServiceName
		{
			get;
			private set;
		}

		public ServiceRequestMessage(ServiceName serviceName)
		{
			ServiceName = serviceName;
		}

		protected override void LoadData()
		{
			throw new InvalidOperationException("Load data is not supported.");
		}

		protected override void SaveData()
		{
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
		}
	}
}
