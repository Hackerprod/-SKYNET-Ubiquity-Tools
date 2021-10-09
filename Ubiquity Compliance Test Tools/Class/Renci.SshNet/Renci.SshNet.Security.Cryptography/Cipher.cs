namespace Renci.SshNet.Security.Cryptography
{
	public abstract class Cipher
	{
		public abstract byte MinimumSize
		{
			get;
		}

		public abstract byte[] Encrypt(byte[] input);

		public abstract byte[] Decrypt(byte[] input);

		protected static void UInt32ToBigEndian(uint number, byte[] buffer)
		{
			buffer[0] = (byte)(number >> 24);
			buffer[1] = (byte)(number >> 16);
			buffer[2] = (byte)(number >> 8);
			buffer[3] = (byte)number;
		}

		protected static void UInt32ToBigEndian(uint number, byte[] buffer, int offset)
		{
			buffer[offset] = (byte)(number >> 24);
			buffer[offset + 1] = (byte)(number >> 16);
			buffer[offset + 2] = (byte)(number >> 8);
			buffer[offset + 3] = (byte)number;
		}

		protected static uint BigEndianToUInt32(byte[] buffer)
		{
			uint num = (uint)(buffer[0] << 24);
			num = (uint)((int)num | (buffer[1] << 16));
			num = (uint)((int)num | (buffer[2] << 8));
			return num | buffer[3];
		}

		protected static uint BigEndianToUInt32(byte[] buffer, int offset)
		{
			uint num = (uint)(buffer[offset] << 24);
			num = (uint)((int)num | (buffer[offset + 1] << 16));
			num = (uint)((int)num | (buffer[offset + 2] << 8));
			return num | buffer[offset + 3];
		}

		protected static ulong BigEndianToUInt64(byte[] buffer)
		{
			uint num = BigEndianToUInt32(buffer);
			uint num2 = BigEndianToUInt32(buffer, 4);
			return ((ulong)num << 32) | num2;
		}

		protected static ulong BigEndianToUInt64(byte[] buffer, int offset)
		{
			uint num = BigEndianToUInt32(buffer, offset);
			uint num2 = BigEndianToUInt32(buffer, offset + 4);
			return ((ulong)num << 32) | num2;
		}

		protected static void UInt64ToBigEndian(ulong number, byte[] buffer)
		{
			UInt32ToBigEndian((uint)(number >> 32), buffer);
			UInt32ToBigEndian((uint)number, buffer, 4);
		}

		protected static void UInt64ToBigEndian(ulong number, byte[] buffer, int offset)
		{
			UInt32ToBigEndian((uint)(number >> 32), buffer, offset);
			UInt32ToBigEndian((uint)number, buffer, offset + 4);
		}

		protected static void UInt32ToLittleEndian(uint number, byte[] buffer)
		{
			buffer[0] = (byte)number;
			buffer[1] = (byte)(number >> 8);
			buffer[2] = (byte)(number >> 16);
			buffer[3] = (byte)(number >> 24);
		}

		protected static void UInt32ToLittleEndian(uint number, byte[] buffer, int offset)
		{
			buffer[offset] = (byte)number;
			buffer[offset + 1] = (byte)(number >> 8);
			buffer[offset + 2] = (byte)(number >> 16);
			buffer[offset + 3] = (byte)(number >> 24);
		}

		protected static uint LittleEndianToUInt32(byte[] buffer)
		{
			uint num = buffer[0];
			num = (uint)((int)num | (buffer[1] << 8));
			num = (uint)((int)num | (buffer[2] << 16));
			return (uint)((int)num | (buffer[3] << 24));
		}

		protected static uint LittleEndianToUInt32(byte[] buffer, int offset)
		{
			uint num = buffer[offset];
			num = (uint)((int)num | (buffer[offset + 1] << 8));
			num = (uint)((int)num | (buffer[offset + 2] << 16));
			return (uint)((int)num | (buffer[offset + 3] << 24));
		}

		protected static ulong LittleEndianToUInt64(byte[] buffer)
		{
			uint num = LittleEndianToUInt32(buffer);
			uint num2 = LittleEndianToUInt32(buffer, 4);
			return ((ulong)num2 << 32) | num;
		}

		protected static ulong LittleEndianToUInt64(byte[] buffer, int offset)
		{
			uint num = LittleEndianToUInt32(buffer, offset);
			uint num2 = LittleEndianToUInt32(buffer, offset + 4);
			return ((ulong)num2 << 32) | num;
		}

		protected static void UInt64ToLittleEndian(ulong number, byte[] buffer)
		{
			UInt32ToLittleEndian((uint)number, buffer);
			UInt32ToLittleEndian((uint)(number >> 32), buffer, 4);
		}

		protected static void UInt64ToLittleEndian(ulong number, byte[] buffer, int offset)
		{
			UInt32ToLittleEndian((uint)number, buffer, offset);
			UInt32ToLittleEndian((uint)(number >> 32), buffer, offset + 4);
		}
	}
}
