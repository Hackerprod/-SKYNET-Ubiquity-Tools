using System;
using System.Linq;
using System.Security.Cryptography;

namespace Renci.SshNet
{
	public class HashInfo
	{
		public int KeySize
		{
			get;
			private set;
		}

		public Func<byte[], HashAlgorithm> HashAlgorithm
		{
			get;
			private set;
		}

		public HashInfo(int keySize, Func<byte[], HashAlgorithm> hash)
		{
			KeySize = keySize;
			Func<byte[], HashAlgorithm> func2 = HashAlgorithm = ((byte[] key) => hash(key.Take(KeySize / 8).ToArray()));
		}
	}
}
