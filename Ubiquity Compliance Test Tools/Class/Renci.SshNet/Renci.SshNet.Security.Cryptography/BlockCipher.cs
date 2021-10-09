using Renci.SshNet.Security.Cryptography.Ciphers;
using System;

namespace Renci.SshNet.Security.Cryptography
{
	public abstract class BlockCipher : SymmetricCipher
	{
		private readonly CipherMode _mode;

		private readonly CipherPadding _padding;

		protected readonly byte _blockSize;

		public override byte MinimumSize => BlockSize;

		public byte BlockSize => _blockSize;

		protected BlockCipher(byte[] key, byte blockSize, CipherMode mode, CipherPadding padding)
			: base(key)
		{
			_blockSize = blockSize;
			_mode = mode;
			_padding = padding;
			if (_mode != null)
			{
				_mode.Init(this);
			}
		}

		public override byte[] Encrypt(byte[] data)
		{
			byte[] array = new byte[data.Length];
			if (data.Length % (int)_blockSize > 0)
			{
				if (_padding == null)
				{
					throw new ArgumentException("data");
				}
				data = _padding.Pad(_blockSize, data);
			}
			int num = 0;
			for (int i = 0; i < data.Length / (int)_blockSize; i++)
			{
				num = ((_mode != null) ? (num + _mode.EncryptBlock(data, i * _blockSize, _blockSize, array, i * _blockSize)) : (num + EncryptBlock(data, i * _blockSize, _blockSize, array, i * _blockSize)));
			}
			if (num < data.Length)
			{
				throw new InvalidOperationException("Encryption error.");
			}
			return array;
		}

		public override byte[] Decrypt(byte[] data)
		{
			if (data.Length % (int)_blockSize > 0)
			{
				if (_padding == null)
				{
					throw new ArgumentException("data");
				}
				data = _padding.Pad(_blockSize, data);
			}
			byte[] array = new byte[data.Length];
			int num = 0;
			for (int i = 0; i < data.Length / (int)_blockSize; i++)
			{
				num = ((_mode != null) ? (num + _mode.DecryptBlock(data, i * _blockSize, _blockSize, array, i * _blockSize)) : (num + DecryptBlock(data, i * _blockSize, _blockSize, array, i * _blockSize)));
			}
			if (num < data.Length)
			{
				throw new InvalidOperationException("Encryption error.");
			}
			return array;
		}
	}
}
