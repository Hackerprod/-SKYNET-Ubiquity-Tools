using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Transport;
using System;
using System.Globalization;
using System.Linq;

namespace Renci.SshNet.Security
{
	public class KeyExchangeEllipticCurveDiffieHellman : KeyExchange
	{
		private class _ExchangeHashData : SshData
		{
			public string ServerVersion
			{
				get;
				set;
			}

			public string ClientVersion
			{
				get;
				set;
			}

			public byte[] ClientPayload
			{
				get;
				set;
			}

			public byte[] ServerPayload
			{
				get;
				set;
			}

			public byte[] HostKey
			{
				get;
				set;
			}

			public uint MinimumGroupSize
			{
				get;
				set;
			}

			public uint PreferredGroupSize
			{
				get;
				set;
			}

			public uint MaximumGroupSize
			{
				get;
				set;
			}

			public BigInteger Prime
			{
				get;
				set;
			}

			public BigInteger SubGroup
			{
				get;
				set;
			}

			public BigInteger ClientExchangeValue
			{
				get;
				set;
			}

			public BigInteger ServerExchangeValue
			{
				get;
				set;
			}

			public BigInteger SharedKey
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
				Write(ClientVersion);
				Write(ServerVersion);
				WriteBinaryString(ClientPayload);
				WriteBinaryString(ServerPayload);
				WriteBinaryString(HostKey);
				Write(MinimumGroupSize);
				Write(PreferredGroupSize);
				Write(MaximumGroupSize);
				Write(Prime);
				Write(SubGroup);
				Write(ClientExchangeValue);
				Write(ServerExchangeValue);
				Write(SharedKey);
			}
		}

		protected byte[] _clientPayload;

		protected byte[] _serverPayload;

		protected BigInteger _clientExchangeValue;

		protected BigInteger _serverExchangeValue;

		protected BigInteger _randomValue;

		protected byte[] _hostKey;

		protected byte[] _signature;

		public override string Name => "ecdh-sha2-nistp256";

		public override void Start(Session session, KeyExchangeInitMessage message)
		{
			base.Start(session, message);
			_serverPayload = message.GetBytes().ToArray();
			_clientPayload = base.Session.ClientInitMessage.GetBytes().ToArray();
			base.Session.RegisterMessage("SSH_MSG_KEXECDH_REPLY");
			base.Session.MessageReceived += Session_MessageReceived;
			BigInteger.TryParse("00FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF", NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out BigInteger _);
			BigInteger.TryParse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC7634D81F4372DDF581A0DB248B0A77AECEC196ACCC52973", NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out BigInteger result2);
			BigInteger.TryParse("00036B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C296", NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out BigInteger result3);
			BigInteger bigInteger;
			do
			{
				bigInteger = BigInteger.Random(result2.BitLength);
			}
			while (bigInteger < 1L || bigInteger > result2);
			BigInteger q = bigInteger * result3;
			SendMessage(new KeyExchangeEcdhInitMessage(bigInteger, q));
		}

		private void Session_MessageReceived(object sender, MessageEventArgs<Message> e)
		{
			KeyExchangeEcdhReplyMessage keyExchangeEcdhReplyMessage = e.Message as KeyExchangeEcdhReplyMessage;
			if (keyExchangeEcdhReplyMessage != null)
			{
				base.Session.UnRegisterMessage("SSH_MSG_KEXECDH_REPLY");
				HandleServerEcdhReply();
				Finish();
			}
		}

		protected override bool ValidateExchangeHash()
		{
			return false;
		}

		protected virtual void HandleServerEcdhReply()
		{
		}

		protected override byte[] CalculateHash()
		{
			_ExchangeHashData exchangeHashData = new _ExchangeHashData();
			exchangeHashData.ClientVersion = base.Session.ClientVersion;
			exchangeHashData.ServerVersion = base.Session.ServerVersion;
			exchangeHashData.ClientPayload = _clientPayload;
			exchangeHashData.ServerPayload = _serverPayload;
			exchangeHashData.HostKey = _hostKey;
			exchangeHashData.SharedKey = base.SharedKey;
			byte[] bytes = exchangeHashData.GetBytes();
			return Hash(bytes);
		}
	}
}
