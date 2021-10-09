using System.Security.Cryptography;

namespace Renci.SshNet.Security.Cryptography
{
	public abstract class SHA2HashBase : HashAlgorithm
	{
		protected ulong H1;

		protected ulong H2;

		protected ulong H3;

		protected ulong H4;

		protected ulong H5;

		protected ulong H6;

		protected ulong H7;

		protected ulong H8;

		private readonly ulong[] X = new ulong[80];

		private int _offset;

		private readonly byte[] _buffer;

		private int _bufferOffset;

		private long _byteCount1;

		private long _byteCount2;

		private static readonly ulong[] K = new ulong[80]
		{
			4794697086780616226uL,
			8158064640168781261uL,
			13096744586834688815uL,
			16840607885511220156uL,
			4131703408338449720uL,
			6480981068601479193uL,
			10538285296894168987uL,
			12329834152419229976uL,
			15566598209576043074uL,
			1334009975649890238uL,
			2608012711638119052uL,
			6128411473006802146uL,
			8268148722764581231uL,
			9286055187155687089uL,
			11230858885718282805uL,
			13951009754708518548uL,
			16472876342353939154uL,
			17275323862435702243uL,
			1135362057144423861uL,
			2597628984639134821uL,
			3308224258029322869uL,
			5365058923640841347uL,
			6679025012923562964uL,
			8573033837759648693uL,
			10970295158949994411uL,
			12119686244451234320uL,
			12683024718118986047uL,
			13788192230050041572uL,
			14330467153632333762uL,
			15395433587784984357uL,
			489312712824947311uL,
			1452737877330783856uL,
			2861767655752347644uL,
			3322285676063803686uL,
			5560940570517711597uL,
			5996557281743188959uL,
			7280758554555802590uL,
			8532644243296465576uL,
			9350256976987008742uL,
			10552545826968843579uL,
			11727347734174303076uL,
			12113106623233404929uL,
			14000437183269869457uL,
			14369950271660146224uL,
			15101387698204529176uL,
			15463397548674623760uL,
			17586052441742319658uL,
			1182934255886127544uL,
			1847814050463011016uL,
			2177327727835720531uL,
			2830643537854262169uL,
			3796741975233480872uL,
			4115178125766777443uL,
			5681478168544905931uL,
			6601373596472566643uL,
			7507060721942968483uL,
			8399075790359081724uL,
			8693463985226723168uL,
			9568029438360202098uL,
			10144078919501101548uL,
			10430055236837252648uL,
			11840083180663258601uL,
			13761210420658862357uL,
			14299343276471374635uL,
			14566680578165727644uL,
			15097957966210449927uL,
			16922976911328602910uL,
			17689382322260857208uL,
			500013540394364858uL,
			748580250866718886uL,
			1242879168328830382uL,
			1977374033974150939uL,
			2944078676154940804uL,
			3659926193048069267uL,
			4368137639120453308uL,
			4836135668995329356uL,
			5532061633213252278uL,
			6448918945643986474uL,
			6902733635092675308uL,
			7801388544844847127uL
		};

		public override bool CanReuseTransform => true;

		public override bool CanTransformMultipleBlocks => true;

		public SHA2HashBase()
		{
			_buffer = new byte[8];
			Initialize();
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
				_byteCount1 += _buffer.Length;
			}
			while (cbSize > 0)
			{
				Update(array[ibStart]);
				ibStart++;
				cbSize--;
			}
		}

		public override void Initialize()
		{
			_byteCount1 = 0L;
			_byteCount2 = 0L;
			_bufferOffset = 0;
			for (int i = 0; i < _buffer.Length; i++)
			{
				_buffer[i] = 0;
			}
			_offset = 0;
			for (int j = 0; j < X.Length; j++)
			{
				X[j] = 0uL;
			}
		}

		protected void Finish()
		{
			AdjustByteCounts();
			long lowW = _byteCount1 << 3;
			long byteCount = _byteCount2;
			Update(128);
			while (_bufferOffset != 0)
			{
				Update(0);
			}
			ProcessLength(lowW, byteCount);
			ProcessBlock();
		}

		private void Update(byte input)
		{
			_buffer[_bufferOffset++] = input;
			if (_bufferOffset == _buffer.Length)
			{
				ProcessWord(_buffer, 0);
				_bufferOffset = 0;
			}
			_byteCount1++;
		}

		private void ProcessWord(byte[] input, int inOff)
		{
			X[_offset] = BE_To_UInt64(input, inOff);
			if (++_offset == 16)
			{
				ProcessBlock();
			}
		}

		internal void ProcessLength(long lowW, long hiW)
		{
			if (_offset > 14)
			{
				ProcessBlock();
			}
			X[14] = (ulong)hiW;
			X[15] = (ulong)lowW;
		}

		private void ProcessBlock()
		{
			AdjustByteCounts();
			for (int i = 16; i <= 79; i++)
			{
				X[i] = Sigma1(X[i - 2]) + X[i - 7] + Sigma0(X[i - 15]) + X[i - 16];
			}
			ulong num = H1;
			ulong num2 = H2;
			ulong num3 = H3;
			ulong num4 = H4;
			ulong num5 = H5;
			ulong num6 = H6;
			ulong num7 = H7;
			ulong num8 = H8;
			int num9 = 0;
			for (int j = 0; j < 10; j++)
			{
				num8 += Sum1(num5) + Ch(num5, num6, num7) + K[num9] + X[num9++];
				num4 += num8;
				num8 += Sum0(num) + Maj(num, num2, num3);
				num7 += Sum1(num4) + Ch(num4, num5, num6) + K[num9] + X[num9++];
				num3 += num7;
				num7 += Sum0(num8) + Maj(num8, num, num2);
				num6 += Sum1(num3) + Ch(num3, num4, num5) + K[num9] + X[num9++];
				num2 += num6;
				num6 += Sum0(num7) + Maj(num7, num8, num);
				num5 += Sum1(num2) + Ch(num2, num3, num4) + K[num9] + X[num9++];
				num += num5;
				num5 += Sum0(num6) + Maj(num6, num7, num8);
				num4 += Sum1(num) + Ch(num, num2, num3) + K[num9] + X[num9++];
				num8 += num4;
				num4 += Sum0(num5) + Maj(num5, num6, num7);
				num3 += Sum1(num8) + Ch(num8, num, num2) + K[num9] + X[num9++];
				num7 += num3;
				num3 += Sum0(num4) + Maj(num4, num5, num6);
				num2 += Sum1(num7) + Ch(num7, num8, num) + K[num9] + X[num9++];
				num6 += num2;
				num2 += Sum0(num3) + Maj(num3, num4, num5);
				num += Sum1(num6) + Ch(num6, num7, num8) + K[num9] + X[num9++];
				num5 += num;
				num += Sum0(num2) + Maj(num2, num3, num4);
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
				X[k] = 0uL;
			}
		}

		private void AdjustByteCounts()
		{
			if (_byteCount1 > 2305843009213693951L)
			{
				_byteCount2 += (long)((ulong)_byteCount1 >> 61);
				_byteCount1 &= 2305843009213693951L;
			}
		}

		private static ulong Ch(ulong x, ulong y, ulong z)
		{
			return (x & y) ^ (~x & z);
		}

		private static ulong Maj(ulong x, ulong y, ulong z)
		{
			return (x & y) ^ (x & z) ^ (y & z);
		}

		private static ulong Sum0(ulong x)
		{
			return ((x << 36) | (x >> 28)) ^ ((x << 30) | (x >> 34)) ^ ((x << 25) | (x >> 39));
		}

		private static ulong Sum1(ulong x)
		{
			return ((x << 50) | (x >> 14)) ^ ((x << 46) | (x >> 18)) ^ ((x << 23) | (x >> 41));
		}

		private static ulong Sigma0(ulong x)
		{
			return ((x << 63) | (x >> 1)) ^ ((x << 56) | (x >> 8)) ^ (x >> 7);
		}

		private static ulong Sigma1(ulong x)
		{
			return ((x << 45) | (x >> 19)) ^ ((x << 3) | (x >> 61)) ^ (x >> 6);
		}

		protected static void UInt32_To_BE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 24);
			bs[++off] = (byte)(n >> 16);
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)n;
		}

		protected static void UInt64_To_BE(ulong n, byte[] bs, int off)
		{
			UInt32_To_BE((uint)(n >> 32), bs, off);
			UInt32_To_BE((uint)n, bs, off + 4);
		}

		protected static ulong BE_To_UInt64(byte[] bs)
		{
			uint num = BE_To_UInt32(bs);
			uint num2 = BE_To_UInt32(bs, 4);
			return ((ulong)num << 32) | num2;
		}

		protected static ulong BE_To_UInt64(byte[] bs, int off)
		{
			uint num = BE_To_UInt32(bs, off);
			uint num2 = BE_To_UInt32(bs, off + 4);
			return ((ulong)num << 32) | num2;
		}

		protected static uint BE_To_UInt32(byte[] bs, int off)
		{
			uint num = (uint)(bs[off] << 24);
			num = (uint)((int)num | (bs[++off] << 16));
			num = (uint)((int)num | (bs[++off] << 8));
			return num | bs[++off];
		}

		protected static uint BE_To_UInt32(byte[] bs)
		{
			uint num = (uint)(bs[0] << 24);
			num = (uint)((int)num | (bs[1] << 16));
			num = (uint)((int)num | (bs[2] << 8));
			return num | bs[3];
		}
	}
}
