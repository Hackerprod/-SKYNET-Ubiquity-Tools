using Renci.SshNet.Common;
using Renci.SshNet.Security.Cryptography;
using System;
using System.Collections.Generic;

namespace Renci.SshNet.Security
{
	public abstract class Key
	{
		protected BigInteger[] _privateKey;

		protected abstract DigitalSignature DigitalSignature
		{
			get;
		}

		public abstract BigInteger[] Public
		{
			get;
			set;
		}

		public abstract int KeyLength
		{
			get;
		}

		public Key(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			DerData derData = new DerData(data);
			derData.ReadBigInteger();
			List<BigInteger> list = new List<BigInteger>();
			while (!derData.IsEndOfData)
			{
				list.Add(derData.ReadBigInteger());
			}
			_privateKey = list.ToArray();
		}

		public Key()
		{
		}

		public byte[] Sign(byte[] data)
		{
			return DigitalSignature.Sign(data);
		}

		public bool VerifySignature(byte[] data, byte[] signature)
		{
			return DigitalSignature.Verify(data, signature);
		}
	}
}
