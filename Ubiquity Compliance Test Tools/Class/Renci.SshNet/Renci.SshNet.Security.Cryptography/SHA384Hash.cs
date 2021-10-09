namespace Renci.SshNet.Security.Cryptography
{
	public class SHA384Hash : SHA2HashBase
	{
		private const int DIGEST_SIZE = 48;

		public override int HashSize => 384;

		public override int InputBlockSize => 96;

		public override int OutputBlockSize => 96;

		protected override byte[] HashFinal()
		{
			byte[] array = new byte[48];
			Finish();
			SHA2HashBase.UInt64_To_BE(H1, array, 0);
			SHA2HashBase.UInt64_To_BE(H2, array, 8);
			SHA2HashBase.UInt64_To_BE(H3, array, 16);
			SHA2HashBase.UInt64_To_BE(H4, array, 24);
			SHA2HashBase.UInt64_To_BE(H5, array, 32);
			SHA2HashBase.UInt64_To_BE(H6, array, 40);
			Initialize();
			return array;
		}

		public override void Initialize()
		{
			base.Initialize();
			H1 = 14680500436340154072uL;
			H2 = 7105036623409894663uL;
			H3 = 10473403895298186519uL;
			H4 = 1526699215303891257uL;
			H5 = 7436329637833083697uL;
			H6 = 10282925794625328401uL;
			H7 = 15784041429090275239uL;
			H8 = 5167115440072839076uL;
		}
	}
}
