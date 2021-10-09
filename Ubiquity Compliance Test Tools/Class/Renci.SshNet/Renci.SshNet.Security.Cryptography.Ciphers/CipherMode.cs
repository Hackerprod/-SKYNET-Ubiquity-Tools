using System.Linq;

namespace Renci.SshNet.Security.Cryptography.Ciphers
{
	public abstract class CipherMode
	{
		protected BlockCipher Cipher;

		protected byte[] IV;

		protected int _blockSize;

		protected CipherMode(byte[] iv)
		{
			IV = iv;
		}

		internal void Init(BlockCipher cipher)
		{
			Cipher = cipher;
			_blockSize = cipher.BlockSize;
			IV = IV.Take(_blockSize).ToArray();
		}

		public abstract int EncryptBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

		public abstract int DecryptBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);
	}
}
