using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
	public class DsaDigitalSignature : DigitalSignature, IDisposable
	{
		private HashAlgorithm _hash;

		private readonly DsaKey _key;

		private bool _isDisposed;

		public DsaDigitalSignature(DsaKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			_key = key;
			_hash = new SHA1Hash();
		}

		public override bool Verify(byte[] input, byte[] signature)
		{
			byte[] source = _hash.ComputeHash(input);
			IEnumerable<byte> first = source.Reverse();
			byte[] second = new byte[1];
			BigInteger left = new BigInteger(first.Concat(second).ToArray());
			if (signature.Length != 40)
			{
				throw new InvalidOperationException("Invalid signature.");
			}
			byte[] array = new byte[21];
			byte[] array2 = new byte[21];
			int num = 0;
			int num2 = 20;
			while (num < 20)
			{
				array[num] = signature[num2 - 1];
				array2[num] = signature[num2 + 20 - 1];
				num++;
				num2--;
			}
			BigInteger bigInteger = new BigInteger(array);
			BigInteger bigInteger2 = new BigInteger(array2);
			if (bigInteger <= 0L || bigInteger >= _key.Q)
			{
				return false;
			}
			if (bigInteger2 <= 0L || bigInteger2 >= _key.Q)
			{
				return false;
			}
			BigInteger right = BigInteger.ModInverse(bigInteger2, _key.Q);
			BigInteger exponent = left * right % _key.Q;
			BigInteger exponent2 = bigInteger * right % _key.Q;
			exponent = BigInteger.ModPow(_key.G, exponent, _key.P);
			exponent2 = BigInteger.ModPow(_key.Y, exponent2, _key.P);
			BigInteger left2 = exponent * exponent2 % _key.P % _key.Q;
			return left2 == bigInteger;
		}

		public override byte[] Sign(byte[] input)
		{
			byte[] source = _hash.ComputeHash(input);
			IEnumerable<byte> first = source.Reverse();
			byte[] second = new byte[1];
			BigInteger left = new BigInteger(first.Concat(second).ToArray());
			BigInteger right;
			BigInteger bigInteger2;
			do
			{
				BigInteger bigInteger = BigInteger.Zero;
				do
				{
					int bitLength = _key.Q.BitLength;
					if (_key.Q < BigInteger.Zero)
					{
						throw new SshException("Invalid DSA key.");
					}
					while (bigInteger <= 0L || bigInteger >= _key.Q)
					{
						bigInteger = BigInteger.Random(bitLength);
					}
					right = BigInteger.ModPow(_key.G, bigInteger, _key.P) % _key.Q;
				}
				while (right.IsZero);
				bigInteger = BigInteger.ModInverse(bigInteger, _key.Q) * (left + _key.X * right);
				bigInteger2 = bigInteger % _key.Q;
			}
			while (bigInteger2.IsZero);
			byte[] array = new byte[40];
			byte[] array2 = right.ToByteArray().Reverse().TrimLeadingZero()
				.ToArray();
			Array.Copy(array2, 0, array, 20 - array2.Length, array2.Length);
			byte[] array3 = bigInteger2.ToByteArray().Reverse().TrimLeadingZero()
				.ToArray();
			Array.Copy(array3, 0, array, 40 - array3.Length, array3.Length);
			return array;
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

		~DsaDigitalSignature()
		{
			Dispose(disposing: false);
		}
	}
}
