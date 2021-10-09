using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Renci.SshNet.Security.Cryptography.Ciphers
{
	public class RsaCipher : AsymmetricCipher
	{
		private readonly bool _isPrivate;

		private readonly RsaKey _key;

		public RsaCipher(RsaKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			_key = key;
			_isPrivate = !_key.D.IsZero;
		}

		public override byte[] Encrypt(byte[] data)
		{
			int bitLength = _key.Modulus.BitLength;
			byte[] array = new byte[bitLength / 8 + ((bitLength % 8 > 0) ? 1 : 0) - 1];
			array[0] = 1;
			for (int i = 1; i < array.Length - data.Length - 1; i++)
			{
				array[i] = byte.MaxValue;
			}
			Buffer.BlockCopy(data, 0, array, array.Length - data.Length, data.Length);
			return Transform(array);
		}

		public override byte[] Decrypt(byte[] data)
		{
			byte[] array = Transform(data);
			if (array[0] != 1 && array[0] != 2)
			{
				throw new NotSupportedException("Only block type 01 or 02 are supported.");
			}
			int i;
			for (i = 1; i < array.Length && array[i] != 0; i++)
			{
			}
			i++;
			byte[] array2 = new byte[array.Length - i];
			Buffer.BlockCopy(array, i, array2, 0, array2.Length);
			return array2;
		}

		private byte[] Transform(byte[] data)
		{
			List<byte> list = new List<byte>(data.Reverse());
			list.Add(0);
			BigInteger bigInteger = new BigInteger(list.ToArray());
			BigInteger bigInteger4;
			if (_isPrivate)
			{
				BigInteger bigInteger2 = BigInteger.One;
				BigInteger bigInteger3 = _key.Modulus - 1;
				int bitLength = _key.Modulus.BitLength;
				if (bigInteger3 < BigInteger.One)
				{
					throw new SshException("Invalid RSA key.");
				}
				while (bigInteger2 <= BigInteger.One || bigInteger2 >= bigInteger3)
				{
					bigInteger2 = BigInteger.Random(bitLength);
				}
				BigInteger dividend = BigInteger.PositiveMod(BigInteger.ModPow(bigInteger2, _key.Exponent, _key.Modulus) * bigInteger, _key.Modulus);
				BigInteger left = BigInteger.ModPow(dividend % _key.P, _key.DP, _key.P);
				BigInteger right = BigInteger.ModPow(dividend % _key.Q, _key.DQ, _key.Q);
				BigInteger left2 = BigInteger.PositiveMod((left - right) * _key.InverseQ, _key.P);
				BigInteger left3 = left2 * _key.Q + right;
				BigInteger right2 = BigInteger.ModInverse(bigInteger2, _key.Modulus);
				bigInteger4 = BigInteger.PositiveMod(left3 * right2, _key.Modulus);
			}
			else
			{
				bigInteger4 = BigInteger.ModPow(bigInteger, _key.Exponent, _key.Modulus);
			}
			return bigInteger4.ToByteArray().Reverse().ToArray();
		}
	}
}
