namespace Renci.SshNet.Security.Cryptography.Ciphers
{
	public abstract class CipherPadding
	{
		public abstract byte[] Pad(int blockSize, byte[] input);
	}
}
