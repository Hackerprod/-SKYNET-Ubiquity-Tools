namespace Renci.SshNet.Messages.Authentication
{
	public class RequestMessagePublicKey : RequestMessage
	{
		public override string MethodName => "publickey";

		public string PublicKeyAlgorithmName
		{
			get;
			private set;
		}

		public byte[] PublicKeyData
		{
			get;
			private set;
		}

		public byte[] Signature
		{
			get;
			set;
		}

		public RequestMessagePublicKey(ServiceName serviceName, string username, string keyAlgorithmName, byte[] keyData)
			: base(serviceName, username)
		{
			PublicKeyAlgorithmName = keyAlgorithmName;
			PublicKeyData = keyData;
		}

		public RequestMessagePublicKey(ServiceName serviceName, string username, string keyAlgorithmName, byte[] keyData, byte[] signature)
			: this(serviceName, username, keyAlgorithmName, keyData)
		{
			Signature = signature;
		}

		protected override void SaveData()
		{
			base.SaveData();
			if (Signature == null)
			{
				Write(data: false);
			}
			else
			{
				Write(data: true);
			}
			WriteAscii(PublicKeyAlgorithmName);
			WriteBinaryString(PublicKeyData);
			if (Signature != null)
			{
				WriteBinaryString(Signature);
			}
		}
	}
}
