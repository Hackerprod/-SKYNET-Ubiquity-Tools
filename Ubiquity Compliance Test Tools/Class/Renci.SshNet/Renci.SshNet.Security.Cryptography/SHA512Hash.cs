namespace Renci.SshNet.Security.Cryptography
{
	public class SHA512Hash : SHA2HashBase
	{
		private const int DIGEST_SIZE = 64;

		public override int HashSize => 512;

		public override int InputBlockSize => 128;

		public override int OutputBlockSize => 128;

		protected override byte[] HashFinal()
		{
			byte[] array = new byte[64];
			Finish();
			SHA2HashBase.UInt64_To_BE(H1, array, 0);
			SHA2HashBase.UInt64_To_BE(H2, array, 8);
			SHA2HashBase.UInt64_To_BE(H3, array, 16);
			SHA2HashBase.UInt64_To_BE(H4, array, 24);
			SHA2HashBase.UInt64_To_BE(H5, array, 32);
			SHA2HashBase.UInt64_To_BE(H6, array, 40);
			SHA2HashBase.UInt64_To_BE(H7, array, 48);
			SHA2HashBase.UInt64_To_BE(H8, array, 56);
			Initialize();
			return array;
		}

		public override void Initialize()
		{
			base.Initialize();
			H1 = 7640891576956012808uL;
			H2 = 13503953896175478587uL;
			H3 = 4354685564936845355uL;
			H4 = 11912009170470909681uL;
			H5 = 5840696475078001361uL;
			H6 = 11170449401992604703uL;
			H7 = 2270897969802886507uL;
			H8 = 6620516959819538809uL;
		}
	}
}
