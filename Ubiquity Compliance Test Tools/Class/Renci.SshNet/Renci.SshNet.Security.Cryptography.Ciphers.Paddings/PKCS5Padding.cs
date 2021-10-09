using System;

namespace Renci.SshNet.Security.Cryptography.Ciphers.Paddings
{
	public class PKCS5Padding : CipherPadding
	{
		public override byte[] Pad(int blockSize, byte[] input)
		{
			int num = blockSize - input.Length % blockSize;
			byte[] array = new byte[input.Length + num];
			Buffer.BlockCopy(input, 0, array, 0, input.Length);
			for (int i = 0; i < num; i++)
			{
				array[input.Length + i] = (byte)num;
			}
			return array;
		}
	}
}
