using System.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
	public class SHA256Hash : HashAlgorithm
	{
		private const int DIGEST_SIZE = 32;

		private uint H1;

		private uint H2;

		private uint H3;

		private uint H4;

		private uint H5;

		private uint H6;

		private uint H7;

		private uint H8;

		private readonly uint[] X = new uint[64];

		private int _offset;

		private readonly byte[] _buffer;

		private int _bufferOffset;

		private long _byteCount;

		private static readonly uint[] K = new uint[64]
		{
			1116352408u,
			1899447441u,
			3049323471u,
			3921009573u,
			961987163u,
			1508970993u,
			2453635748u,
			2870763221u,
			3624381080u,
			310598401u,
			607225278u,
			1426881987u,
			1925078388u,
			2162078206u,
			2614888103u,
			3248222580u,
			3835390401u,
			4022224774u,
			264347078u,
			604807628u,
			770255983u,
			1249150122u,
			1555081692u,
			1996064986u,
			2554220882u,
			2821834349u,
			2952996808u,
			3210313671u,
			3336571891u,
			3584528711u,
			113926993u,
			338241895u,
			666307205u,
			773529912u,
			1294757372u,
			1396182291u,
			1695183700u,
			1986661051u,
			2177026350u,
			2456956037u,
			2730485921u,
			2820302411u,
			3259730800u,
			3345764771u,
			3516065817u,
			3600352804u,
			4094571909u,
			275423344u,
			430227734u,
			506948616u,
			659060556u,
			883997877u,
			958139571u,
			1322822218u,
			1537002063u,
			1747873779u,
			1955562222u,
			2024104815u,
			2227730452u,
			2361852424u,
			2428436474u,
			2756734187u,
			3204031479u,
			3329325298u
		};

		public override int HashSize => 256;

		public override int InputBlockSize => 64;

		public override int OutputBlockSize => 64;

		public override bool CanReuseTransform => true;

		public override bool CanTransformMultipleBlocks => true;

		public SHA256Hash()
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
			byte[] array = new byte[32];
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
			X[14] = (uint)((ulong)num >> 32);
			X[15] = (uint)num;
			ProcessBlock();
			UInt32_To_BE(H1, array, 0);
			UInt32_To_BE(H2, array, 4);
			UInt32_To_BE(H3, array, 8);
			UInt32_To_BE(H4, array, 12);
			UInt32_To_BE(H5, array, 16);
			UInt32_To_BE(H6, array, 20);
			UInt32_To_BE(H7, array, 24);
			UInt32_To_BE(H8, array, 28);
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
			for (int i = 0; i < _buffer.Length; i++)
			{
				_buffer[i] = 0;
			}
			H1 = 1779033703u;
			H2 = 3144134277u;
			H3 = 1013904242u;
			H4 = 2773480762u;
			H5 = 1359893119u;
			H6 = 2600822924u;
			H7 = 528734635u;
			H8 = 1541459225u;
			_offset = 0;
			for (int j = 0; j < X.Length; j++)
			{
				X[j] = 0u;
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

		private static uint BE_To_UInt32(byte[] bs, int off)
		{
			uint num = (uint)(bs[off] << 24);
			num = (uint)((int)num | (bs[++off] << 16));
			num = (uint)((int)num | (bs[++off] << 8));
			return num | bs[++off];
		}

		private static void UInt32_To_BE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 24);
			bs[++off] = (byte)(n >> 16);
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)n;
		}

		private void ProcessWord(byte[] input, int inOff)
		{
			X[_offset] = BE_To_UInt32(input, inOff);
			if (++_offset == 16)
			{
				ProcessBlock();
			}
		}

		private void ProcessLength(long bitLength)
		{
			if (_offset > 14)
			{
				ProcessBlock();
			}
			X[14] = (uint)((ulong)bitLength >> 32);
			X[15] = (uint)bitLength;
		}

		private void ProcessBlock()
		{
			for (int i = 16; i <= 63; i++)
			{
				X[i] = Theta1(X[i - 2]) + X[i - 7] + Theta0(X[i - 15]) + X[i - 16];
			}
			uint num = H1;
			uint num2 = H2;
			uint num3 = H3;
			uint num4 = H4;
			uint num5 = H5;
			uint num6 = H6;
			uint num7 = H7;
			uint num8 = H8;
			int num9 = 0;
			for (int j = 0; j < 8; j++)
			{
				num8 += Sum1Ch(num5, num6, num7) + K[num9] + X[num9];
				num4 += num8;
				num8 += Sum0Maj(num, num2, num3);
				num9++;
				num7 += Sum1Ch(num4, num5, num6) + K[num9] + X[num9];
				num3 += num7;
				num7 += Sum0Maj(num8, num, num2);
				num9++;
				num6 += Sum1Ch(num3, num4, num5) + K[num9] + X[num9];
				num2 += num6;
				num6 += Sum0Maj(num7, num8, num);
				num9++;
				num5 += Sum1Ch(num2, num3, num4) + K[num9] + X[num9];
				num += num5;
				num5 += Sum0Maj(num6, num7, num8);
				num9++;
				num4 += Sum1Ch(num, num2, num3) + K[num9] + X[num9];
				num8 += num4;
				num4 += Sum0Maj(num5, num6, num7);
				num9++;
				num3 += Sum1Ch(num8, num, num2) + K[num9] + X[num9];
				num7 += num3;
				num3 += Sum0Maj(num4, num5, num6);
				num9++;
				num2 += Sum1Ch(num7, num8, num) + K[num9] + X[num9];
				num6 += num2;
				num2 += Sum0Maj(num3, num4, num5);
				num9++;
				num += Sum1Ch(num6, num7, num8) + K[num9] + X[num9];
				num5 += num;
				num += Sum0Maj(num2, num3, num4);
				num9++;
			}
			H1 += num;
			H2 += num2;
			H3 += num3;
			H4 += num4;
			H5 += num5;
			H6 += num6;
			H7 += num7;
			H8 += num8;
			_offset = 0;
			for (int k = 0; k < X.Length; k++)
			{
				X[k] = 0u;
			}
		}

		private static uint Sum1Ch(uint x, uint y, uint z)
		{
			return (((x >> 6) | (x << 26)) ^ ((x >> 11) | (x << 21)) ^ ((x >> 25) | (x << 7))) + ((x & y) ^ (~x & z));
		}

		private static uint Sum0Maj(uint x, uint y, uint z)
		{
			return (((x >> 2) | (x << 30)) ^ ((x >> 13) | (x << 19)) ^ ((x >> 22) | (x << 10))) + ((x & y) ^ (x & z) ^ (y & z));
		}

		private static uint Theta0(uint x)
		{
			return ((x >> 7) | (x << 25)) ^ ((x >> 18) | (x << 14)) ^ (x >> 3);
		}

		private static uint Theta1(uint x)
		{
			return ((x >> 17) | (x << 15)) ^ ((x >> 19) | (x << 13)) ^ (x >> 10);
		}
	}
}
