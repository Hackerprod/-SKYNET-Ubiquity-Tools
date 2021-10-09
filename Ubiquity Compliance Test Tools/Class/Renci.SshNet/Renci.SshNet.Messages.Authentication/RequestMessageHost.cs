namespace Renci.SshNet.Messages.Authentication
{
	internal class RequestMessageHost : RequestMessage
	{
		public override string MethodName => "hostbased";

		public string PublicKeyAlgorithm
		{
			get;
			private set;
		}

		public byte[] PublicHostKey
		{
			get;
			private set;
		}

		public string ClientHostName
		{
			get;
			private set;
		}

		public string ClientUsername
		{
			get;
			private set;
		}

		public byte[] Signature
		{
			get;
			set;
		}

		public RequestMessageHost(ServiceName serviceName, string username, string publicKeyAlgorithm, byte[] publicHostKey, string clientHostName, string clientUsername)
			: base(serviceName, username)
		{
			PublicKeyAlgorithm = publicKeyAlgorithm;
			PublicHostKey = publicHostKey;
			ClientHostName = clientHostName;
			ClientUsername = clientUsername;
		}

		protected override void SaveData()
		{
			base.SaveData();
			WriteAscii(PublicKeyAlgorithm);
			WriteBinaryString(PublicHostKey);
			Write(ClientHostName);
			Write(ClientUsername);
			if (Signature != null)
			{
				WriteBinaryString(Signature);
			}
		}
	}
}
