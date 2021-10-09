using System.Text;

namespace Renci.SshNet.Common
{
	public class ASCIIEncoding : Encoding
	{
		private readonly char _fallbackChar;

		private static readonly char[] _byteToChar;

		static ASCIIEncoding()
		{
			if (_byteToChar == null)
			{
				_byteToChar = new char[128];
				char c = '\0';
				for (byte b = 0; b < 128; b = (byte)(b + 1))
				{
					char[] byteToChar = _byteToChar;
					byte num = b;
					char num2 = c;
					c = (char)(num2 + 1);
					byteToChar[num] = num2;
				}
			}
		}

		public ASCIIEncoding()
		{
			_fallbackChar = '?';
		}

		public override int GetByteCount(char[] chars, int index, int count)
		{
			return count;
		}

		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			for (int i = 0; i < charCount && i < chars.Length; i++)
			{
				byte b = (byte)chars[i + charIndex];
				if (b > 127)
				{
					b = (byte)_fallbackChar;
				}
				bytes[i + byteIndex] = b;
			}
			return charCount;
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return count;
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			for (int i = 0; i < byteCount; i++)
			{
				byte b = bytes[i + byteIndex];
				char c = chars[i + charIndex] = ((b <= 127) ? _byteToChar[b] : _fallbackChar);
			}
			return byteCount;
		}

		public override int GetMaxByteCount(int charCount)
		{
			return charCount;
		}

		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}
	}
}
