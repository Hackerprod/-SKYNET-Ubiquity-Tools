using System.Linq;
using System.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
	public class HMac<T> : KeyedHashAlgorithm where T : HashAlgorithm, new()
	{
		private HashAlgorithm _hash;

		private byte[] _innerPadding;

		private byte[] _outerPadding;

		protected int BlockSize => _hash.InputBlockSize;

		public override byte[] Key
		{
			get
			{
				return (byte[])KeyValue.Clone();
			}
			set
			{
				SetKey(value);
			}
		}

		private HMac()
		{
			_hash = new T();
			HashSizeValue = _hash.HashSize;
		}

		public HMac(byte[] key, int hashSizeValue)
			: this(key)
		{
			HashSizeValue = hashSizeValue;
		}

		public HMac(byte[] key)
			: this()
		{
			KeyValue = key;
			InternalInitialize();
		}

		public override void Initialize()
		{
			InternalInitialize();
		}

		protected override void HashCore(byte[] rgb, int ib, int cb)
		{
			_hash.TransformBlock(rgb, ib, cb, rgb, ib);
		}

		protected override byte[] HashFinal()
		{
			_hash.TransformFinalBlock(new byte[0], 0, 0);
			byte[] hash = _hash.Hash;
			_hash.TransformBlock(_outerPadding, 0, BlockSize, _outerPadding, 0);
			_hash.TransformFinalBlock(hash, 0, hash.Length);
			return _hash.Hash.Take(HashSize / 8).ToArray();
		}

		private void InternalInitialize()
		{
			SetKey(KeyValue);
		}

		private void SetKey(byte[] value)
		{
			_hash.Initialize();
			if (value.Length > BlockSize)
			{
				KeyValue = _hash.ComputeHash(value);
			}
			else
			{
				KeyValue = (byte[])value.Clone();
			}
			_innerPadding = new byte[BlockSize];
			_outerPadding = new byte[BlockSize];
			for (int i = 0; i < KeyValue.Length; i++)
			{
				_innerPadding[i] = (byte)(0x36 ^ KeyValue[i]);
				_outerPadding[i] = (byte)(0x5C ^ KeyValue[i]);
			}
			for (int j = KeyValue.Length; j < BlockSize; j++)
			{
				_innerPadding[j] = 54;
				_outerPadding[j] = 92;
			}
			_hash.TransformBlock(_innerPadding, 0, BlockSize, _innerPadding, 0);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (_hash != null)
			{
				_hash.Clear();
				_hash = null;
			}
		}
	}
}
