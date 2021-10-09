using Renci.SshNet.Security.Cryptography;
using System;
using System.Linq;

namespace Renci.SshNet
{
	public class CipherInfo
	{
		public int KeySize
		{
			get;
			private set;
		}

		public Func<byte[], byte[], Cipher> Cipher
		{
			get;
			private set;
		}

		public CipherInfo(int keySize, Func<byte[], byte[], Cipher> cipher)
		{
			KeySize = keySize;
			Func<byte[], byte[], Cipher> func2 = Cipher = ((byte[] key, byte[] iv) => cipher(key.Take(KeySize / 8).ToArray(), iv));
		}
	}
}
