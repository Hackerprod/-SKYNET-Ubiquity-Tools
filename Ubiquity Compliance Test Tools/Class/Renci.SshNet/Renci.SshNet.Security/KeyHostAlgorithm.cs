using Renci.SshNet.Common;
using System.Collections.Generic;

namespace Renci.SshNet.Security
{
	public class KeyHostAlgorithm : HostAlgorithm
	{
		private class SshKeyData : SshData
		{
			public BigInteger[] Keys
			{
				get;
				private set;
			}

			public string Name
			{
				get;
				private set;
			}

			public SshKeyData()
			{
			}

			public SshKeyData(string name, params BigInteger[] keys)
			{
				Name = name;
				Keys = keys;
			}

			protected override void LoadData()
			{
				Name = ReadString();
				List<BigInteger> list = new List<BigInteger>();
				while (!base.IsEndOfData)
				{
					list.Add(ReadBigInt());
				}
				Keys = list.ToArray();
			}

			protected override void SaveData()
			{
				Write(Name);
				BigInteger[] keys = Keys;
				foreach (BigInteger data in keys)
				{
					Write(data);
				}
			}
		}

		private class SignatureKeyData : SshData
		{
			public string AlgorithmName
			{
				get;
				private set;
			}

			public byte[] Signature
			{
				get;
				private set;
			}

			public SignatureKeyData()
			{
			}

			public SignatureKeyData(string name, byte[] signature)
			{
				AlgorithmName = name;
				Signature = signature;
			}

			protected override void LoadData()
			{
				AlgorithmName = ReadString();
				Signature = ReadBinaryString();
			}

			protected override void SaveData()
			{
				Write(AlgorithmName);
				WriteBinaryString(Signature);
			}
		}

		public Key Key
		{
			get;
			private set;
		}

		public override byte[] Data => new SshKeyData(base.Name, Key.Public).GetBytes();

		public KeyHostAlgorithm(string name, Key key)
			: base(name)
		{
			Key = key;
		}

		public KeyHostAlgorithm(string name, Key key, byte[] data)
			: base(name)
		{
			Key = key;
			SshKeyData sshKeyData = new SshKeyData();
			sshKeyData.Load(data);
			Key.Public = sshKeyData.Keys;
		}

		public override byte[] Sign(byte[] data)
		{
			return new SignatureKeyData(base.Name, Key.Sign(data)).GetBytes();
		}

		public override bool VerifySignature(byte[] data, byte[] signature)
		{
			SignatureKeyData signatureKeyData = new SignatureKeyData();
			signatureKeyData.Load(signature);
			return Key.VerifySignature(data, signatureKeyData.Signature);
		}
	}
}
