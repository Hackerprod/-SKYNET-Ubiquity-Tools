using System.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
	public sealed class MD5Hash : HashAlgorithm
	{
		private const int S11 = 7;

		private const int S12 = 12;

		private const int S13 = 17;

		private const int S14 = 22;

		private const int S21 = 5;

		private const int S22 = 9;

		private const int S23 = 14;

		private const int S24 = 20;

		private const int S31 = 4;

		private const int S32 = 11;

		private const int S33 = 16;

		private const int S34 = 23;

		private const int S41 = 6;

		private const int S42 = 10;

		private const int S43 = 15;

		private const int S44 = 21;

		private readonly byte[] _buffer = new byte[4];

		private int _bufferOffset;

		private long _byteCount;

		private int H1;

		private int H2;

		private int H3;

		private int H4;

		private readonly int[] _hashValue = new int[16];

		private int _offset;

		public override int HashSize => 128;

		public override int InputBlockSize => 64;

		public override int OutputBlockSize => 64;

		public override bool CanReuseTransform => true;

		public override bool CanTransformMultipleBlocks => true;

		public MD5Hash()
		{
			InternalInitialize();
		}

		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{
			while (_bufferOffset != 0 && cbSize > 0)
			{
				Update(array[ibStart]);
				ibStart++;
				cbSize--;
			}
			while (cbSize > _buffer.Length)
			{
				ProcessWord(array, ibStart);
				ibStart += _buffer.Length;
				cbSize -= _buffer.Length;
				_byteCount += _buffer.Length;
			}
			while (cbSize > 0)
			{
				Update(array[ibStart]);
				ibStart++;
				cbSize--;
			}
		}

		protected override byte[] HashFinal()
		{
			long num = _byteCount << 3;
			Update(128);
			while (_bufferOffset != 0)
			{
				Update(0);
			}
			if (_offset > 14)
			{
				ProcessBlock();
			}
			_hashValue[14] = (int)(num & uint.MaxValue);
			_hashValue[15] = (int)((ulong)num >> 32);
			ProcessBlock();
			byte[] array = new byte[16];
			UnpackWord(H1, array, 0);
			UnpackWord(H2, array, 4);
			UnpackWord(H3, array, 8);
			UnpackWord(H4, array, 12);
			Initialize();
			return array;
		}

		public override void Initialize()
		{
			InternalInitialize();
		}

		private void InternalInitialize()
		{
			_byteCount = 0L;
			_bufferOffset = 0;
			for (int i = 0; i < 4; i++)
			{
				_buffer[i] = 0;
			}
			H1 = 1732584193;
			H2 = -271733879;
			H3 = -1732584194;
			H4 = 271733878;
			_offset = 0;
			for (int j = 0; j != _hashValue.Length; j++)
			{
				_hashValue[j] = 0;
			}
		}

		private void Update(byte input)
		{
			_buffer[_bufferOffset++] = input;
			if (_bufferOffset == _buffer.Length)
			{
				ProcessWord(_buffer, 0);
				_bufferOffset = 0;
			}
			_byteCount++;
		}

		private void ProcessWord(byte[] input, int inOff)
		{
			_hashValue[_offset++] = ((input[inOff] & 0xFF) | ((input[inOff + 1] & 0xFF) << 8) | ((input[inOff + 2] & 0xFF) << 16) | ((input[inOff + 3] & 0xFF) << 24));
			if (_offset == 16)
			{
				ProcessBlock();
			}
		}

		private void UnpackWord(int word, byte[] outBytes, int outOff)
		{
			outBytes[outOff] = (byte)word;
			outBytes[outOff + 1] = (byte)((uint)word >> 8);
			outBytes[outOff + 2] = (byte)((uint)word >> 16);
			outBytes[outOff + 3] = (byte)((uint)word >> 24);
		}

		private static int RotateLeft(int x, int n)
		{
			return (x << n) | (int)((uint)x >> 32 - n);
		}

		private static int F(int u, int v, int w)
		{
			return (u & v) | (~u & w);
		}

		private static int G(int u, int v, int w)
		{
			return (u & w) | (v & ~w);
		}

		private static int H(int u, int v, int w)
		{
			return u ^ v ^ w;
		}

		private static int K(int u, int v, int w)
		{
			return v ^ (u | ~w);
		}

		private void ProcessBlock()
		{
			int h = H1;
			int h2 = H2;
			int h3 = H3;
			int h4 = H4;
			h = RotateLeft(h + F(h2, h3, h4) + _hashValue[0] + -680876936, 7) + h2;
			h4 = RotateLeft(h4 + F(h, h2, h3) + _hashValue[1] + -389564586, 12) + h;
			h3 = RotateLeft(h3 + F(h4, h, h2) + _hashValue[2] + 606105819, 17) + h4;
			h2 = RotateLeft(h2 + F(h3, h4, h) + _hashValue[3] + -1044525330, 22) + h3;
			h = RotateLeft(h + F(h2, h3, h4) + _hashValue[4] + -176418897, 7) + h2;
			h4 = RotateLeft(h4 + F(h, h2, h3) + _hashValue[5] + 1200080426, 12) + h;
			h3 = RotateLeft(h3 + F(h4, h, h2) + _hashValue[6] + -1473231341, 17) + h4;
			h2 = RotateLeft(h2 + F(h3, h4, h) + _hashValue[7] + -45705983, 22) + h3;
			h = RotateLeft(h + F(h2, h3, h4) + _hashValue[8] + 1770035416, 7) + h2;
			h4 = RotateLeft(h4 + F(h, h2, h3) + _hashValue[9] + -1958414417, 12) + h;
			h3 = RotateLeft(h3 + F(h4, h, h2) + _hashValue[10] + -42063, 17) + h4;
			h2 = RotateLeft(h2 + F(h3, h4, h) + _hashValue[11] + -1990404162, 22) + h3;
			h = RotateLeft(h + F(h2, h3, h4) + _hashValue[12] + 1804603682, 7) + h2;
			h4 = RotateLeft(h4 + F(h, h2, h3) + _hashValue[13] + -40341101, 12) + h;
			h3 = RotateLeft(h3 + F(h4, h, h2) + _hashValue[14] + -1502002290, 17) + h4;
			h2 = RotateLeft(h2 + F(h3, h4, h) + _hashValue[15] + 1236535329, 22) + h3;
			h = RotateLeft(h + G(h2, h3, h4) + _hashValue[1] + -165796510, 5) + h2;
			h4 = RotateLeft(h4 + G(h, h2, h3) + _hashValue[6] + -1069501632, 9) + h;
			h3 = RotateLeft(h3 + G(h4, h, h2) + _hashValue[11] + 643717713, 14) + h4;
			h2 = RotateLeft(h2 + G(h3, h4, h) + _hashValue[0] + -373897302, 20) + h3;
			h = RotateLeft(h + G(h2, h3, h4) + _hashValue[5] + -701558691, 5) + h2;
			h4 = RotateLeft(h4 + G(h, h2, h3) + _hashValue[10] + 38016083, 9) + h;
			h3 = RotateLeft(h3 + G(h4, h, h2) + _hashValue[15] + -660478335, 14) + h4;
			h2 = RotateLeft(h2 + G(h3, h4, h) + _hashValue[4] + -405537848, 20) + h3;
			h = RotateLeft(h + G(h2, h3, h4) + _hashValue[9] + 568446438, 5) + h2;
			h4 = RotateLeft(h4 + G(h, h2, h3) + _hashValue[14] + -1019803690, 9) + h;
			h3 = RotateLeft(h3 + G(h4, h, h2) + _hashValue[3] + -187363961, 14) + h4;
			h2 = RotateLeft(h2 + G(h3, h4, h) + _hashValue[8] + 1163531501, 20) + h3;
			h = RotateLeft(h + G(h2, h3, h4) + _hashValue[13] + -1444681467, 5) + h2;
			h4 = RotateLeft(h4 + G(h, h2, h3) + _hashValue[2] + -51403784, 9) + h;
			h3 = RotateLeft(h3 + G(h4, h, h2) + _hashValue[7] + 1735328473, 14) + h4;
			h2 = RotateLeft(h2 + G(h3, h4, h) + _hashValue[12] + -1926607734, 20) + h3;
			h = RotateLeft(h + H(h2, h3, h4) + _hashValue[5] + -378558, 4) + h2;
			h4 = RotateLeft(h4 + H(h, h2, h3) + _hashValue[8] + -2022574463, 11) + h;
			h3 = RotateLeft(h3 + H(h4, h, h2) + _hashValue[11] + 1839030562, 16) + h4;
			h2 = RotateLeft(h2 + H(h3, h4, h) + _hashValue[14] + -35309556, 23) + h3;
			h = RotateLeft(h + H(h2, h3, h4) + _hashValue[1] + -1530992060, 4) + h2;
			h4 = RotateLeft(h4 + H(h, h2, h3) + _hashValue[4] + 1272893353, 11) + h;
			h3 = RotateLeft(h3 + H(h4, h, h2) + _hashValue[7] + -155497632, 16) + h4;
			h2 = RotateLeft(h2 + H(h3, h4, h) + _hashValue[10] + -1094730640, 23) + h3;
			h = RotateLeft(h + H(h2, h3, h4) + _hashValue[13] + 681279174, 4) + h2;
			h4 = RotateLeft(h4 + H(h, h2, h3) + _hashValue[0] + -358537222, 11) + h;
			h3 = RotateLeft(h3 + H(h4, h, h2) + _hashValue[3] + -722521979, 16) + h4;
			h2 = RotateLeft(h2 + H(h3, h4, h) + _hashValue[6] + 76029189, 23) + h3;
			h = RotateLeft(h + H(h2, h3, h4) + _hashValue[9] + -640364487, 4) + h2;
			h4 = RotateLeft(h4 + H(h, h2, h3) + _hashValue[12] + -421815835, 11) + h;
			h3 = RotateLeft(h3 + H(h4, h, h2) + _hashValue[15] + 530742520, 16) + h4;
			h2 = RotateLeft(h2 + H(h3, h4, h) + _hashValue[2] + -995338651, 23) + h3;
			h = RotateLeft(h + K(h2, h3, h4) + _hashValue[0] + -198630844, 6) + h2;
			h4 = RotateLeft(h4 + K(h, h2, h3) + _hashValue[7] + 1126891415, 10) + h;
			h3 = RotateLeft(h3 + K(h4, h, h2) + _hashValue[14] + -1416354905, 15) + h4;
			h2 = RotateLeft(h2 + K(h3, h4, h) + _hashValue[5] + -57434055, 21) + h3;
			h = RotateLeft(h + K(h2, h3, h4) + _hashValue[12] + 1700485571, 6) + h2;
			h4 = RotateLeft(h4 + K(h, h2, h3) + _hashValue[3] + -1894986606, 10) + h;
			h3 = RotateLeft(h3 + K(h4, h, h2) + _hashValue[10] + -1051523, 15) + h4;
			h2 = RotateLeft(h2 + K(h3, h4, h) + _hashValue[1] + -2054922799, 21) + h3;
			h = RotateLeft(h + K(h2, h3, h4) + _hashValue[8] + 1873313359, 6) + h2;
			h4 = RotateLeft(h4 + K(h, h2, h3) + _hashValue[15] + -30611744, 10) + h;
			h3 = RotateLeft(h3 + K(h4, h, h2) + _hashValue[6] + -1560198380, 15) + h4;
			h2 = RotateLeft(h2 + K(h3, h4, h) + _hashValue[13] + 1309151649, 21) + h3;
			h = RotateLeft(h + K(h2, h3, h4) + _hashValue[4] + -145523070, 6) + h2;
			h4 = RotateLeft(h4 + K(h, h2, h3) + _hashValue[11] + -1120210379, 10) + h;
			h3 = RotateLeft(h3 + K(h4, h, h2) + _hashValue[2] + 718787259, 15) + h4;
			h2 = RotateLeft(h2 + K(h3, h4, h) + _hashValue[9] + -343485551, 21) + h3;
			H1 += h;
			H2 += h2;
			H3 += h3;
			H4 += h4;
			_offset = 0;
			for (int i = 0; i != _hashValue.Length; i++)
			{
				_hashValue[i] = 0;
			}
		}
	}
}
