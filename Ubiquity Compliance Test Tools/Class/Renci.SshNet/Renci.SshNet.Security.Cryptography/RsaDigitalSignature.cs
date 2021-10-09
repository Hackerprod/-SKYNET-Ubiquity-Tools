using Renci.SshNet.Common;
using Renci.SshNet.Security.Cryptography.Ciphers;
using System;
using System.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
	public class RsaDigitalSignature : CipherDigitalSignature, IDisposable
	{
		private HashAlgorithm _hash;

		private bool _isDisposed;

		public RsaDigitalSignature(RsaKey rsaKey)
			: base(new ObjectIdentifier(1uL, 3uL, 14uL, 3uL, 2uL, 26uL), new RsaCipher(rsaKey))
		{
			_hash = new SHA1Hash();
		}

		protected override byte[] Hash(byte[] input)
		{
			return _hash.ComputeHash(input);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing && _hash != null)
				{
					_hash.Clear();
					_hash = null;
				}
				_isDisposed = true;
			}
		}

		~RsaDigitalSignature()
		{
			Dispose(disposing: false);
		}
	}
}
