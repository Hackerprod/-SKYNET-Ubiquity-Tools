using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace Renci.SshNet.Common
{
	public struct BigInteger : IComparable, IFormattable, IComparable<BigInteger>, IEquatable<BigInteger>
	{
		private const ulong _BASE = 4294967296uL;

		private const int _DECIMALSIGNMASK = int.MinValue;

		private const int _BIAS = 1075;

		private static readonly RNGCryptoServiceProvider _randomizer = new RNGCryptoServiceProvider();

		private static readonly uint[] _zero = new uint[1];

		private static readonly uint[] _one = new uint[1]
		{
			1u
		};

		private readonly uint[] _data;

		private readonly short _sign;

		public int BitLength
		{
			get
			{
				if (_sign == 0)
				{
					return 0;
				}
				int num = _data.Length - 1;
				while (_data[num] == 0)
				{
					num--;
				}
				int num2 = BitScanBackward(_data[num]) + 1;
				return num * 4 * 8 + num2 + ((_sign <= 0) ? 1 : 0);
			}
		}

		public bool IsEven => (_data[0] & 1) == 0;

		public bool IsOne
		{
			get
			{
				if (_sign == 1 && _data.Length == 1)
				{
					return _data[0] == 1;
				}
				return false;
			}
		}

		public bool IsPowerOfTwo
		{
			get
			{
				bool flag = false;
				if (_sign != 1)
				{
					return false;
				}
				for (int i = 0; i < _data.Length; i++)
				{
					int num = PopulationCount(_data[i]);
					if (num > 0)
					{
						if (num > 1 || flag)
						{
							return false;
						}
						flag = true;
					}
				}
				return flag;
			}
		}

		public bool IsZero => _sign == 0;

		public static BigInteger MinusOne => new BigInteger(-1, _one);

		public static BigInteger One => new BigInteger(1, _one);

		public int Sign => _sign;

		public static BigInteger Zero => new BigInteger(0, _zero);

		public BigInteger(short sign, uint[] data)
		{
			_sign = sign;
			_data = data;
		}

		public BigInteger(int value)
		{
			if (value == 0)
			{
				_sign = 0;
				_data = _zero;
			}
			else if (value > 0)
			{
				_sign = 1;
				_data = new uint[1]
				{
					(uint)value
				};
			}
			else
			{
				_sign = -1;
				_data = new uint[1]
				{
					(uint)(-value)
				};
			}
		}

		public BigInteger(uint value)
		{
			if (value == 0)
			{
				_sign = 0;
				_data = _zero;
			}
			else
			{
				_sign = 1;
				_data = new uint[1]
				{
					value
				};
			}
		}

		public BigInteger(long value)
		{
			if (value == 0)
			{
				_sign = 0;
				_data = _zero;
			}
			else if (value > 0)
			{
				_sign = 1;
				uint num = (uint)value;
				uint num2 = (uint)(value >> 32);
				_data = new uint[(num2 == 0) ? 1 : 2];
				_data[0] = num;
				if (num2 != 0)
				{
					_data[1] = num2;
				}
			}
			else
			{
				_sign = -1;
				value = -value;
				uint num3 = (uint)value;
				uint num4 = (uint)((ulong)value >> 32);
				_data = new uint[(num4 == 0) ? 1 : 2];
				_data[0] = num3;
				if (num4 != 0)
				{
					_data[1] = num4;
				}
			}
		}

		public BigInteger(ulong value)
		{
			if (value == 0)
			{
				_sign = 0;
				_data = _zero;
			}
			else
			{
				_sign = 1;
				uint num = (uint)value;
				uint num2 = (uint)(value >> 32);
				_data = new uint[(num2 == 0) ? 1 : 2];
				_data[0] = num;
				if (num2 != 0)
				{
					_data[1] = num2;
				}
			}
		}

		public BigInteger(double value)
		{
			if (double.IsNaN(value) || double.IsInfinity(value))
			{
				throw new OverflowException();
			}
			byte[] bytes = BitConverter.GetBytes(value);
			ulong num = Mantissa(bytes);
			if (num == 0)
			{
				int num2 = Exponent(bytes);
				if (num2 == 0)
				{
					_sign = 0;
					_data = _zero;
				}
				else
				{
					BigInteger value2 = Negative(bytes) ? MinusOne : One;
					value2 <<= num2 - 1023;
					_sign = value2._sign;
					_data = value2._data;
				}
			}
			else
			{
				int num3 = Exponent(bytes);
				num |= 0x10000000000000;
				BigInteger value3 = num;
				value3 = ((num3 > 1075) ? (value3 << num3 - 1075) : (value3 >> 1075 - num3));
				_sign = (short)((!Negative(bytes)) ? 1 : (-1));
				_data = value3._data;
			}
		}

		public BigInteger(float value)
		{
			this = new BigInteger((double)value);
		}

		public BigInteger(decimal value)
		{
			int[] bits = decimal.GetBits(decimal.Truncate(value));
			int num = 3;
			while (num > 0 && bits[num - 1] == 0)
			{
				num--;
			}
			if (num == 0)
			{
				_sign = 0;
				_data = _zero;
			}
			else
			{
				_sign = (short)(((bits[3] & -2147483648) == 0) ? 1 : (-1));
				_data = new uint[num];
				_data[0] = (uint)bits[0];
				if (num > 1)
				{
					_data[1] = (uint)bits[1];
				}
				if (num > 2)
				{
					_data[2] = (uint)bits[2];
				}
			}
		}

		public BigInteger(byte[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int num = value.Length;
			switch (num)
			{
			case 1:
				if (value[0] != 0)
				{
					break;
				}
				goto case 0;
			case 0:
				_sign = 0;
				_data = _zero;
				return;
			}
			if ((value[num - 1] & 0x80) != 0)
			{
				_sign = -1;
			}
			else
			{
				_sign = 1;
			}
			if (_sign == 1)
			{
				while (value[num - 1] == 0)
				{
					num--;
				}
				int num2;
				int num3 = num2 = num / 4;
				if ((num & 3) != 0)
				{
					num2++;
				}
				_data = new uint[num2];
				int num4 = 0;
				for (int i = 0; i < num3; i++)
				{
					_data[i] = (uint)(value[num4++] | (value[num4++] << 8) | (value[num4++] << 16) | (value[num4++] << 24));
				}
				num2 = (num & 3);
				if (num2 > 0)
				{
					int num9 = _data.Length - 1;
					for (int j = 0; j < num2; j++)
					{
						_data[num9] |= (uint)(value[num4++] << j * 8);
					}
				}
			}
			else
			{
				int num11;
				int num12 = num11 = num / 4;
				if ((num & 3) != 0)
				{
					num11++;
				}
				_data = new uint[num11];
				uint num13 = 1u;
				ulong num14 = 0uL;
				int num15 = 0;
				for (int k = 0; k < num12; k++)
				{
					uint num20 = (uint)(value[num15++] | (value[num15++] << 8) | (value[num15++] << 16) | (value[num15++] << 24));
					num14 = (ulong)((long)num20 - (long)num13);
					num20 = (uint)num14;
					num13 = (uint)((int)(num14 >> 32) & 1);
					_data[k] = ~num20;
				}
				num11 = (num & 3);
				if (num11 > 0)
				{
					uint num20 = 0u;
					uint num21 = 0u;
					for (int l = 0; l < num11; l++)
					{
						num20 = (uint)((int)num20 | (value[num15++] << l * 8));
						num21 = ((num21 << 8) | 0xFF);
					}
					num14 = num20 - num13;
					num20 = (uint)num14;
					num13 = (uint)((int)(num14 >> 32) & 1);
					_data[_data.Length - 1] = (~num20 & num21);
				}
				if (num13 != 0)
				{
					throw new Exception("non zero final carry");
				}
			}
		}

		public static explicit operator int(BigInteger value)
		{
			if (!value.AsInt32(out int val))
			{
				throw new OverflowException();
			}
			return val;
		}

		public static explicit operator uint(BigInteger value)
		{
			if (value._data.Length > 1 || value._sign == -1)
			{
				throw new OverflowException();
			}
			return value._data[0];
		}

		public static explicit operator short(BigInteger value)
		{
			int num = (int)value;
			if (num < -32768 || num > 32767)
			{
				throw new OverflowException();
			}
			return (short)num;
		}

		public static explicit operator ushort(BigInteger value)
		{
			uint num = (uint)value;
			if (num > 65535)
			{
				throw new OverflowException();
			}
			return (ushort)num;
		}

		public static explicit operator byte(BigInteger value)
		{
			uint num = (uint)value;
			if (num > 255)
			{
				throw new OverflowException();
			}
			return (byte)num;
		}

		public static explicit operator sbyte(BigInteger value)
		{
			int num = (int)value;
			if (num < -128 || num > 127)
			{
				throw new OverflowException();
			}
			return (sbyte)num;
		}

		public static explicit operator long(BigInteger value)
		{
			if (value._sign == 0)
			{
				return 0L;
			}
			if (value._data.Length > 2)
			{
				throw new OverflowException();
			}
			uint num = value._data[0];
			if (value._data.Length == 1)
			{
				if (value._sign == 1)
				{
					return num;
				}
				long num2 = num;
				return -num2;
			}
			uint num3 = value._data[1];
			if (value._sign == 1)
			{
				if (num3 >= 2147483648u)
				{
					throw new OverflowException();
				}
				return (long)(((ulong)num3 << 32) | num);
			}
			if (num3 > 2147483648u)
			{
				throw new OverflowException();
			}
			return (long)(0L - (((ulong)num3 << 32) | num));
		}

		public static explicit operator ulong(BigInteger value)
		{
			if (value._data.Length > 2 || value._sign == -1)
			{
				throw new OverflowException();
			}
			uint num = value._data[0];
			if (value._data.Length == 1)
			{
				return num;
			}
			uint num2 = value._data[1];
			return ((ulong)num2 << 32) | num;
		}

		public static explicit operator double(BigInteger value)
		{
			try
			{
				return double.Parse(value.ToString(), CultureInfo.InvariantCulture.NumberFormat);
			}
			catch (OverflowException)
			{
				return (value._sign == -1) ? (double.NegativeInfinity) : double.PositiveInfinity;
			}
		}

		public static explicit operator float(BigInteger value)
		{
			try
			{
				return float.Parse(value.ToString(), CultureInfo.InvariantCulture.NumberFormat);
			}
			catch (OverflowException)
			{
				return (value._sign == -1) ? (float.NegativeInfinity) : float.PositiveInfinity;
			}
		}

		public static explicit operator decimal(BigInteger value)
		{
			if (value._sign == 0)
			{
				return 0m;
			}
			uint[] data = value._data;
			if (data.Length > 3)
			{
				throw new OverflowException();
			}
			int lo = 0;
			int mid = 0;
			int hi = 0;
			if (data.Length > 2)
			{
				hi = (int)data[2];
			}
			if (data.Length > 1)
			{
				mid = (int)data[1];
			}
			if (data.Length > 0)
			{
				lo = (int)data[0];
			}
			return new decimal(lo, mid, hi, value._sign < 0, 0);
		}

		public static implicit operator BigInteger(int value)
		{
			return new BigInteger(value);
		}

		public static implicit operator BigInteger(uint value)
		{
			return new BigInteger(value);
		}

		public static implicit operator BigInteger(short value)
		{
			return new BigInteger(value);
		}

		public static implicit operator BigInteger(ushort value)
		{
			return new BigInteger(value);
		}

		public static implicit operator BigInteger(byte value)
		{
			return new BigInteger(value);
		}

		public static implicit operator BigInteger(sbyte value)
		{
			return new BigInteger(value);
		}

		public static implicit operator BigInteger(long value)
		{
			return new BigInteger(value);
		}

		public static implicit operator BigInteger(ulong value)
		{
			return new BigInteger(value);
		}

		public static explicit operator BigInteger(double value)
		{
			return new BigInteger(value);
		}

		public static explicit operator BigInteger(float value)
		{
			return new BigInteger(value);
		}

		public static explicit operator BigInteger(decimal value)
		{
			return new BigInteger(value);
		}

		public static BigInteger operator +(BigInteger left, BigInteger right)
		{
			if (left._sign == 0)
			{
				return right;
			}
			if (right._sign == 0)
			{
				return left;
			}
			if (left._sign == right._sign)
			{
				return new BigInteger(left._sign, CoreAdd(left._data, right._data));
			}
			int num = CoreCompare(left._data, right._data);
			if (num == 0)
			{
				return new BigInteger(0, _zero);
			}
			if (num > 0)
			{
				return new BigInteger(left._sign, CoreSub(left._data, right._data));
			}
			return new BigInteger(right._sign, CoreSub(right._data, left._data));
		}

		public static BigInteger operator -(BigInteger left, BigInteger right)
		{
			if (right._sign == 0)
			{
				return left;
			}
			if (left._sign == 0)
			{
				return new BigInteger((short)(-right._sign), right._data);
			}
			if (left._sign == right._sign)
			{
				int num = CoreCompare(left._data, right._data);
				if (num == 0)
				{
					return new BigInteger(0, _zero);
				}
				if (num > 0)
				{
					return new BigInteger(left._sign, CoreSub(left._data, right._data));
				}
				return new BigInteger((short)(-right._sign), CoreSub(right._data, left._data));
			}
			return new BigInteger(left._sign, CoreAdd(left._data, right._data));
		}

		public static BigInteger operator *(BigInteger left, BigInteger right)
		{
			if (left._sign == 0 || right._sign == 0)
			{
				return new BigInteger(0, _zero);
			}
			if (left._data[0] == 1 && left._data.Length == 1)
			{
				if (left._sign == 1)
				{
					return right;
				}
				return new BigInteger((short)(-right._sign), right._data);
			}
			if (right._data[0] == 1 && right._data.Length == 1)
			{
				if (right._sign == 1)
				{
					return left;
				}
				return new BigInteger((short)(-left._sign), left._data);
			}
			uint[] data = left._data;
			uint[] data2 = right._data;
			uint[] array = new uint[data.Length + data2.Length];
			for (int i = 0; i < data.Length; i++)
			{
				uint num = data[i];
				int num2 = i;
				ulong num3 = 0uL;
				for (int j = 0; j < data2.Length; j++)
				{
					num3 = (ulong)((long)num3 + (long)num * (long)data2[j] + array[num2]);
					array[num2++] = (uint)num3;
					num3 >>= 32;
				}
				while (num3 != 0)
				{
					num3 += array[num2];
					array[num2++] = (uint)num3;
					num3 >>= 32;
				}
			}
			int num6 = array.Length - 1;
			while (num6 >= 0 && array[num6] == 0)
			{
				num6--;
			}
			if (num6 < array.Length - 1)
			{
				array = Resize(array, num6 + 1);
			}
			return new BigInteger((short)(left._sign * right._sign), array);
		}

		public static BigInteger operator /(BigInteger dividend, BigInteger divisor)
		{
			if (divisor._sign == 0)
			{
				throw new DivideByZeroException();
			}
			if (dividend._sign == 0)
			{
				return dividend;
			}
			DivModUnsigned(dividend._data, divisor._data, out uint[] q, out uint[] _);
			int num = q.Length - 1;
			while (num >= 0 && q[num] == 0)
			{
				num--;
			}
			if (num == -1)
			{
				return new BigInteger(0, _zero);
			}
			if (num < q.Length - 1)
			{
				q = Resize(q, num + 1);
			}
			return new BigInteger((short)(dividend._sign * divisor._sign), q);
		}

		public static BigInteger operator %(BigInteger dividend, BigInteger divisor)
		{
			if (divisor._sign == 0)
			{
				throw new DivideByZeroException();
			}
			if (dividend._sign == 0)
			{
				return dividend;
			}
			DivModUnsigned(dividend._data, divisor._data, out uint[] _, out uint[] r);
			int num = r.Length - 1;
			while (num >= 0 && r[num] == 0)
			{
				num--;
			}
			if (num == -1)
			{
				return new BigInteger(0, _zero);
			}
			if (num < r.Length - 1)
			{
				r = Resize(r, num + 1);
			}
			return new BigInteger(dividend._sign, r);
		}

		public static BigInteger operator -(BigInteger value)
		{
			if (value._sign == 0)
			{
				return value;
			}
			return new BigInteger((short)(-value._sign), value._data);
		}

		public static BigInteger operator +(BigInteger value)
		{
			return value;
		}

		public static BigInteger operator ++(BigInteger value)
		{
			short sign = value._sign;
			uint[] data = value._data;
			if (data.Length == 1)
			{
				if (sign == -1 && data[0] == 1)
				{
					return new BigInteger(0, _zero);
				}
				if (sign == 0)
				{
					return new BigInteger(1, _one);
				}
			}
			data = ((sign != -1) ? CoreAdd(data, 1u) : CoreSub(data, 1u));
			return new BigInteger(sign, data);
		}

		public static BigInteger operator --(BigInteger value)
		{
			short sign = value._sign;
			uint[] data = value._data;
			if (data.Length == 1)
			{
				if (sign == 1 && data[0] == 1)
				{
					return new BigInteger(0, _zero);
				}
				if (sign == 0)
				{
					return new BigInteger(-1, _one);
				}
			}
			data = ((sign != -1) ? CoreSub(data, 1u) : CoreAdd(data, 1u));
			return new BigInteger(sign, data);
		}

		public static BigInteger operator &(BigInteger left, BigInteger right)
		{
			if (left._sign == 0)
			{
				return left;
			}
			if (right._sign == 0)
			{
				return right;
			}
			uint[] data = left._data;
			uint[] data2 = right._data;
			int sign = left._sign;
			int sign2 = right._sign;
			bool flag = sign == sign2 && sign == -1;
			uint[] array = new uint[Math.Max(data.Length, data2.Length)];
			ulong num = 1uL;
			ulong num2 = 1uL;
			ulong num3 = 1uL;
			int i;
			for (i = 0; i < array.Length; i++)
			{
				uint num4 = 0u;
				if (i < data.Length)
				{
					num4 = data[i];
				}
				if (sign == -1)
				{
					num = ~num4 + num;
					num4 = (uint)num;
					num = (uint)(num >> 32);
				}
				uint num5 = 0u;
				if (i < data2.Length)
				{
					num5 = data2[i];
				}
				if (sign2 == -1)
				{
					num2 = ~num5 + num2;
					num5 = (uint)num2;
					num2 = (uint)(num2 >> 32);
				}
				uint num6 = num4 & num5;
				if (flag)
				{
					num3 = num6 - num3;
					num6 = (uint)(~num3);
					num3 = (uint)((int)(num3 >> 32) & 1);
				}
				array[i] = num6;
			}
			i = array.Length - 1;
			while (i >= 0 && array[i] == 0)
			{
				i--;
			}
			if (i == -1)
			{
				return new BigInteger(0, _zero);
			}
			if (i < array.Length - 1)
			{
				array = Resize(array, i + 1);
			}
			return new BigInteger((short)((!flag) ? 1 : (-1)), array);
		}

		public static BigInteger operator |(BigInteger left, BigInteger right)
		{
			if (left._sign == 0)
			{
				return right;
			}
			if (right._sign == 0)
			{
				return left;
			}
			uint[] data = left._data;
			uint[] data2 = right._data;
			int sign = left._sign;
			int sign2 = right._sign;
			bool flag = sign == -1 || sign2 == -1;
			uint[] array = new uint[Math.Max(data.Length, data2.Length)];
			ulong num = 1uL;
			ulong num2 = 1uL;
			ulong num3 = 1uL;
			int i;
			for (i = 0; i < array.Length; i++)
			{
				uint num4 = 0u;
				if (i < data.Length)
				{
					num4 = data[i];
				}
				if (sign == -1)
				{
					num = ~num4 + num;
					num4 = (uint)num;
					num = (uint)(num >> 32);
				}
				uint num5 = 0u;
				if (i < data2.Length)
				{
					num5 = data2[i];
				}
				if (sign2 == -1)
				{
					num2 = ~num5 + num2;
					num5 = (uint)num2;
					num2 = (uint)(num2 >> 32);
				}
				uint num6 = num4 | num5;
				if (flag)
				{
					num3 = num6 - num3;
					num6 = (uint)(~num3);
					num3 = (uint)((int)(num3 >> 32) & 1);
				}
				array[i] = num6;
			}
			i = array.Length - 1;
			while (i >= 0 && array[i] == 0)
			{
				i--;
			}
			if (i == -1)
			{
				return new BigInteger(0, _zero);
			}
			if (i < array.Length - 1)
			{
				array = Resize(array, i + 1);
			}
			return new BigInteger((short)((!flag) ? 1 : (-1)), array);
		}

		public static BigInteger operator ^(BigInteger left, BigInteger right)
		{
			if (left._sign == 0)
			{
				return right;
			}
			if (right._sign == 0)
			{
				return left;
			}
			uint[] data = left._data;
			uint[] data2 = right._data;
			int sign = left._sign;
			int sign2 = right._sign;
			bool flag = (sign == -1) ^ (sign2 == -1);
			uint[] array = new uint[Math.Max(data.Length, data2.Length)];
			ulong num = 1uL;
			ulong num2 = 1uL;
			ulong num3 = 1uL;
			int i;
			for (i = 0; i < array.Length; i++)
			{
				uint num4 = 0u;
				if (i < data.Length)
				{
					num4 = data[i];
				}
				if (sign == -1)
				{
					num = ~num4 + num;
					num4 = (uint)num;
					num = (uint)(num >> 32);
				}
				uint num5 = 0u;
				if (i < data2.Length)
				{
					num5 = data2[i];
				}
				if (sign2 == -1)
				{
					num2 = ~num5 + num2;
					num5 = (uint)num2;
					num2 = (uint)(num2 >> 32);
				}
				uint num6 = num4 ^ num5;
				if (flag)
				{
					num3 = num6 - num3;
					num6 = (uint)(~num3);
					num3 = (uint)((int)(num3 >> 32) & 1);
				}
				array[i] = num6;
			}
			i = array.Length - 1;
			while (i >= 0 && array[i] == 0)
			{
				i--;
			}
			if (i == -1)
			{
				return new BigInteger(0, _zero);
			}
			if (i < array.Length - 1)
			{
				array = Resize(array, i + 1);
			}
			return new BigInteger((short)((!flag) ? 1 : (-1)), array);
		}

		public static BigInteger operator ~(BigInteger value)
		{
			if (value._sign == 0)
			{
				return new BigInteger(-1, _one);
			}
			uint[] data = value._data;
			int sign = value._sign;
			bool flag = sign == 1;
			uint[] array = new uint[data.Length];
			ulong num = 1uL;
			ulong num2 = 1uL;
			int i;
			for (i = 0; i < array.Length; i++)
			{
				uint num3 = data[i];
				if (sign == -1)
				{
					num = ~num3 + num;
					num3 = (uint)num;
					num = (uint)(num >> 32);
				}
				num3 = ~num3;
				if (flag)
				{
					num2 = num3 - num2;
					num3 = (uint)(~num2);
					num2 = (uint)((int)(num2 >> 32) & 1);
				}
				array[i] = num3;
			}
			i = array.Length - 1;
			while (i >= 0 && array[i] == 0)
			{
				i--;
			}
			if (i == -1)
			{
				return new BigInteger(0, _zero);
			}
			if (i < array.Length - 1)
			{
				array = Resize(array, i + 1);
			}
			return new BigInteger((short)((!flag) ? 1 : (-1)), array);
		}

		public static BigInteger operator <<(BigInteger value, int shift)
		{
			if (shift == 0 || value._sign == 0)
			{
				return value;
			}
			if (shift < 0)
			{
				return value >> -shift;
			}
			uint[] data = value._data;
			int sign = value._sign;
			int num = BitScanBackward(data[data.Length - 1]);
			int num2 = shift - (31 - num);
			int num3 = (num2 >> 5) + (((num2 & 0x1F) != 0) ? 1 : 0);
			uint[] array = new uint[data.Length + num3];
			int num4 = shift >> 5;
			int num5 = shift & 0x1F;
			int num6 = 32 - num5;
			for (int i = 0; i < data.Length; i++)
			{
				uint num7 = data[i];
				array[i + num4] |= num7 << num5;
				if (i + num4 + 1 < array.Length)
				{
					array[i + num4 + 1] = num7 >> num6;
				}
			}
			return new BigInteger((short)sign, array);
		}

		public static BigInteger operator >>(BigInteger value, int shift)
		{
			if (shift == 0 || value._sign == 0)
			{
				return value;
			}
			if (shift < 0)
			{
				return value << -shift;
			}
			uint[] data = value._data;
			int sign = value._sign;
			int num = BitScanBackward(data[data.Length - 1]);
			int num2 = shift >> 5;
			int num3 = shift & 0x1F;
			int num4 = num2;
			if (num3 > num)
			{
				num4++;
			}
			int num5 = data.Length - num4;
			if (num5 <= 0)
			{
				if (sign == 1)
				{
					return new BigInteger(0, _zero);
				}
				return new BigInteger(-1, _one);
			}
			uint[] array = new uint[num5];
			int num6 = 32 - num3;
			for (int num7 = data.Length - 1; num7 >= num2; num7--)
			{
				uint num8 = data[num7];
				if (num7 - num2 < array.Length)
				{
					array[num7 - num2] |= num8 >> num3;
				}
				if (num7 - num2 - 1 >= 0)
				{
					array[num7 - num2 - 1] = num8 << num6;
				}
			}
			if (sign == -1)
			{
				for (int i = 0; i < num2; i++)
				{
					if (data[i] != 0)
					{
						BigInteger value2 = new BigInteger((short)sign, array);
						return --value2;
					}
				}
				if (num3 > 0 && data[num2] << num6 != 0)
				{
					BigInteger value3 = new BigInteger((short)sign, array);
					return --value3;
				}
			}
			return new BigInteger((short)sign, array);
		}

		public static bool operator <(BigInteger left, BigInteger right)
		{
			return Compare(left, right) < 0;
		}

		public static bool operator <(BigInteger left, long right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator <(long left, BigInteger right)
		{
			return right.CompareTo(left) > 0;
		}

		public static bool operator <(BigInteger left, ulong right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator <(ulong left, BigInteger right)
		{
			return right.CompareTo(left) > 0;
		}

		public static bool operator <=(BigInteger left, BigInteger right)
		{
			return Compare(left, right) <= 0;
		}

		public static bool operator <=(BigInteger left, long right)
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool operator <=(long left, BigInteger right)
		{
			return right.CompareTo(left) >= 0;
		}

		public static bool operator <=(BigInteger left, ulong right)
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool operator <=(ulong left, BigInteger right)
		{
			return right.CompareTo(left) >= 0;
		}

		public static bool operator >(BigInteger left, BigInteger right)
		{
			return Compare(left, right) > 0;
		}

		public static bool operator >(BigInteger left, long right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool operator >(long left, BigInteger right)
		{
			return right.CompareTo(left) < 0;
		}

		public static bool operator >(BigInteger left, ulong right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool operator >(ulong left, BigInteger right)
		{
			return right.CompareTo(left) < 0;
		}

		public static bool operator >=(BigInteger left, BigInteger right)
		{
			return Compare(left, right) >= 0;
		}

		public static bool operator >=(BigInteger left, long right)
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool operator >=(long left, BigInteger right)
		{
			return right.CompareTo(left) <= 0;
		}

		public static bool operator >=(BigInteger left, ulong right)
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool operator >=(ulong left, BigInteger right)
		{
			return right.CompareTo(left) <= 0;
		}

		public static bool operator ==(BigInteger left, BigInteger right)
		{
			return Compare(left, right) == 0;
		}

		public static bool operator ==(BigInteger left, long right)
		{
			return left.CompareTo(right) == 0;
		}

		public static bool operator ==(long left, BigInteger right)
		{
			return right.CompareTo(left) == 0;
		}

		public static bool operator ==(BigInteger left, ulong right)
		{
			return left.CompareTo(right) == 0;
		}

		public static bool operator ==(ulong left, BigInteger right)
		{
			return right.CompareTo(left) == 0;
		}

		public static bool operator !=(BigInteger left, BigInteger right)
		{
			return Compare(left, right) != 0;
		}

		public static bool operator !=(BigInteger left, long right)
		{
			return left.CompareTo(right) != 0;
		}

		public static bool operator !=(long left, BigInteger right)
		{
			return right.CompareTo(left) != 0;
		}

		public static bool operator !=(BigInteger left, ulong right)
		{
			return left.CompareTo(right) != 0;
		}

		public static bool operator !=(ulong left, BigInteger right)
		{
			return right.CompareTo(left) != 0;
		}

		public static BigInteger Abs(BigInteger value)
		{
			return new BigInteger(Math.Abs(value._sign), value._data);
		}

		public static BigInteger Add(BigInteger left, BigInteger right)
		{
			return left + right;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is BigInteger))
			{
				return -1;
			}
			return Compare(this, (BigInteger)obj);
		}

		public int CompareTo(BigInteger other)
		{
			return Compare(this, other);
		}

		public int CompareTo(ulong other)
		{
			if (_sign < 0)
			{
				return -1;
			}
			if (_sign == 0)
			{
				if (other != 0)
				{
					return -1;
				}
				return 0;
			}
			if (_data.Length > 2)
			{
				return 1;
			}
			uint high = (uint)(other >> 32);
			uint low = (uint)other;
			return LongCompare(low, high);
		}

		public static BigInteger Random(int bitLength)
		{
			byte[] array = new byte[bitLength / 8 + ((bitLength % 8 > 0) ? 1 : 0)];
			_randomizer.GetBytes(array);
			array[array.Length - 1] = (byte)(array[array.Length - 1] & 0x7F);
			return new BigInteger(array.ToArray());
		}

		public static BigInteger Divide(BigInteger dividend, BigInteger divisor)
		{
			return dividend / divisor;
		}

		public static BigInteger DivRem(BigInteger dividend, BigInteger divisor, out BigInteger remainder)
		{
			if (divisor._sign == 0)
			{
				throw new DivideByZeroException();
			}
			if (dividend._sign == 0)
			{
				remainder = dividend;
				return dividend;
			}
			DivModUnsigned(dividend._data, divisor._data, out uint[] q, out uint[] r);
			int num = r.Length - 1;
			while (num >= 0 && r[num] == 0)
			{
				num--;
			}
			if (num == -1)
			{
				remainder = new BigInteger(0, _zero);
			}
			else
			{
				if (num < r.Length - 1)
				{
					r = Resize(r, num + 1);
				}
				remainder = new BigInteger(dividend._sign, r);
			}
			num = q.Length - 1;
			while (num >= 0 && q[num] == 0)
			{
				num--;
			}
			if (num == -1)
			{
				return new BigInteger(0, _zero);
			}
			if (num < q.Length - 1)
			{
				q = Resize(q, num + 1);
			}
			return new BigInteger((short)(dividend._sign * divisor._sign), q);
		}

		public bool Equals(BigInteger other)
		{
			if (_sign != other._sign)
			{
				return false;
			}
			if (_data.Length != other._data.Length)
			{
				return false;
			}
			for (int i = 0; i < _data.Length; i++)
			{
				if (_data[i] != other._data[i])
				{
					return false;
				}
			}
			return true;
		}

		public bool Equals(long other)
		{
			return CompareTo(other) == 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is BigInteger))
			{
				return false;
			}
			return Equals((BigInteger)obj);
		}

		public bool Equals(ulong other)
		{
			return CompareTo(other) == 0;
		}

		public override int GetHashCode()
		{
			uint num = (uint)((long)_sign * 16843009L);
			for (int i = 0; i < _data.Length; i++)
			{
				num ^= _data[i];
			}
			return (int)num;
		}

		public static BigInteger GreatestCommonDivisor(BigInteger left, BigInteger right)
		{
			if (left._data.Length == 1 && left._data[0] == 1)
			{
				return new BigInteger(1, _one);
			}
			if (right._data.Length == 1 && right._data[0] == 1)
			{
				return new BigInteger(1, _one);
			}
			if (left.IsZero)
			{
				return right;
			}
			if (right.IsZero)
			{
				return left;
			}
			BigInteger bigInteger = new BigInteger(1, left._data);
			BigInteger bigInteger2 = new BigInteger(1, right._data);
			BigInteger bigInteger3 = bigInteger2;
			while (bigInteger._data.Length > 1)
			{
				bigInteger3 = bigInteger;
				bigInteger = bigInteger2 % bigInteger;
				bigInteger2 = bigInteger3;
			}
			if (bigInteger.IsZero)
			{
				return bigInteger3;
			}
			uint num = bigInteger._data[0];
			uint num2 = (uint)(bigInteger2 % num);
			int num3 = 0;
			while (((num2 | num) & 1) == 0)
			{
				num2 >>= 1;
				num >>= 1;
				num3++;
			}
			while (num2 != 0)
			{
				while ((num2 & 1) == 0)
				{
					num2 >>= 1;
				}
				while ((num & 1) == 0)
				{
					num >>= 1;
				}
				if (num2 >= num)
				{
					num2 = num2 - num >> 1;
				}
				else
				{
					num = num - num2 >> 1;
				}
			}
			return num << num3;
		}

		public static double Log(BigInteger value, double baseValue)
		{
			if (value._sign == -1 || baseValue == 1.0 || baseValue == -1.0 || baseValue == double.NegativeInfinity || double.IsNaN(baseValue))
			{
				return double.NaN;
			}
			if (baseValue == 0.0 || baseValue == double.PositiveInfinity)
			{
				if (!value.IsOne)
				{
					return double.NaN;
				}
				return 0.0;
			}
			if (value._sign == 0)
			{
				return double.NegativeInfinity;
			}
			int num = value._data.Length - 1;
			int num2 = -1;
			for (int num3 = 31; num3 >= 0; num3--)
			{
				if ((value._data[num] & (1 << num3)) != 0)
				{
					num2 = num3 + num * 32;
					break;
				}
			}
			long num4 = num2;
			double num5 = 0.0;
			double num6 = 1.0;
			BigInteger value2 = One;
			long num7;
			for (num7 = num4; num7 > 2147483647; num7 -= 2147483647)
			{
				value2 <<= 2147483647;
			}
			value2 <<= (int)num7;
			for (long num8 = num4; num8 >= 0; num8--)
			{
				if ((value & value2)._sign != 0)
				{
					num5 += num6;
				}
				num6 *= 0.5;
				value2 >>= 1;
			}
			return (Math.Log(num5) + Math.Log(2.0) * (double)num4) / Math.Log(baseValue);
		}

		public static double Log(BigInteger value)
		{
			return Log(value, 2.7182818284590451);
		}

		public static double Log10(BigInteger value)
		{
			return Log(value, 10.0);
		}

		public static BigInteger Max(BigInteger left, BigInteger right)
		{
			int sign = left._sign;
			int sign2 = right._sign;
			if (sign > sign2)
			{
				return left;
			}
			if (sign2 > sign)
			{
				return right;
			}
			int num = CoreCompare(left._data, right._data);
			if (sign == -1)
			{
				num = -num;
			}
			if (num >= 0)
			{
				return left;
			}
			return right;
		}

		public static BigInteger Min(BigInteger left, BigInteger right)
		{
			int sign = left._sign;
			int sign2 = right._sign;
			if (sign < sign2)
			{
				return left;
			}
			if (sign2 < sign)
			{
				return right;
			}
			int num = CoreCompare(left._data, right._data);
			if (sign == -1)
			{
				num = -num;
			}
			if (num <= 0)
			{
				return left;
			}
			return right;
		}

		public static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus)
		{
			if (exponent._sign == -1)
			{
				throw new ArgumentOutOfRangeException("exponent", "power must be >= 0");
			}
			if (modulus._sign == 0)
			{
				throw new DivideByZeroException();
			}
			BigInteger bigInteger = One % modulus;
			while (exponent._sign != 0)
			{
				if (!exponent.IsEven)
				{
					bigInteger *= value;
					bigInteger %= modulus;
				}
				if (exponent.IsOne)
				{
					break;
				}
				value *= value;
				value %= modulus;
				exponent >>= 1;
			}
			return bigInteger;
		}

		public static BigInteger ModInverse(BigInteger bi, BigInteger modulus)
		{
			BigInteger bigInteger = modulus;
			BigInteger bigInteger2 = bi % modulus;
			BigInteger bigInteger3 = 0;
			BigInteger bigInteger4 = 1;
			while (!bigInteger2.IsZero)
			{
				if (bigInteger2.IsOne)
				{
					return bigInteger4;
				}
				bigInteger3 += bigInteger / bigInteger2 * bigInteger4;
				bigInteger %= bigInteger2;
				if (bigInteger.IsZero)
				{
					break;
				}
				if (bigInteger.IsOne)
				{
					return modulus - bigInteger3;
				}
				bigInteger4 += bigInteger2 / bigInteger * bigInteger3;
				bigInteger2 %= bigInteger;
			}
			return 0;
		}

		public static BigInteger PositiveMod(BigInteger dividend, BigInteger divisor)
		{
			BigInteger bigInteger = dividend % divisor;
			if (bigInteger < 0L)
			{
				bigInteger += divisor;
			}
			return bigInteger;
		}

		public static BigInteger Multiply(BigInteger left, BigInteger right)
		{
			return left * right;
		}

		public static BigInteger Negate(BigInteger value)
		{
			return -value;
		}

		public static BigInteger Parse(string value, NumberStyles style, IFormatProvider provider)
		{
			if (!Parse(value, tryParse: false, style, provider, out BigInteger result, out Exception exc))
			{
				throw exc;
			}
			return result;
		}

		public static BigInteger Parse(string value, IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public static BigInteger Parse(string value, NumberStyles style)
		{
			throw new NotImplementedException();
		}

		public static BigInteger Pow(BigInteger value, int exponent)
		{
			if (exponent >= 0)
			{
				switch (exponent)
				{
				case 0:
					return One;
				case 1:
					return value;
				default:
				{
					BigInteger bigInteger = One;
					while (exponent != 0)
					{
						if ((exponent & 1) != 0)
						{
							bigInteger *= value;
						}
						if (exponent == 1)
						{
							break;
						}
						value *= value;
						exponent >>= 1;
					}
					return bigInteger;
				}
				}
			}
			throw new ArgumentOutOfRangeException("exponent", "exp must be >= 0");
		}

		public static BigInteger Remainder(BigInteger dividend, BigInteger divisor)
		{
			return dividend % divisor;
		}

		public static BigInteger Subtract(BigInteger left, BigInteger right)
		{
			return left - right;
		}

		public byte[] ToByteArray()
		{
			if (_sign == 0)
			{
				return new byte[1];
			}
			int num = (_data.Length - 1) * 4;
			bool flag = false;
			uint num2 = _data[_data.Length - 1];
			int num3;
			if (_sign == 1)
			{
				num3 = TopByte(num2);
				uint num4 = (uint)(128 << (num3 - 1) * 8);
				if ((num2 & num4) != 0)
				{
					flag = true;
				}
			}
			else
			{
				num3 = TopByte(num2);
			}
			byte[] array = new byte[num + num3 + (flag ? 1 : 0)];
			if (_sign == 1)
			{
				int num5 = 0;
				int num6 = _data.Length - 1;
				for (int i = 0; i < num6; i++)
				{
					uint num7 = _data[i];
					array[num5++] = (byte)num7;
					array[num5++] = (byte)(num7 >> 8);
					array[num5++] = (byte)(num7 >> 16);
					array[num5++] = (byte)(num7 >> 24);
				}
				while (num3-- > 0)
				{
					array[num5++] = (byte)num2;
					num2 >>= 8;
				}
			}
			else
			{
				int num14 = 0;
				int num15 = _data.Length - 1;
				uint num16 = 1u;
				ulong num18;
				uint num17;
				for (int j = 0; j < num15; j++)
				{
					num17 = _data[j];
					num18 = (ulong)((long)(~num17) + (long)num16);
					num17 = (uint)num18;
					num16 = (uint)(num18 >> 32);
					array[num14++] = (byte)num17;
					array[num14++] = (byte)(num17 >> 8);
					array[num14++] = (byte)(num17 >> 16);
					array[num14++] = (byte)(num17 >> 24);
				}
				num18 = (ulong)((long)(~num2) + (long)num16);
				num17 = (uint)num18;
				if ((int)(num18 >> 32) == 0)
				{
					int num23 = FirstNonFFByte(num17);
					bool flag2 = (num17 & (1 << num23 * 8 - 1)) == 0;
					int num24 = num23 + (flag2 ? 1 : 0);
					if (num24 != num3)
					{
						array = Resize(array, num + num24);
					}
					while (num23-- > 0)
					{
						array[num14++] = (byte)num17;
						num17 >>= 8;
					}
					if (flag2)
					{
						array[num14++] = byte.MaxValue;
					}
				}
				else
				{
					array = Resize(array, num + 5);
					array[num14++] = (byte)num17;
					array[num14++] = (byte)(num17 >> 8);
					array[num14++] = (byte)(num17 >> 16);
					array[num14++] = (byte)(num17 >> 24);
					array[num14++] = byte.MaxValue;
				}
			}
			return array;
		}

		public override string ToString()
		{
			return ToString(10u, null);
		}

		public string ToString(IFormatProvider provider)
		{
			return ToString(null, provider);
		}

		public string ToString(string format)
		{
			return ToString(format, null);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			if (!string.IsNullOrEmpty(format))
			{
				switch (format[0])
				{
				case 'D':
				case 'G':
				case 'R':
				case 'd':
				case 'g':
				case 'r':
					return ToStringWithPadding(format, 10u, provider);
				case 'X':
				case 'x':
					return ToStringWithPadding(format, 16u, null);
				default:
					throw new FormatException($"format '{format}' not implemented");
				}
			}
			return ToString(10u, provider);
		}

		public static bool TryParse(string value, NumberStyles style, CultureInfo cultureInfo, out BigInteger result)
		{
			Exception exc;
			return Parse(value, tryParse: true, style, cultureInfo, out result, out exc);
		}

		public static bool TryParse(string value, out BigInteger result)
		{
			throw new NotImplementedException();
		}

		public int CompareTo(long other)
		{
			int sign = _sign;
			int num = Math.Sign(other);
			if (sign != num)
			{
				if (sign <= num)
				{
					return -1;
				}
				return 1;
			}
			if (sign == 0)
			{
				return 0;
			}
			if (_data.Length > 2)
			{
				return _sign;
			}
			if (other < 0)
			{
				other = -other;
			}
			uint low = (uint)other;
			uint high = (uint)((ulong)other >> 32);
			int num2 = LongCompare(low, high);
			if (sign == -1)
			{
				num2 = -num2;
			}
			return num2;
		}

		public static int Compare(BigInteger left, BigInteger right)
		{
			int sign = left._sign;
			int sign2 = right._sign;
			if (sign != sign2)
			{
				if (sign <= sign2)
				{
					return -1;
				}
				return 1;
			}
			int num = CoreCompare(left._data, right._data);
			if (sign < 0)
			{
				num = -num;
			}
			return num;
		}

		private static bool Negative(byte[] v)
		{
			return (v[7] & 0x80) != 0;
		}

		private static ushort Exponent(byte[] v)
		{
			return (ushort)(((ushort)(v[7] & 0x7F) << 4) | ((ushort)(v[6] & 0xF0) >> 4));
		}

		private static ulong Mantissa(byte[] v)
		{
			uint num = (uint)(v[0] | (v[1] << 8) | (v[2] << 16) | (v[3] << 24));
			uint num2 = (uint)(v[4] | (v[5] << 8) | ((v[6] & 0xF) << 16));
			return num | ((ulong)num2 << 32);
		}

		private static int PopulationCount(uint x)
		{
			x -= ((x >> 1) & 0x55555555);
			x = (x & 0x33333333) + ((x >> 2) & 0x33333333);
			x = ((x + (x >> 4)) & 0xF0F0F0F);
			x += x >> 8;
			x += x >> 16;
			return (int)(x & 0x3F);
		}

		private string ToStringWithPadding(string format, uint radix, IFormatProvider provider)
		{
			if (format.Length > 1)
			{
				int num = Convert.ToInt32(format.Substring(1), CultureInfo.InvariantCulture.NumberFormat);
				string text = ToString(radix, provider);
				if (text.Length < num)
				{
					string text2 = new string('0', num - text.Length);
					if (text[0] != '-')
					{
						return text2 + text;
					}
					return "-" + text2 + text.Substring(1);
				}
				return text;
			}
			return ToString(radix, provider);
		}

		private static uint[] MakeTwoComplement(uint[] v)
		{
			uint[] array = new uint[v.Length];
			ulong num = 1uL;
			for (int i = 0; i < v.Length; i++)
			{
				uint num2 = v[i];
				num = ~num2 + num;
				num2 = (uint)num;
				num = (uint)(num >> 32);
				array[i] = num2;
			}
			uint num3 = array[array.Length - 1];
			int num4 = FirstNonFFByte(num3);
			uint num5 = 255u;
			for (int j = 1; j < num4; j++)
			{
				num5 = ((num5 << 8) | 0xFF);
			}
			array[array.Length - 1] = (num3 & num5);
			return array;
		}

		private string ToString(uint radix, IFormatProvider provider)
		{
			if ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length < radix)
			{
				throw new ArgumentException("charSet length less than radix", "characterSet");
			}
			if (radix == 1)
			{
				throw new ArgumentException("There is no such thing as radix one notation", "radix");
			}
			if (_sign == 0)
			{
				return "0";
			}
			if (_data.Length == 1 && _data[0] == 1)
			{
				if (_sign != 1)
				{
					return "-1";
				}
				return "1";
			}
			List<char> list = new List<char>(1 + _data.Length * 3 / 10);
			BigInteger bigInteger;
			if (_sign == 1)
			{
				bigInteger = this;
			}
			else
			{
				uint[] array = _data;
				if (radix > 10)
				{
					array = MakeTwoComplement(array);
				}
				bigInteger = new BigInteger(1, array);
			}
			while (bigInteger != 0L)
			{
				bigInteger = DivRem(bigInteger, radix, out BigInteger remainder);
				list.Add("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"[(int)remainder]);
			}
			if (_sign == -1 && radix == 10)
			{
				NumberFormatInfo numberFormatInfo = null;
				if (provider != null)
				{
					numberFormatInfo = (provider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo);
				}
				if (numberFormatInfo != null)
				{
					string negativeSign = numberFormatInfo.NegativeSign;
					for (int num = negativeSign.Length - 1; num >= 0; num--)
					{
						list.Add(negativeSign[num]);
					}
				}
				else
				{
					list.Add('-');
				}
			}
			char c = list[list.Count - 1];
			if (_sign == 1 && radix > 10 && (c < '0' || c > '9'))
			{
				list.Add('0');
			}
			list.Reverse();
			return new string(list.ToArray());
		}

		private static Exception GetFormatException()
		{
			return new FormatException("Input string was not in the correct format");
		}

		private static bool ProcessTrailingWhitespace(bool tryParse, string s, int position, ref Exception exc)
		{
			int length = s.Length;
			for (int i = position; i < length; i++)
			{
				char c = s[i];
				if (c != 0 && !char.IsWhiteSpace(c))
				{
					if (!tryParse)
					{
						exc = GetFormatException();
					}
					return false;
				}
			}
			return true;
		}

		private static bool Parse(string s, bool tryParse, NumberStyles style, IFormatProvider provider, out BigInteger result, out Exception exc)
		{
			int num = 1;
			bool flag = false;
			int value = 10;
			switch (style)
			{
			case NumberStyles.AllowHexSpecifier:
			case NumberStyles.HexNumber:
				value = 16;
				break;
			default:
				throw new NotSupportedException($"Style '{style}' is not supported.");
			case NumberStyles.None:
				break;
			}
			result = Zero;
			exc = null;
			if (s == null)
			{
				if (!tryParse)
				{
					exc = new ArgumentNullException("value");
				}
				return false;
			}
			int length = s.Length;
			int i;
			for (i = 0; i < length; i++)
			{
				char c = s[i];
				if (!char.IsWhiteSpace(c))
				{
					break;
				}
			}
			if (i == length)
			{
				if (!tryParse)
				{
					exc = GetFormatException();
				}
				return false;
			}
			NumberFormatInfo numberFormatInfo = provider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo;
			string negativeSign = numberFormatInfo.NegativeSign;
			string positiveSign = numberFormatInfo.PositiveSign;
			if (string.CompareOrdinal(s, i, positiveSign, 0, positiveSign.Length) == 0)
			{
				i += positiveSign.Length;
			}
			else if (string.CompareOrdinal(s, i, negativeSign, 0, negativeSign.Length) == 0)
			{
				num = -1;
				i += negativeSign.Length;
			}
			BigInteger bigInteger = Zero;
			for (; i < length; i++)
			{
				char c = s[i];
				if (c == '\0')
				{
					i = length;
				}
				else if (c >= '0' && c <= '9')
				{
					byte value2 = (byte)(c - 48);
					bigInteger = bigInteger * value + value2;
					flag = true;
				}
				else if (c >= 'A' && c <= 'F')
				{
					byte value3 = (byte)(c - 65 + 10);
					bigInteger = bigInteger * value + value3;
					flag = true;
				}
				else if (!ProcessTrailingWhitespace(tryParse, s, i, ref exc))
				{
					return false;
				}
			}
			if (!flag)
			{
				if (!tryParse)
				{
					exc = GetFormatException();
				}
				return false;
			}
			if (bigInteger._sign == 0)
			{
				result = bigInteger;
			}
			else if (num == -1)
			{
				result = new BigInteger(-1, bigInteger._data);
			}
			else
			{
				result = new BigInteger(1, bigInteger._data);
			}
			return true;
		}

		private int LongCompare(uint low, uint high)
		{
			uint num = 0u;
			if (_data.Length > 1)
			{
				num = _data[1];
			}
			if (num > high)
			{
				return 1;
			}
			if (num < high)
			{
				return -1;
			}
			uint num2 = _data[0];
			if (num2 > low)
			{
				return 1;
			}
			if (num2 < low)
			{
				return -1;
			}
			return 0;
		}

		private bool AsUInt64(out ulong val)
		{
			val = 0uL;
			if (_data.Length > 2 || _sign == -1)
			{
				return false;
			}
			val = _data[0];
			if (_data.Length == 1)
			{
				return true;
			}
			uint num = _data[1];
			val |= (ulong)num << 32;
			return true;
		}

		private bool AsInt32(out int val)
		{
			val = 0;
			if (_data.Length > 1)
			{
				return false;
			}
			uint num = _data[0];
			if (_sign == 1)
			{
				if (num > 2147483647)
				{
					return false;
				}
				val = (int)num;
			}
			else if (_sign == -1)
			{
				if (num > 2147483648u)
				{
					return false;
				}
				val = (int)(0 - num);
			}
			return true;
		}

		private static int BitScanBackward(uint word)
		{
			for (int num = 31; num >= 0; num--)
			{
				uint num2 = (uint)(1 << num);
				if ((word & num2) == num2)
				{
					return num;
				}
			}
			return 0;
		}

		private static int TopByte(uint x)
		{
			if (((int)x & -65536) != 0)
			{
				if (((int)x & -16777216) != 0)
				{
					return 4;
				}
				return 3;
			}
			if ((x & 0xFF00) != 0)
			{
				return 2;
			}
			return 1;
		}

		private static int FirstNonFFByte(uint word)
		{
			if (((int)word & -16777216) != -16777216)
			{
				return 4;
			}
			if ((word & 0xFF0000) != 16711680)
			{
				return 3;
			}
			if ((word & 0xFF00) != 65280)
			{
				return 2;
			}
			return 1;
		}

		private static byte[] Resize(byte[] v, int len)
		{
			byte[] array = new byte[len];
			Buffer.BlockCopy(v, 0, array, 0, Math.Min(v.Length, len));
			Array.Copy(v, array, Math.Min(v.Length, len));
			return array;
		}

		private static uint[] Resize(uint[] v, int len)
		{
			uint[] array = new uint[len];
			Buffer.BlockCopy(v, 0, array, 0, Math.Min(v.Length, len) * 4);
			return array;
		}

		private static uint[] CoreAdd(uint[] a, uint[] b)
		{
			if (a.Length < b.Length)
			{
				uint[] array = a;
				a = b;
				b = array;
			}
			int num = a.Length;
			int num2 = b.Length;
			uint[] array2 = new uint[num];
			ulong num3 = 0uL;
			int i;
			for (i = 0; i < num2; i++)
			{
				num3 = num3 + a[i] + b[i];
				array2[i] = (uint)num3;
				num3 >>= 32;
			}
			for (; i < num; i++)
			{
				num3 += a[i];
				array2[i] = (uint)num3;
				num3 >>= 32;
			}
			if (num3 != 0)
			{
				array2 = Resize(array2, num + 1);
				array2[i] = (uint)num3;
			}
			return array2;
		}

		private static uint[] CoreSub(uint[] a, uint[] b)
		{
			int num = a.Length;
			int num2 = b.Length;
			uint[] array = new uint[num];
			ulong num3 = 0uL;
			int i;
			for (i = 0; i < num2; i++)
			{
				num3 = (ulong)((long)a[i] - (long)b[i] - (long)num3);
				array[i] = (uint)num3;
				num3 = ((num3 >> 32) & 1);
			}
			for (; i < num; i++)
			{
				num3 = a[i] - num3;
				array[i] = (uint)num3;
				num3 = ((num3 >> 32) & 1);
			}
			i = num - 1;
			while (i >= 0 && array[i] == 0)
			{
				i--;
			}
			if (i < num - 1)
			{
				array = Resize(array, i + 1);
			}
			return array;
		}

		private static uint[] CoreAdd(uint[] a, uint b)
		{
			int num = a.Length;
			uint[] array = new uint[num];
			ulong num2 = b;
			int i;
			for (i = 0; i < num; i++)
			{
				num2 += a[i];
				array[i] = (uint)num2;
				num2 >>= 32;
			}
			if (num2 != 0)
			{
				array = Resize(array, num + 1);
				array[i] = (uint)num2;
			}
			return array;
		}

		private static uint[] CoreSub(uint[] a, uint b)
		{
			int num = a.Length;
			uint[] array = new uint[num];
			ulong num2 = b;
			int i;
			for (i = 0; i < num; i++)
			{
				num2 = a[i] - num2;
				array[i] = (uint)num2;
				num2 = ((num2 >> 32) & 1);
			}
			i = num - 1;
			while (i >= 0 && array[i] == 0)
			{
				i--;
			}
			if (i < num - 1)
			{
				array = Resize(array, i + 1);
			}
			return array;
		}

		private static int CoreCompare(uint[] a, uint[] b)
		{
			int num = a.Length;
			int num2 = b.Length;
			if (num > num2)
			{
				return 1;
			}
			if (num2 > num)
			{
				return -1;
			}
			for (int num3 = num - 1; num3 >= 0; num3--)
			{
				uint num4 = a[num3];
				uint num5 = b[num3];
				if (num4 > num5)
				{
					return 1;
				}
				if (num4 < num5)
				{
					return -1;
				}
			}
			return 0;
		}

		private static int GetNormalizeShift(uint value)
		{
			int num = 0;
			if (((int)value & -65536) == 0)
			{
				value <<= 16;
				num += 16;
			}
			if (((int)value & -16777216) == 0)
			{
				value <<= 8;
				num += 8;
			}
			if (((int)value & -268435456) == 0)
			{
				value <<= 4;
				num += 4;
			}
			if (((int)value & -1073741824) == 0)
			{
				value <<= 2;
				num += 2;
			}
			if (((int)value & -2147483648) == 0)
			{
				value <<= 1;
				num++;
			}
			return num;
		}

		private static void Normalize(uint[] u, int l, uint[] un, int shift)
		{
			uint num = 0u;
			int i;
			if (shift > 0)
			{
				int num2 = 32 - shift;
				for (i = 0; i < l; i++)
				{
					uint num3 = u[i];
					un[i] = ((num3 << shift) | num);
					num = num3 >> num2;
				}
			}
			else
			{
				for (i = 0; i < l; i++)
				{
					un[i] = u[i];
				}
			}
			while (i < un.Length)
			{
				un[i++] = 0u;
			}
			if (num != 0)
			{
				un[l] = num;
			}
		}

		private static void Unnormalize(uint[] un, out uint[] r, int shift)
		{
			int num = un.Length;
			r = new uint[num];
			if (shift > 0)
			{
				int num2 = 32 - shift;
				uint num3 = 0u;
				for (int num4 = num - 1; num4 >= 0; num4--)
				{
					uint num5 = un[num4];
					r[num4] = ((num5 >> shift) | num3);
					num3 = num5 << num2;
				}
			}
			else
			{
				for (int i = 0; i < num; i++)
				{
					r[i] = un[i];
				}
			}
		}

		private static void DivModUnsigned(uint[] u, uint[] v, out uint[] q, out uint[] r)
		{
			int num = u.Length;
			int num2 = v.Length;
			if (num2 <= 1)
			{
				ulong num3 = 0uL;
				uint num4 = v[0];
				q = new uint[num];
				r = new uint[1];
				for (int num5 = num - 1; num5 >= 0; num5--)
				{
					num3 *= 4294967296L;
					num3 += u[num5];
					ulong num6 = num3 / num4;
					num3 -= num6 * num4;
					q[num5] = (uint)num6;
				}
				r[0] = (uint)num3;
			}
			else if (num >= num2)
			{
				int normalizeShift = GetNormalizeShift(v[num2 - 1]);
				uint[] array = new uint[num + 1];
				uint[] array2 = new uint[num2];
				Normalize(u, num, array, normalizeShift);
				Normalize(v, num2, array2, normalizeShift);
				q = new uint[num - num2 + 1];
				r = null;
				for (int num7 = num - num2; num7 >= 0; num7--)
				{
					ulong num8 = (ulong)(4294967296L * array[num7 + num2] + array[num7 + num2 - 1]);
					ulong num9 = num8 / array2[num2 - 1];
					num8 -= num9 * array2[num2 - 1];
					while (num9 >= 4294967296L || num9 * array2[num2 - 2] > num8 * 4294967296L + array[num7 + num2 - 2])
					{
						num9--;
						num8 += array2[num2 - 1];
						if (num8 >= 4294967296L)
						{
							break;
						}
					}
					long num10 = 0L;
					long num11 = 0L;
					for (int i = 0; i < num2; i++)
					{
						ulong num12 = array2[i] * num9;
						num11 = (long)array[i + num7] - (long)(uint)num12 - num10;
						array[i + num7] = (uint)num11;
						num12 >>= 32;
						num11 >>= 32;
						num10 = (long)num12 - num11;
					}
					num11 = array[num7 + num2] - num10;
					array[num7 + num2] = (uint)num11;
					q[num7] = (uint)num9;
					if (num11 < 0)
					{
						q[num7]--;
						ulong num13 = 0uL;
						for (int i = 0; i < num2; i++)
						{
							num13 = (ulong)((long)array2[i] + (long)array[num7 + i] + (long)num13);
							array[num7 + i] = (uint)num13;
							num13 >>= 32;
						}
						num13 += array[num7 + num2];
						array[num7 + num2] = (uint)num13;
					}
				}
				Unnormalize(array, out r, normalizeShift);
			}
			else
			{
				uint[] array3 = q = new uint[1];
				r = u;
			}
		}
	}
}
