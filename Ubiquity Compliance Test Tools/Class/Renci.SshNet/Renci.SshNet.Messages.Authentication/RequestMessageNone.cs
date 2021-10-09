namespace Renci.SshNet.Messages.Authentication
{
	internal class RequestMessageNone : RequestMessage
	{
		public override string MethodName => "none";

		public RequestMessageNone(ServiceName serviceName, string username)
			: base(serviceName, username)
		{
		}
	}
}
