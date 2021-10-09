using System.Security.Cryptography;

namespace Renci.SshNet.Messages.Transport
{
	[Message("SSH_MSG_KEXINIT", 20)]
	public class KeyExchangeInitMessage : Message, IKeyExchangedAllowed
	{
		private static readonly RNGCryptoServiceProvider _randomizer = new RNGCryptoServiceProvider();

		public byte[] Cookie
		{
			get;
			private set;
		}

		public string[] KeyExchangeAlgorithms
		{
			get;
			set;
		}

		public string[] ServerHostKeyAlgorithms
		{
			get;
			set;
		}

		public string[] EncryptionAlgorithmsClientToServer
		{
			get;
			set;
		}

		public string[] EncryptionAlgorithmsServerToClient
		{
			get;
			set;
		}

		public string[] MacAlgorithmsClientToServer
		{
			get;
			set;
		}

		public string[] MacAlgorithmsServerToClient
		{
			get;
			set;
		}

		public string[] CompressionAlgorithmsClientToServer
		{
			get;
			set;
		}

		public string[] CompressionAlgorithmsServerToClient
		{
			get;
			set;
		}

		public string[] LanguagesClientToServer
		{
			get;
			set;
		}

		public string[] LanguagesServerToClient
		{
			get;
			set;
		}

		public bool FirstKexPacketFollows
		{
			get;
			set;
		}

		public uint Reserved
		{
			get;
			set;
		}

		public KeyExchangeInitMessage()
		{
			byte[] array = new byte[16];
			_randomizer.GetBytes(array);
			Cookie = array;
		}

		protected override void LoadData()
		{
			ResetReader();
			Cookie = ReadBytes(16);
			KeyExchangeAlgorithms = ReadNamesList();
			ServerHostKeyAlgorithms = ReadNamesList();
			EncryptionAlgorithmsClientToServer = ReadNamesList();
			EncryptionAlgorithmsServerToClient = ReadNamesList();
			MacAlgorithmsClientToServer = ReadNamesList();
			MacAlgorithmsServerToClient = ReadNamesList();
			CompressionAlgorithmsClientToServer = ReadNamesList();
			CompressionAlgorithmsServerToClient = ReadNamesList();
			LanguagesClientToServer = ReadNamesList();
			LanguagesServerToClient = ReadNamesList();
			FirstKexPacketFollows = ReadBoolean();
			Reserved = ReadUInt32();
		}

		protected override void SaveData()
		{
			Write(Cookie);
			Write(KeyExchangeAlgorithms);
			Write(ServerHostKeyAlgorithms);
			Write(EncryptionAlgorithmsClientToServer);
			Write(EncryptionAlgorithmsServerToClient);
			Write(MacAlgorithmsClientToServer);
			Write(MacAlgorithmsServerToClient);
			Write(CompressionAlgorithmsClientToServer);
			Write(CompressionAlgorithmsServerToClient);
			Write(LanguagesClientToServer);
			Write(LanguagesServerToClient);
			Write(FirstKexPacketFollows);
			Write(Reserved);
		}
	}
}
