using System.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
	public sealed class SHA1Hash : HashAlgorithm
	{
		private const int DIGEST_SIZE = 20;

		private const uint Y1 = 1518500249u;

		private const uint Y2 = 1859775393u;

		private const uint Y3 = 2400959708u;

		private const uint Y4 = 3395469782u;

		private uint H1;

		private uint H2;

		private uint H3;

		private uint H4;

		private uint H5;

		private readonly uint[] _hashValue = new uint[80];

		private int _offset;

		private readonly byte[] _buffer;

		private int _bufferOffset;

		private long _byteCount;

		public override int HashSize => 160;

		public override int InputBlockSize => 64;

		public override int OutputBlockSize => 64;

		public override bool CanReuseTransform => true;

		public override bool CanTransformMultipleBlocks => true;

		public SHA1Hash()
		{
			_buffer = new byte[4];
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
			byte[] array = new byte[20];
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
			_hashValue[14] = (uint)((ulong)num >> 32);
			_hashValue[15] = (uint)num;
			ProcessBlock();
			UInt32ToBigEndian(H1, array, 0);
			UInt32ToBigEndian(H2, array, 4);
			UInt32ToBigEndian(H3, array, 8);
			UInt32ToBigEndian(H4, array, 12);
			UInt32ToBigEndian(H5, array, 16);
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
			H1 = 1732584193u;
			H2 = 4023233417u;
			H3 = 2562383102u;
			H4 = 271733878u;
			H5 = 3285377520u;
			_offset = 0;
			for (int j = 0; j != _hashValue.Length; j++)
			{
				_hashValue[j] = 0u;
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
			_hashValue[_offset] = BigEndianToUInt32(input, inOff);
			if (++_offset == 16)
			{
				ProcessBlock();
			}
		}

		private static uint F(uint u, uint v, uint w)
		{
			return (u & v) | (~u & w);
		}

		private static uint H(uint u, uint v, uint w)
		{
			return u ^ v ^ w;
		}

		private static uint G(uint u, uint v, uint w)
		{
			return (u & v) | (u & w) | (v & w);
		}

		private void ProcessBlock()
		{
			for (int i = 16; i < 80; i++)
			{
				uint num = _hashValue[i - 3] ^ _hashValue[i - 8] ^ _hashValue[i - 14] ^ _hashValue[i - 16];
				_hashValue[i] = ((num << 1) | (num >> 31));
			}
			uint h = H1;
			uint h2 = H2;
			uint h3 = H3;
			uint h4 = H4;
			uint h5 = H5;
			int num2 = 0;
			h5 += ((h << 5) | (h >> 27)) + F(h2, h3, h4) + _hashValue[num2++] + 1518500249;
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 += ((h5 << 5) | (h5 >> 27)) + F(h, h2, h3) + _hashValue[num2++] + 1518500249;
			h = ((h << 30) | (h >> 2));
			h3 += ((h4 << 5) | (h4 >> 27)) + F(h5, h, h2) + _hashValue[num2++] + 1518500249;
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 += ((h3 << 5) | (h3 >> 27)) + F(h4, h5, h) + _hashValue[num2++] + 1518500249;
			h4 = ((h4 << 30) | (h4 >> 2));
			h += ((h2 << 5) | (h2 >> 27)) + F(h3, h4, h5) + _hashValue[num2++] + 1518500249;
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 += ((h << 5) | (h >> 27)) + F(h2, h3, h4) + _hashValue[num2++] + 1518500249;
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 += ((h5 << 5) | (h5 >> 27)) + F(h, h2, h3) + _hashValue[num2++] + 1518500249;
			h = ((h << 30) | (h >> 2));
			h3 += ((h4 << 5) | (h4 >> 27)) + F(h5, h, h2) + _hashValue[num2++] + 1518500249;
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 += ((h3 << 5) | (h3 >> 27)) + F(h4, h5, h) + _hashValue[num2++] + 1518500249;
			h4 = ((h4 << 30) | (h4 >> 2));
			h += ((h2 << 5) | (h2 >> 27)) + F(h3, h4, h5) + _hashValue[num2++] + 1518500249;
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 += ((h << 5) | (h >> 27)) + F(h2, h3, h4) + _hashValue[num2++] + 1518500249;
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 += ((h5 << 5) | (h5 >> 27)) + F(h, h2, h3) + _hashValue[num2++] + 1518500249;
			h = ((h << 30) | (h >> 2));
			h3 += ((h4 << 5) | (h4 >> 27)) + F(h5, h, h2) + _hashValue[num2++] + 1518500249;
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 += ((h3 << 5) | (h3 >> 27)) + F(h4, h5, h) + _hashValue[num2++] + 1518500249;
			h4 = ((h4 << 30) | (h4 >> 2));
			h += ((h2 << 5) | (h2 >> 27)) + F(h3, h4, h5) + _hashValue[num2++] + 1518500249;
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 += ((h << 5) | (h >> 27)) + F(h2, h3, h4) + _hashValue[num2++] + 1518500249;
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 += ((h5 << 5) | (h5 >> 27)) + F(h, h2, h3) + _hashValue[num2++] + 1518500249;
			h = ((h << 30) | (h >> 2));
			h3 += ((h4 << 5) | (h4 >> 27)) + F(h5, h, h2) + _hashValue[num2++] + 1518500249;
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 += ((h3 << 5) | (h3 >> 27)) + F(h4, h5, h) + _hashValue[num2++] + 1518500249;
			h4 = ((h4 << 30) | (h4 >> 2));
			h += ((h2 << 5) | (h2 >> 27)) + F(h3, h4, h5) + _hashValue[num2++] + 1518500249;
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 += ((h << 5) | (h >> 27)) + H(h2, h3, h4) + _hashValue[num2++] + 1859775393;
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 += ((h5 << 5) | (h5 >> 27)) + H(h, h2, h3) + _hashValue[num2++] + 1859775393;
			h = ((h << 30) | (h >> 2));
			h3 += ((h4 << 5) | (h4 >> 27)) + H(h5, h, h2) + _hashValue[num2++] + 1859775393;
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 += ((h3 << 5) | (h3 >> 27)) + H(h4, h5, h) + _hashValue[num2++] + 1859775393;
			h4 = ((h4 << 30) | (h4 >> 2));
			h += ((h2 << 5) | (h2 >> 27)) + H(h3, h4, h5) + _hashValue[num2++] + 1859775393;
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 += ((h << 5) | (h >> 27)) + H(h2, h3, h4) + _hashValue[num2++] + 1859775393;
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 += ((h5 << 5) | (h5 >> 27)) + H(h, h2, h3) + _hashValue[num2++] + 1859775393;
			h = ((h << 30) | (h >> 2));
			h3 += ((h4 << 5) | (h4 >> 27)) + H(h5, h, h2) + _hashValue[num2++] + 1859775393;
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 += ((h3 << 5) | (h3 >> 27)) + H(h4, h5, h) + _hashValue[num2++] + 1859775393;
			h4 = ((h4 << 30) | (h4 >> 2));
			h += ((h2 << 5) | (h2 >> 27)) + H(h3, h4, h5) + _hashValue[num2++] + 1859775393;
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 += ((h << 5) | (h >> 27)) + H(h2, h3, h4) + _hashValue[num2++] + 1859775393;
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 += ((h5 << 5) | (h5 >> 27)) + H(h, h2, h3) + _hashValue[num2++] + 1859775393;
			h = ((h << 30) | (h >> 2));
			h3 += ((h4 << 5) | (h4 >> 27)) + H(h5, h, h2) + _hashValue[num2++] + 1859775393;
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 += ((h3 << 5) | (h3 >> 27)) + H(h4, h5, h) + _hashValue[num2++] + 1859775393;
			h4 = ((h4 << 30) | (h4 >> 2));
			h += ((h2 << 5) | (h2 >> 27)) + H(h3, h4, h5) + _hashValue[num2++] + 1859775393;
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 += ((h << 5) | (h >> 27)) + H(h2, h3, h4) + _hashValue[num2++] + 1859775393;
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 += ((h5 << 5) | (h5 >> 27)) + H(h, h2, h3) + _hashValue[num2++] + 1859775393;
			h = ((h << 30) | (h >> 2));
			h3 += ((h4 << 5) | (h4 >> 27)) + H(h5, h, h2) + _hashValue[num2++] + 1859775393;
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 += ((h3 << 5) | (h3 >> 27)) + H(h4, h5, h) + _hashValue[num2++] + 1859775393;
			h4 = ((h4 << 30) | (h4 >> 2));
			h += ((h2 << 5) | (h2 >> 27)) + H(h3, h4, h5) + _hashValue[num2++] + 1859775393;
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 = (uint)((int)h5 + ((int)(((h << 5) | (h >> 27)) + G(h2, h3, h4) + _hashValue[num2++]) + -1894007588));
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 = (uint)((int)h4 + ((int)(((h5 << 5) | (h5 >> 27)) + G(h, h2, h3) + _hashValue[num2++]) + -1894007588));
			h = ((h << 30) | (h >> 2));
			h3 = (uint)((int)h3 + ((int)(((h4 << 5) | (h4 >> 27)) + G(h5, h, h2) + _hashValue[num2++]) + -1894007588));
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 = (uint)((int)h2 + ((int)(((h3 << 5) | (h3 >> 27)) + G(h4, h5, h) + _hashValue[num2++]) + -1894007588));
			h4 = ((h4 << 30) | (h4 >> 2));
			h = (uint)((int)h + ((int)(((h2 << 5) | (h2 >> 27)) + G(h3, h4, h5) + _hashValue[num2++]) + -1894007588));
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 = (uint)((int)h5 + ((int)(((h << 5) | (h >> 27)) + G(h2, h3, h4) + _hashValue[num2++]) + -1894007588));
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 = (uint)((int)h4 + ((int)(((h5 << 5) | (h5 >> 27)) + G(h, h2, h3) + _hashValue[num2++]) + -1894007588));
			h = ((h << 30) | (h >> 2));
			h3 = (uint)((int)h3 + ((int)(((h4 << 5) | (h4 >> 27)) + G(h5, h, h2) + _hashValue[num2++]) + -1894007588));
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 = (uint)((int)h2 + ((int)(((h3 << 5) | (h3 >> 27)) + G(h4, h5, h) + _hashValue[num2++]) + -1894007588));
			h4 = ((h4 << 30) | (h4 >> 2));
			h = (uint)((int)h + ((int)(((h2 << 5) | (h2 >> 27)) + G(h3, h4, h5) + _hashValue[num2++]) + -1894007588));
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 = (uint)((int)h5 + ((int)(((h << 5) | (h >> 27)) + G(h2, h3, h4) + _hashValue[num2++]) + -1894007588));
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 = (uint)((int)h4 + ((int)(((h5 << 5) | (h5 >> 27)) + G(h, h2, h3) + _hashValue[num2++]) + -1894007588));
			h = ((h << 30) | (h >> 2));
			h3 = (uint)((int)h3 + ((int)(((h4 << 5) | (h4 >> 27)) + G(h5, h, h2) + _hashValue[num2++]) + -1894007588));
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 = (uint)((int)h2 + ((int)(((h3 << 5) | (h3 >> 27)) + G(h4, h5, h) + _hashValue[num2++]) + -1894007588));
			h4 = ((h4 << 30) | (h4 >> 2));
			h = (uint)((int)h + ((int)(((h2 << 5) | (h2 >> 27)) + G(h3, h4, h5) + _hashValue[num2++]) + -1894007588));
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 = (uint)((int)h5 + ((int)(((h << 5) | (h >> 27)) + G(h2, h3, h4) + _hashValue[num2++]) + -1894007588));
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 = (uint)((int)h4 + ((int)(((h5 << 5) | (h5 >> 27)) + G(h, h2, h3) + _hashValue[num2++]) + -1894007588));
			h = ((h << 30) | (h >> 2));
			h3 = (uint)((int)h3 + ((int)(((h4 << 5) | (h4 >> 27)) + G(h5, h, h2) + _hashValue[num2++]) + -1894007588));
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 = (uint)((int)h2 + ((int)(((h3 << 5) | (h3 >> 27)) + G(h4, h5, h) + _hashValue[num2++]) + -1894007588));
			h4 = ((h4 << 30) | (h4 >> 2));
			h = (uint)((int)h + ((int)(((h2 << 5) | (h2 >> 27)) + G(h3, h4, h5) + _hashValue[num2++]) + -1894007588));
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 = (uint)((int)h5 + ((int)(((h << 5) | (h >> 27)) + H(h2, h3, h4) + _hashValue[num2++]) + -899497514));
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 = (uint)((int)h4 + ((int)(((h5 << 5) | (h5 >> 27)) + H(h, h2, h3) + _hashValue[num2++]) + -899497514));
			h = ((h << 30) | (h >> 2));
			h3 = (uint)((int)h3 + ((int)(((h4 << 5) | (h4 >> 27)) + H(h5, h, h2) + _hashValue[num2++]) + -899497514));
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 = (uint)((int)h2 + ((int)(((h3 << 5) | (h3 >> 27)) + H(h4, h5, h) + _hashValue[num2++]) + -899497514));
			h4 = ((h4 << 30) | (h4 >> 2));
			h = (uint)((int)h + ((int)(((h2 << 5) | (h2 >> 27)) + H(h3, h4, h5) + _hashValue[num2++]) + -899497514));
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 = (uint)((int)h5 + ((int)(((h << 5) | (h >> 27)) + H(h2, h3, h4) + _hashValue[num2++]) + -899497514));
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 = (uint)((int)h4 + ((int)(((h5 << 5) | (h5 >> 27)) + H(h, h2, h3) + _hashValue[num2++]) + -899497514));
			h = ((h << 30) | (h >> 2));
			h3 = (uint)((int)h3 + ((int)(((h4 << 5) | (h4 >> 27)) + H(h5, h, h2) + _hashValue[num2++]) + -899497514));
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 = (uint)((int)h2 + ((int)(((h3 << 5) | (h3 >> 27)) + H(h4, h5, h) + _hashValue[num2++]) + -899497514));
			h4 = ((h4 << 30) | (h4 >> 2));
			h = (uint)((int)h + ((int)(((h2 << 5) | (h2 >> 27)) + H(h3, h4, h5) + _hashValue[num2++]) + -899497514));
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 = (uint)((int)h5 + ((int)(((h << 5) | (h >> 27)) + H(h2, h3, h4) + _hashValue[num2++]) + -899497514));
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 = (uint)((int)h4 + ((int)(((h5 << 5) | (h5 >> 27)) + H(h, h2, h3) + _hashValue[num2++]) + -899497514));
			h = ((h << 30) | (h >> 2));
			h3 = (uint)((int)h3 + ((int)(((h4 << 5) | (h4 >> 27)) + H(h5, h, h2) + _hashValue[num2++]) + -899497514));
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 = (uint)((int)h2 + ((int)(((h3 << 5) | (h3 >> 27)) + H(h4, h5, h) + _hashValue[num2++]) + -899497514));
			h4 = ((h4 << 30) | (h4 >> 2));
			h = (uint)((int)h + ((int)(((h2 << 5) | (h2 >> 27)) + H(h3, h4, h5) + _hashValue[num2++]) + -899497514));
			h3 = ((h3 << 30) | (h3 >> 2));
			h5 = (uint)((int)h5 + ((int)(((h << 5) | (h >> 27)) + H(h2, h3, h4) + _hashValue[num2++]) + -899497514));
			h2 = ((h2 << 30) | (h2 >> 2));
			h4 = (uint)((int)h4 + ((int)(((h5 << 5) | (h5 >> 27)) + H(h, h2, h3) + _hashValue[num2++]) + -899497514));
			h = ((h << 30) | (h >> 2));
			h3 = (uint)((int)h3 + ((int)(((h4 << 5) | (h4 >> 27)) + H(h5, h, h2) + _hashValue[num2++]) + -899497514));
			h5 = ((h5 << 30) | (h5 >> 2));
			h2 = (uint)((int)h2 + ((int)(((h3 << 5) | (h3 >> 27)) + H(h4, h5, h) + _hashValue[num2++]) + -899497514));
			h4 = ((h4 << 30) | (h4 >> 2));
			h = (uint)((int)h + ((int)(((h2 << 5) | (h2 >> 27)) + H(h3, h4, h5) + _hashValue[num2++]) + -899497514));
			h3 = ((h3 << 30) | (h3 >> 2));
			H1 += h;
			H2 += h2;
			H3 += h3;
			H4 += h4;
			H5 += h5;
			_offset = 0;
			for (int j = 0; j < _hashValue.Length; j++)
			{
				_hashValue[j] = 0u;
			}
		}

		private static uint BigEndianToUInt32(byte[] bs, int off)
		{
			uint num = (uint)(bs[off] << 24);
			num = (uint)((int)num | (bs[++off] << 16));
			num = (uint)((int)num | (bs[++off] << 8));
			return num | bs[++off];
		}

		private static void UInt32ToBigEndian(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 24);
			bs[++off] = (byte)(n >> 16);
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)n;
		}
	}
}
