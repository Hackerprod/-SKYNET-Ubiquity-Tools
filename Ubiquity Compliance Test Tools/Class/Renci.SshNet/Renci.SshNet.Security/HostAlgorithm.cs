namespace Renci.SshNet.Security
{
	public abstract class HostAlgorithm
	{
		public string Name
		{
			get;
			private set;
		}

		public abstract byte[] Data
		{
			get;
		}

		public HostAlgorithm(string name)
		{
			Name = name;
		}

		public abstract byte[] Sign(byte[] data);

		public abstract bool VerifySignature(byte[] data, byte[] signature);
	}
}
