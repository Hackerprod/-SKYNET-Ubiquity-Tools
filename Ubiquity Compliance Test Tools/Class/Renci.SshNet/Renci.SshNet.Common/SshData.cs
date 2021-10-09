using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Renci.SshNet.Common
{
	public abstract class SshData
	{
		private static readonly Encoding _ascii = new ASCIIEncoding();

		private static readonly Encoding _utf8 = Encoding.Default;

		private List<byte> _data;

		private int _readerIndex;

		private byte[] _loadedData;

		public bool IsEndOfData => _readerIndex >= _data.Count();

		protected virtual int ZeroReaderIndex => 0;

		public virtual byte[] GetBytes()
		{
			_data = new List<byte>();
			SaveData();
			return _data.ToArray();
		}

		internal T OfType<T>() where T : SshData, new()
		{
			T result = new T();
			result.LoadBytes(_loadedData);
			result.LoadData();
			return result;
		}

		public void Load(byte[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			LoadBytes(value);
			LoadData();
		}

		protected abstract void LoadData();

		protected abstract void SaveData();

		protected void LoadBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			ResetReader();
			_loadedData = bytes;
			_data = new List<byte>(bytes);
		}

		protected void ResetReader()
		{
			_readerIndex = ZeroReaderIndex;
		}

		protected byte[] ReadBytes()
		{
			byte[] array = new byte[_data.Count - _readerIndex];
			_data.CopyTo(_readerIndex, array, 0, array.Length);
			return array;
		}

		protected byte[] ReadBytes(int length)
		{
			if (length > _data.Count)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			byte[] array = new byte[length];
			_data.CopyTo(_readerIndex, array, 0, length);
			_readerIndex += length;
			return array;
		}

		protected byte ReadByte()
		{
			return ReadBytes(1).FirstOrDefault();
		}

		protected bool ReadBoolean()
		{
			if (ReadByte() != 0)
			{
				return true;
			}
			return false;
		}

		protected ushort ReadUInt16()
		{
			byte[] array = ReadBytes(2);
			return (ushort)((array[0] << 8) | array[1]);
		}

		protected uint ReadUInt32()
		{
			byte[] array = ReadBytes(4);
			return (uint)((array[0] << 24) | (array[1] << 16) | (array[2] << 8) | array[3]);
		}

		protected ulong ReadUInt64()
		{
			byte[] array = ReadBytes(8);
			return ((ulong)array[0] << 56) | ((ulong)array[1] << 48) | ((ulong)array[2] << 40) | ((ulong)array[3] << 32) | ((ulong)array[4] << 24) | ((ulong)array[5] << 16) | ((ulong)array[6] << 8) | array[7];
		}

		protected long ReadInt64()
		{
			byte[] array = ReadBytes(8);
			return (array[0] << 24) | (array[1] << 16) | (array[2] << 8) | array[3] | (array[4] << 24) | (array[5] << 16) | (array[6] << 8) | array[7];
		}

		protected string ReadAsciiString()
		{
			uint num = ReadUInt32();
			if (num > 2147483647)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Strings longer than {0} is not supported.", new object[1]
				{
					2147483647
				}));
			}
			return _ascii.GetString(ReadBytes((int)num), 0, (int)num);
		}

		protected string ReadString()
		{
			return ReadString(_utf8);
		}

		protected string ReadString(Encoding encoding)
		{
			uint num = ReadUInt32();
			if (num > 2147483647)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Strings longer than {0} is not supported.", new object[1]
				{
					2147483647
				}));
			}
			return encoding.GetString(ReadBytes((int)num), 0, (int)num);
		}

		protected byte[] ReadBinaryString()
		{
			uint num = ReadUInt32();
			if (num > 2147483647)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Strings longer than {0} is not supported.", new object[1]
				{
					2147483647
				}));
			}
			return ReadBytes((int)num);
		}

		protected BigInteger ReadBigInt()
		{
			uint length = ReadUInt32();
			byte[] source = ReadBytes((int)length);
			return new BigInteger(source.Reverse().ToArray());
		}

		protected string[] ReadNamesList()
		{
			string text = ReadString();
			return text.Split(',');
		}

		protected IDictionary<string, string> ReadExtensionPair()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			while (_readerIndex < _data.Count)
			{
				string key = ReadString();
				string value = ReadString();
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		protected void Write(IEnumerable<byte> data)
		{
			_data.AddRange(data);
		}

		protected void Write(byte data)
		{
			_data.Add(data);
		}

		protected void Write(bool data)
		{
			if (data)
			{
				Write(1);
			}
			else
			{
				Write(0);
			}
		}

		protected void Write(ushort data)
		{
			Write(data.GetBytes());
		}

		protected void Write(uint data)
		{
			Write(data.GetBytes());
		}

		protected void Write(ulong data)
		{
			Write(data.GetBytes());
		}

		protected void Write(long data)
		{
			Write(data.GetBytes());
		}

		protected void WriteAscii(string data)
		{
			Write(data, _ascii);
		}

		protected void Write(string data)
		{
			Write(data, _utf8);
		}

		protected void Write(string data, Encoding encoding)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			byte[] bytes = encoding.GetBytes(data);
			Write((uint)bytes.Length);
			Write(bytes);
		}

		protected void WriteBinaryString(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			Write((uint)data.Length);
			_data.AddRange(data);
		}

		protected void Write(BigInteger data)
		{
			List<byte> list = data.ToByteArray().Reverse().ToList();
			Write((uint)list.Count);
			Write(list);
		}

		protected void Write(string[] data)
		{
			WriteAscii(string.Join(",", data));
		}

		protected void Write(IDictionary<string, string> data)
		{
			foreach (KeyValuePair<string, string> datum in data)
			{
				WriteAscii(datum.Key);
				WriteAscii(datum.Value);
			}
		}
	}
}
