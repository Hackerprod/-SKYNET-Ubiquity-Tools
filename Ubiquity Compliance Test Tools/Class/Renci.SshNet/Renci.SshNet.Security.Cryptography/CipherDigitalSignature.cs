using Renci.SshNet.Common;
using System;
using System.Linq;

namespace Renci.SshNet.Security.Cryptography
{
	public abstract class CipherDigitalSignature : DigitalSignature
	{
		private readonly AsymmetricCipher _cipher;

		private readonly ObjectIdentifier _oid;

		public CipherDigitalSignature(ObjectIdentifier oid, AsymmetricCipher cipher)
		{
			if (cipher == null)
			{
				throw new ArgumentNullException("cipher");
			}
			_cipher = cipher;
			_oid = oid;
		}

		public override bool Verify(byte[] input, byte[] signature)
		{
			byte[] second = _cipher.Decrypt(signature);
			byte[] hashData = Hash(input);
			byte[] first = DerEncode(hashData);
			return first.SequenceEqual(second);
		}

		public override byte[] Sign(byte[] input)
		{
			byte[] hashData = Hash(input);
			byte[] input2 = DerEncode(hashData);
			return _cipher.Encrypt(input2).TrimLeadingZero().ToArray();
		}

		protected abstract byte[] Hash(byte[] input);

		protected byte[] DerEncode(byte[] hashData)
		{
			DerData derData = new DerData();
			DerData derData2 = new DerData();
			derData2.Write(_oid);
			derData2.WriteNull();
			derData.Write(derData2);
			derData.Write(hashData);
			return derData.Encode();
		}
	}
}
