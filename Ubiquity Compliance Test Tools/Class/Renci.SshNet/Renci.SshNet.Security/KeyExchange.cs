using Renci.SshNet.Common;
using Renci.SshNet.Compression;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Transport;
using Renci.SshNet.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Renci.SshNet.Security
{
	public abstract class KeyExchange : Algorithm, IDisposable
	{
		private class _SessionKeyGeneration : SshData
		{
			public BigInteger SharedKey
			{
				get;
				set;
			}

			public byte[] ExchangeHash
			{
				get;
				set;
			}

			public char Char
			{
				get;
				set;
			}

			public byte[] SessionId
			{
				get;
				set;
			}

			protected override void LoadData()
			{
				throw new NotImplementedException();
			}

			protected override void SaveData()
			{
				Write(SharedKey);
				Write(ExchangeHash);
				Write((byte)Char);
				Write(SessionId);
			}
		}

		private class _SessionKeyAdjustment : SshData
		{
			public BigInteger SharedKey
			{
				get;
				set;
			}

			public byte[] ExcahngeHash
			{
				get;
				set;
			}

			public byte[] Key
			{
				get;
				set;
			}

			protected override void LoadData()
			{
				throw new NotImplementedException();
			}

			protected override void SaveData()
			{
				Write(SharedKey);
				Write(ExcahngeHash);
				Write(Key);
			}
		}

		private CipherInfo _clientCipherInfo;

		private CipherInfo _serverCipherInfo;

		private HashInfo _clientHashInfo;

		private HashInfo _serverHashInfo;

		private Type _compressionType;

		private Type _decompressionType;

		private byte[] _exchangeHash;

		protected Session Session
		{
			get;
			private set;
		}

		public BigInteger SharedKey
		{
			get;
			protected set;
		}

		public byte[] ExchangeHash
		{
			get
			{
				if (_exchangeHash == null)
				{
					_exchangeHash = CalculateHash();
				}
				return _exchangeHash;
			}
		}

		public event EventHandler<HostKeyEventArgs> HostKeyReceived;

		public virtual void Start(Session session, KeyExchangeInitMessage message)
		{
			Session = session;
			SendMessage(session.ClientInitMessage);
			string text = (from b in session.ConnectionInfo.Encryptions.Keys
			from a in message.EncryptionAlgorithmsClientToServer
			where a == b
			select a).FirstOrDefault();
			if (string.IsNullOrEmpty(text))
			{
				throw new SshConnectionException("Client encryption algorithm not found", DisconnectReason.KeyExchangeFailed);
			}
			session.ConnectionInfo.CurrentClientEncryption = text;
			string text2 = (from b in session.ConnectionInfo.Encryptions.Keys
			from a in message.EncryptionAlgorithmsServerToClient
			where a == b
			select a).FirstOrDefault();
			if (string.IsNullOrEmpty(text2))
			{
				throw new SshConnectionException("Server decryption algorithm not found", DisconnectReason.KeyExchangeFailed);
			}
			session.ConnectionInfo.CurrentServerEncryption = text2;
			string text3 = (from b in session.ConnectionInfo.HmacAlgorithms.Keys
			from a in message.MacAlgorithmsClientToServer
			where a == b
			select a).FirstOrDefault();
			if (string.IsNullOrEmpty(text3))
			{
				throw new SshConnectionException("Server HMAC algorithm not found", DisconnectReason.KeyExchangeFailed);
			}
			session.ConnectionInfo.CurrentClientHmacAlgorithm = text3;
			string text4 = (from b in session.ConnectionInfo.HmacAlgorithms.Keys
			from a in message.MacAlgorithmsServerToClient
			where a == b
			select a).FirstOrDefault();
			if (string.IsNullOrEmpty(text4))
			{
				throw new SshConnectionException("Server HMAC algorithm not found", DisconnectReason.KeyExchangeFailed);
			}
			session.ConnectionInfo.CurrentServerHmacAlgorithm = text4;
			string text5 = (from b in session.ConnectionInfo.CompressionAlgorithms.Keys
			from a in message.CompressionAlgorithmsClientToServer
			where a == b
			select a).LastOrDefault();
			if (string.IsNullOrEmpty(text5))
			{
				throw new SshConnectionException("Compression algorithm not found", DisconnectReason.KeyExchangeFailed);
			}
			session.ConnectionInfo.CurrentClientCompressionAlgorithm = text5;
			string text6 = (from b in session.ConnectionInfo.CompressionAlgorithms.Keys
			from a in message.CompressionAlgorithmsServerToClient
			where a == b
			select a).LastOrDefault();
			if (string.IsNullOrEmpty(text6))
			{
				throw new SshConnectionException("Decompression algorithm not found", DisconnectReason.KeyExchangeFailed);
			}
			session.ConnectionInfo.CurrentServerCompressionAlgorithm = text6;
			_clientCipherInfo = session.ConnectionInfo.Encryptions[text];
			_serverCipherInfo = session.ConnectionInfo.Encryptions[text2];
			_clientHashInfo = session.ConnectionInfo.HmacAlgorithms[text3];
			_serverHashInfo = session.ConnectionInfo.HmacAlgorithms[text4];
			_compressionType = session.ConnectionInfo.CompressionAlgorithms[text5];
			_decompressionType = session.ConnectionInfo.CompressionAlgorithms[text6];
		}

		public virtual void Finish()
		{
			if (ValidateExchangeHash())
			{
				SendMessage(new NewKeysMessage());
				return;
			}
			throw new SshConnectionException("Key exchange negotiation failed.", DisconnectReason.KeyExchangeFailed);
		}

		public Cipher CreateServerCipher()
		{
			byte[] sessionId = Session.SessionId ?? ExchangeHash;
			byte[] arg = Hash(GenerateSessionKey(SharedKey, ExchangeHash, 'B', sessionId));
			byte[] key = Hash(GenerateSessionKey(SharedKey, ExchangeHash, 'D', sessionId));
			key = GenerateSessionKey(SharedKey, ExchangeHash, key, _serverCipherInfo.KeySize / 8);
			return _serverCipherInfo.Cipher(key, arg);
		}

		public Cipher CreateClientCipher()
		{
			byte[] sessionId = Session.SessionId ?? ExchangeHash;
			byte[] arg = Hash(GenerateSessionKey(SharedKey, ExchangeHash, 'A', sessionId));
			byte[] key = Hash(GenerateSessionKey(SharedKey, ExchangeHash, 'C', sessionId));
			key = GenerateSessionKey(SharedKey, ExchangeHash, key, _clientCipherInfo.KeySize / 8);
			return _clientCipherInfo.Cipher(key, arg);
		}

		public HashAlgorithm CreateServerHash()
		{
			byte[] sessionId = Session.SessionId ?? ExchangeHash;
			byte[] key = Hash(GenerateSessionKey(SharedKey, ExchangeHash, 'F', sessionId));
			key = GenerateSessionKey(SharedKey, ExchangeHash, key, _serverHashInfo.KeySize / 8);
			return _serverHashInfo.HashAlgorithm(key);
		}

		public HashAlgorithm CreateClientHash()
		{
			byte[] sessionId = Session.SessionId ?? ExchangeHash;
			byte[] key = Hash(GenerateSessionKey(SharedKey, ExchangeHash, 'E', sessionId));
			key = GenerateSessionKey(SharedKey, ExchangeHash, key, _clientHashInfo.KeySize / 8);
			return _clientHashInfo.HashAlgorithm(key);
		}

		public Compressor CreateCompressor()
		{
			if ((object)_compressionType == null)
			{
				return null;
			}
			Compressor compressor = _compressionType.CreateInstance<Compressor>();
			compressor.Init(Session);
			return compressor;
		}

		public Compressor CreateDecompressor()
		{
			if ((object)_compressionType == null)
			{
				return null;
			}
			Compressor compressor = _decompressionType.CreateInstance<Compressor>();
			compressor.Init(Session);
			return compressor;
		}

		protected bool CanTrustHostKey(KeyHostAlgorithm host)
		{
			HostKeyEventArgs hostKeyEventArgs = new HostKeyEventArgs(host);
			if (this.HostKeyReceived != null)
			{
				this.HostKeyReceived(this, hostKeyEventArgs);
			}
			return hostKeyEventArgs.CanTrust;
		}

		protected abstract bool ValidateExchangeHash();

		protected abstract byte[] CalculateHash();

		protected virtual byte[] Hash(byte[] hashData)
		{
			using (SHA1Hash sHA1Hash = new SHA1Hash())
			{
				return sHA1Hash.ComputeHash(hashData, 0, hashData.Length);
			}
		}

		protected void SendMessage(Message message)
		{
			Session.SendMessage(message);
		}

		private byte[] GenerateSessionKey(BigInteger sharedKey, byte[] exchangeHash, byte[] key, int size)
		{
			List<byte> list = new List<byte>(key);
			while (size > list.Count)
			{
				list.AddRange(Hash(new _SessionKeyAdjustment
				{
					SharedKey = sharedKey,
					ExcahngeHash = exchangeHash,
					Key = key
				}.GetBytes()));
			}
			return list.ToArray();
		}

		private byte[] GenerateSessionKey(BigInteger sharedKey, byte[] exchangeHash, char p, byte[] sessionId)
		{
			_SessionKeyGeneration sessionKeyGeneration = new _SessionKeyGeneration();
			sessionKeyGeneration.SharedKey = sharedKey;
			sessionKeyGeneration.ExchangeHash = exchangeHash;
			sessionKeyGeneration.Char = p;
			sessionKeyGeneration.SessionId = sessionId;
			return sessionKeyGeneration.GetBytes();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		~KeyExchange()
		{
			Dispose(disposing: false);
		}
	}
}
