using System;

namespace Renci.SshNet.Security.Cryptography.Ciphers
{
	public sealed class TwofishCipher : BlockCipher
	{
		private const int P_00 = 1;

		private const int P_01 = 0;

		private const int P_02 = 0;

		private const int P_03 = 1;

		private const int P_04 = 1;

		private const int P_10 = 0;

		private const int P_11 = 0;

		private const int P_12 = 1;

		private const int P_13 = 1;

		private const int P_14 = 0;

		private const int P_20 = 1;

		private const int P_21 = 1;

		private const int P_22 = 0;

		private const int P_23 = 0;

		private const int P_24 = 0;

		private const int P_30 = 0;

		private const int P_31 = 1;

		private const int P_32 = 1;

		private const int P_33 = 0;

		private const int P_34 = 1;

		private const int GF256_FDBK = 361;

		private const int GF256_FDBK_2 = 180;

		private const int GF256_FDBK_4 = 90;

		private const int RS_GF_FDBK = 333;

		private const int ROUNDS = 16;

		private const int MAX_ROUNDS = 16;

		private const int MAX_KEY_BITS = 256;

		private const int INPUT_WHITEN = 0;

		private const int OUTPUT_WHITEN = 4;

		private const int ROUND_SUBKEYS = 8;

		private const int TOTAL_SUBKEYS = 40;

		private const int SK_STEP = 33686018;

		private const int SK_BUMP = 16843009;

		private const int SK_ROTL = 9;

		private static readonly byte[] P = new byte[512]
		{
			169,
			103,
			179,
			232,
			4,
			253,
			163,
			118,
			154,
			146,
			128,
			120,
			228,
			221,
			209,
			56,
			13,
			198,
			53,
			152,
			24,
			247,
			236,
			108,
			67,
			117,
			55,
			38,
			250,
			19,
			148,
			72,
			242,
			208,
			139,
			48,
			132,
			84,
			223,
			35,
			25,
			91,
			61,
			89,
			243,
			174,
			162,
			130,
			99,
			1,
			131,
			46,
			217,
			81,
			155,
			124,
			166,
			235,
			165,
			190,
			22,
			12,
			227,
			97,
			192,
			140,
			58,
			245,
			115,
			44,
			37,
			11,
			187,
			78,
			137,
			107,
			83,
			106,
			180,
			241,
			225,
			230,
			189,
			69,
			226,
			244,
			182,
			102,
			204,
			149,
			3,
			86,
			212,
			28,
			30,
			215,
			251,
			195,
			142,
			181,
			233,
			207,
			191,
			186,
			234,
			119,
			57,
			175,
			51,
			201,
			98,
			113,
			129,
			121,
			9,
			173,
			36,
			205,
			249,
			216,
			229,
			197,
			185,
			77,
			68,
			8,
			134,
			231,
			161,
			29,
			170,
			237,
			6,
			112,
			178,
			210,
			65,
			123,
			160,
			17,
			49,
			194,
			39,
			144,
			32,
			246,
			96,
			byte.MaxValue,
			150,
			92,
			177,
			171,
			158,
			156,
			82,
			27,
			95,
			147,
			10,
			239,
			145,
			133,
			73,
			238,
			45,
			79,
			143,
			59,
			71,
			135,
			109,
			70,
			214,
			62,
			105,
			100,
			42,
			206,
			203,
			47,
			252,
			151,
			5,
			122,
			172,
			127,
			213,
			26,
			75,
			14,
			167,
			90,
			40,
			20,
			63,
			41,
			136,
			60,
			76,
			2,
			184,
			218,
			176,
			23,
			85,
			31,
			138,
			125,
			87,
			199,
			141,
			116,
			183,
			196,
			159,
			114,
			126,
			21,
			34,
			18,
			88,
			7,
			153,
			52,
			110,
			80,
			222,
			104,
			101,
			188,
			219,
			248,
			200,
			168,
			43,
			64,
			220,
			254,
			50,
			164,
			202,
			16,
			33,
			240,
			211,
			93,
			15,
			0,
			111,
			157,
			54,
			66,
			74,
			94,
			193,
			224,
			117,
			243,
			198,
			244,
			219,
			123,
			251,
			200,
			74,
			211,
			230,
			107,
			69,
			125,
			232,
			75,
			214,
			50,
			216,
			253,
			55,
			113,
			241,
			225,
			48,
			15,
			248,
			27,
			135,
			250,
			6,
			63,
			94,
			186,
			174,
			91,
			138,
			0,
			188,
			157,
			109,
			193,
			177,
			14,
			128,
			93,
			210,
			213,
			160,
			132,
			7,
			20,
			181,
			144,
			44,
			163,
			178,
			115,
			76,
			84,
			146,
			116,
			54,
			81,
			56,
			176,
			189,
			90,
			252,
			96,
			98,
			150,
			108,
			66,
			247,
			16,
			124,
			40,
			39,
			140,
			19,
			149,
			156,
			199,
			36,
			70,
			59,
			112,
			202,
			227,
			133,
			203,
			17,
			208,
			147,
			184,
			166,
			131,
			32,
			byte.MaxValue,
			159,
			119,
			195,
			204,
			3,
			111,
			8,
			191,
			64,
			231,
			43,
			226,
			121,
			12,
			170,
			130,
			65,
			58,
			234,
			185,
			228,
			154,
			164,
			151,
			126,
			218,
			122,
			23,
			102,
			148,
			161,
			29,
			61,
			240,
			222,
			179,
			11,
			114,
			167,
			28,
			239,
			209,
			83,
			62,
			143,
			51,
			38,
			95,
			236,
			118,
			42,
			73,
			129,
			136,
			238,
			33,
			196,
			26,
			235,
			217,
			197,
			57,
			153,
			205,
			173,
			49,
			139,
			1,
			24,
			35,
			221,
			31,
			78,
			45,
			249,
			72,
			79,
			242,
			101,
			142,
			120,
			92,
			88,
			25,
			141,
			229,
			152,
			87,
			103,
			127,
			5,
			100,
			175,
			99,
			182,
			254,
			245,
			183,
			60,
			165,
			206,
			233,
			104,
			68,
			224,
			77,
			67,
			105,
			41,
			46,
			172,
			21,
			89,
			168,
			10,
			158,
			110,
			71,
			223,
			52,
			53,
			106,
			207,
			220,
			34,
			201,
			192,
			155,
			137,
			212,
			237,
			171,
			18,
			162,
			13,
			82,
			187,
			2,
			47,
			169,
			215,
			97,
			30,
			180,
			80,
			4,
			246,
			194,
			22,
			37,
			134,
			86,
			85,
			9,
			190,
			145
		};

		private readonly int[] gMDS0 = new int[256];

		private readonly int[] gMDS1 = new int[256];

		private readonly int[] gMDS2 = new int[256];

		private readonly int[] gMDS3 = new int[256];

		private int[] gSubKeys;

		private int[] gSBox;

		private int k64Cnt;

		public TwofishCipher(byte[] key, CipherMode mode, CipherPadding padding)
			: base(key, 16, mode, padding)
		{
			int num = key.Length * 8;
			if (num != 128 && num != 192 && num != 256)
			{
				throw new ArgumentException($"KeySize '{num}' is not valid for this algorithm.");
			}
			int[] array = new int[2];
			int[] array2 = new int[2];
			int[] array3 = new int[2];
			for (int i = 0; i < 256; i++)
			{
				int x = array[0] = (P[i] & 0xFF);
				array2[0] = (Mx_X(x) & 0xFF);
				array3[0] = (Mx_Y(x) & 0xFF);
				x = (array[1] = (P[256 + i] & 0xFF));
				array2[1] = (Mx_X(x) & 0xFF);
				array3[1] = (Mx_Y(x) & 0xFF);
				gMDS0[i] = (array[1] | (array2[1] << 8) | (array3[1] << 16) | (array3[1] << 24));
				gMDS1[i] = (array3[0] | (array3[0] << 8) | (array2[0] << 16) | (array[0] << 24));
				gMDS2[i] = (array2[1] | (array3[1] << 8) | (array[1] << 16) | (array3[1] << 24));
				gMDS3[i] = (array2[0] | (array[0] << 8) | (array3[0] << 16) | (array2[0] << 24));
			}
			k64Cnt = key.Length / 8;
			SetKey(key);
		}

		public override int EncryptBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			int num = BytesTo32Bits(inputBuffer, inputOffset) ^ gSubKeys[0];
			int num2 = BytesTo32Bits(inputBuffer, inputOffset + 4) ^ gSubKeys[1];
			int num3 = BytesTo32Bits(inputBuffer, inputOffset + 8) ^ gSubKeys[2];
			int num4 = BytesTo32Bits(inputBuffer, inputOffset + 12) ^ gSubKeys[3];
			int num5 = 8;
			for (int i = 0; i < 16; i += 2)
			{
				int num6 = Fe32_0(gSBox, num);
				int num7 = Fe32_3(gSBox, num2);
				num3 ^= num6 + num7 + gSubKeys[num5++];
				num3 = ((int)((uint)num3 >> 1) | (num3 << 31));
				num4 = (((num4 << 1) | (int)((uint)num4 >> 31)) ^ (num6 + 2 * num7 + gSubKeys[num5++]));
				num6 = Fe32_0(gSBox, num3);
				num7 = Fe32_3(gSBox, num4);
				num ^= num6 + num7 + gSubKeys[num5++];
				num = ((int)((uint)num >> 1) | (num << 31));
				num2 = (((num2 << 1) | (int)((uint)num2 >> 31)) ^ (num6 + 2 * num7 + gSubKeys[num5++]));
			}
			Bits32ToBytes(num3 ^ gSubKeys[4], outputBuffer, outputOffset);
			Bits32ToBytes(num4 ^ gSubKeys[5], outputBuffer, outputOffset + 4);
			Bits32ToBytes(num ^ gSubKeys[6], outputBuffer, outputOffset + 8);
			Bits32ToBytes(num2 ^ gSubKeys[7], outputBuffer, outputOffset + 12);
			return base.BlockSize;
		}

		public override int DecryptBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			int num = BytesTo32Bits(inputBuffer, inputOffset) ^ gSubKeys[4];
			int num2 = BytesTo32Bits(inputBuffer, inputOffset + 4) ^ gSubKeys[5];
			int num3 = BytesTo32Bits(inputBuffer, inputOffset + 8) ^ gSubKeys[6];
			int num4 = BytesTo32Bits(inputBuffer, inputOffset + 12) ^ gSubKeys[7];
			int num5 = 39;
			for (int i = 0; i < 16; i += 2)
			{
				int num6 = Fe32_0(gSBox, num);
				int num7 = Fe32_3(gSBox, num2);
				num4 ^= num6 + 2 * num7 + gSubKeys[num5--];
				num3 = (((num3 << 1) | (int)((uint)num3 >> 31)) ^ (num6 + num7 + gSubKeys[num5--]));
				num4 = ((int)((uint)num4 >> 1) | (num4 << 31));
				num6 = Fe32_0(gSBox, num3);
				num7 = Fe32_3(gSBox, num4);
				num2 ^= num6 + 2 * num7 + gSubKeys[num5--];
				num = (((num << 1) | (int)((uint)num >> 31)) ^ (num6 + num7 + gSubKeys[num5--]));
				num2 = ((int)((uint)num2 >> 1) | (num2 << 31));
			}
			Bits32ToBytes(num3 ^ gSubKeys[0], outputBuffer, outputOffset);
			Bits32ToBytes(num4 ^ gSubKeys[1], outputBuffer, outputOffset + 4);
			Bits32ToBytes(num ^ gSubKeys[2], outputBuffer, outputOffset + 8);
			Bits32ToBytes(num2 ^ gSubKeys[3], outputBuffer, outputOffset + 12);
			return base.BlockSize;
		}

		private void SetKey(byte[] key)
		{
			int[] array = new int[4];
			int[] array2 = new int[4];
			int[] array3 = new int[4];
			gSubKeys = new int[40];
			if (k64Cnt < 1)
			{
				throw new ArgumentException("Key size less than 64 bits");
			}
			if (k64Cnt > 4)
			{
				throw new ArgumentException("Key size larger than 256 bits");
			}
			for (int i = 0; i < k64Cnt; i++)
			{
				int num = i * 8;
				array[i] = BytesTo32Bits(key, num);
				array2[i] = BytesTo32Bits(key, num + 4);
				array3[k64Cnt - 1 - i] = RS_MDS_Encode(array[i], array2[i]);
			}
			for (int j = 0; j < 20; j++)
			{
				int num2 = j * 33686018;
				int num3 = F32(num2, array);
				int num4 = F32(num2 + 16843009, array2);
				num4 = ((num4 << 8) | (int)((uint)num4 >> 24));
				num3 += num4;
				gSubKeys[j * 2] = num3;
				num3 += num4;
				gSubKeys[j * 2 + 1] = ((num3 << 9) | (int)((uint)num3 >> 23));
			}
			int x = array3[0];
			int x2 = array3[1];
			int x3 = array3[2];
			int x4 = array3[3];
			gSBox = new int[1024];
			for (int k = 0; k < 256; k++)
			{
				int num7;
				int num6;
				int num5;
				int num8 = num7 = (num6 = (num5 = k));
				switch (k64Cnt & 3)
				{
				case 1:
					gSBox[k * 2] = gMDS0[(P[num8] & 0xFF) ^ M_b0(x)];
					gSBox[k * 2 + 1] = gMDS1[(P[num7] & 0xFF) ^ M_b1(x)];
					gSBox[k * 2 + 512] = gMDS2[(P[256 + num6] & 0xFF) ^ M_b2(x)];
					gSBox[k * 2 + 513] = gMDS3[(P[256 + num5] & 0xFF) ^ M_b3(x)];
					break;
				case 0:
					num8 = ((P[256 + num8] & 0xFF) ^ M_b0(x4));
					num7 = ((P[num7] & 0xFF) ^ M_b1(x4));
					num6 = ((P[num6] & 0xFF) ^ M_b2(x4));
					num5 = ((P[256 + num5] & 0xFF) ^ M_b3(x4));
					goto case 3;
				case 3:
					num8 = ((P[256 + num8] & 0xFF) ^ M_b0(x3));
					num7 = ((P[256 + num7] & 0xFF) ^ M_b1(x3));
					num6 = ((P[num6] & 0xFF) ^ M_b2(x3));
					num5 = ((P[num5] & 0xFF) ^ M_b3(x3));
					goto case 2;
				case 2:
					gSBox[k * 2] = gMDS0[(P[(P[num8] & 0xFF) ^ M_b0(x2)] & 0xFF) ^ M_b0(x)];
					gSBox[k * 2 + 1] = gMDS1[(P[(P[256 + num7] & 0xFF) ^ M_b1(x2)] & 0xFF) ^ M_b1(x)];
					gSBox[k * 2 + 512] = gMDS2[(P[(256 + (P[num6] & 0xFF)) ^ M_b2(x2)] & 0xFF) ^ M_b2(x)];
					gSBox[k * 2 + 513] = gMDS3[(P[(256 + (P[256 + num5] & 0xFF)) ^ M_b3(x2)] & 0xFF) ^ M_b3(x)];
					break;
				}
			}
		}

		private int F32(int x, int[] k32)
		{
			int num = M_b0(x);
			int num2 = M_b1(x);
			int num3 = M_b2(x);
			int num4 = M_b3(x);
			int x2 = k32[0];
			int x3 = k32[1];
			int x4 = k32[2];
			int x5 = k32[3];
			int result = 0;
			switch (k64Cnt & 3)
			{
			case 1:
				result = (gMDS0[(P[num] & 0xFF) ^ M_b0(x2)] ^ gMDS1[(P[num2] & 0xFF) ^ M_b1(x2)] ^ gMDS2[(P[256 + num3] & 0xFF) ^ M_b2(x2)] ^ gMDS3[(P[256 + num4] & 0xFF) ^ M_b3(x2)]);
				break;
			case 0:
				num = ((P[256 + num] & 0xFF) ^ M_b0(x5));
				num2 = ((P[num2] & 0xFF) ^ M_b1(x5));
				num3 = ((P[num3] & 0xFF) ^ M_b2(x5));
				num4 = ((P[256 + num4] & 0xFF) ^ M_b3(x5));
				goto case 3;
			case 3:
				num = ((P[256 + num] & 0xFF) ^ M_b0(x4));
				num2 = ((P[256 + num2] & 0xFF) ^ M_b1(x4));
				num3 = ((P[num3] & 0xFF) ^ M_b2(x4));
				num4 = ((P[num4] & 0xFF) ^ M_b3(x4));
				goto case 2;
			case 2:
				result = (gMDS0[(P[(P[num] & 0xFF) ^ M_b0(x3)] & 0xFF) ^ M_b0(x2)] ^ gMDS1[(P[(P[256 + num2] & 0xFF) ^ M_b1(x3)] & 0xFF) ^ M_b1(x2)] ^ gMDS2[(P[(256 + (P[num3] & 0xFF)) ^ M_b2(x3)] & 0xFF) ^ M_b2(x2)] ^ gMDS3[(P[(256 + (P[256 + num4] & 0xFF)) ^ M_b3(x3)] & 0xFF) ^ M_b3(x2)]);
				break;
			}
			return result;
		}

		private static int RS_MDS_Encode(int k0, int k1)
		{
			int x = RS_rem(k1);
			x = RS_rem(x);
			x = RS_rem(x);
			x = RS_rem(x);
			x ^= k0;
			x = RS_rem(x);
			x = RS_rem(x);
			x = RS_rem(x);
			return RS_rem(x);
		}

		private static int RS_rem(int x)
		{
			int num = (int)(((uint)x >> 24) & 0xFF);
			int num2 = ((num << 1) ^ (((num & 0x80) != 0) ? 333 : 0)) & 0xFF;
			int num3 = (int)((uint)num >> 1) ^ (((num & 1) != 0) ? 166 : 0) ^ num2;
			return (x << 8) ^ (num3 << 24) ^ (num2 << 16) ^ (num3 << 8) ^ num;
		}

		private static int LFSR1(int x)
		{
			return (x >> 1) ^ (((x & 1) != 0) ? 180 : 0);
		}

		private static int LFSR2(int x)
		{
			return (x >> 2) ^ (((x & 2) != 0) ? 180 : 0) ^ (((x & 1) != 0) ? 90 : 0);
		}

		private static int Mx_X(int x)
		{
			return x ^ LFSR2(x);
		}

		private static int Mx_Y(int x)
		{
			return x ^ LFSR1(x) ^ LFSR2(x);
		}

		private static int M_b0(int x)
		{
			return x & 0xFF;
		}

		private static int M_b1(int x)
		{
			return (int)(((uint)x >> 8) & 0xFF);
		}

		private static int M_b2(int x)
		{
			return (int)(((uint)x >> 16) & 0xFF);
		}

		private static int M_b3(int x)
		{
			return (int)(((uint)x >> 24) & 0xFF);
		}

		private static int Fe32_0(int[] gSBox1, int x)
		{
			return gSBox1[2 * (x & 0xFF)] ^ gSBox1[1 + 2 * (((uint)x >> 8) & 0xFF)] ^ gSBox1[512 + 2 * (((uint)x >> 16) & 0xFF)] ^ gSBox1[513 + 2 * (((uint)x >> 24) & 0xFF)];
		}

		private static int Fe32_3(int[] gSBox1, int x)
		{
			return gSBox1[2 * (((uint)x >> 24) & 0xFF)] ^ gSBox1[1 + 2 * (x & 0xFF)] ^ gSBox1[512 + 2 * (((uint)x >> 8) & 0xFF)] ^ gSBox1[513 + 2 * (((uint)x >> 16) & 0xFF)];
		}

		private static int BytesTo32Bits(byte[] b, int p)
		{
			return (b[p] & 0xFF) | ((b[p + 1] & 0xFF) << 8) | ((b[p + 2] & 0xFF) << 16) | ((b[p + 3] & 0xFF) << 24);
		}

		private static void Bits32ToBytes(int inData, byte[] b, int offset)
		{
			b[offset] = (byte)inData;
			b[offset + 1] = (byte)(inData >> 8);
			b[offset + 2] = (byte)(inData >> 16);
			b[offset + 3] = (byte)(inData >> 24);
		}
	}
}
