using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Transport;
using System;

namespace Renci.SshNet.Security
{
	public abstract class KeyExchangeDiffieHellmanGroupSha1 : KeyExchangeDiffieHellman
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
				Write(ClientExchangeValue);
				Write(ServerExchangeValue);
				Write(SharedKey);
			}
		}

		public abstract BigInteger GroupPrime
		{
			get;
		}

		protected override byte[] CalculateHash()
		{
			_ExchangeHashData exchangeHashData = new _ExchangeHashData();
			exchangeHashData.ClientVersion = base.Session.ClientVersion;
			exchangeHashData.ServerVersion = base.Session.ServerVersion;
			exchangeHashData.ClientPayload = _clientPayload;
			exchangeHashData.ServerPayload = _serverPayload;
			exchangeHashData.HostKey = _hostKey;
			exchangeHashData.ClientExchangeValue = _clientExchangeValue;
			exchangeHashData.ServerExchangeValue = _serverExchangeValue;
			exchangeHashData.SharedKey = base.SharedKey;
			byte[] bytes = exchangeHashData.GetBytes();
			return Hash(bytes);
		}

		public override void Start(Session session, KeyExchangeInitMessage message)
		{
			base.Start(session, message);
			base.Session.RegisterMessage("SSH_MSG_KEXDH_REPLY");
			base.Session.MessageReceived += Session_MessageReceived;
			_prime = GroupPrime;
			_group = new BigInteger(new byte[1]
			{
				2
			});
			PopulateClientExchangeValue();
			SendMessage(new KeyExchangeDhInitMessage(_clientExchangeValue));
		}

		public override void Finish()
		{
			base.Finish();
			base.Session.MessageReceived -= Session_MessageReceived;
		}

		private void Session_MessageReceived(object sender, MessageEventArgs<Message> e)
		{
			KeyExchangeDhReplyMessage keyExchangeDhReplyMessage = e.Message as KeyExchangeDhReplyMessage;
			if (keyExchangeDhReplyMessage != null)
			{
				base.Session.UnRegisterMessage("SSH_MSG_KEXDH_REPLY");
				HandleServerDhReply(keyExchangeDhReplyMessage.HostKey, keyExchangeDhReplyMessage.F, keyExchangeDhReplyMessage.Signature);
				Finish();
			}
		}
	}
}
