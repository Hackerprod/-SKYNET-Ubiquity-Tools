using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Transport;
using System;

namespace Renci.SshNet.Security
{
	internal class KeyExchangeDiffieHellmanGroupExchangeSha1 : KeyExchangeDiffieHellman
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

		public override string Name => "diffie-hellman-group-exchange-sha1";

		protected override byte[] CalculateHash()
		{
			_ExchangeHashData exchangeHashData = new _ExchangeHashData();
			exchangeHashData.ClientVersion = base.Session.ClientVersion;
			exchangeHashData.ServerVersion = base.Session.ServerVersion;
			exchangeHashData.ClientPayload = _clientPayload;
			exchangeHashData.ServerPayload = _serverPayload;
			exchangeHashData.HostKey = _hostKey;
			exchangeHashData.MinimumGroupSize = 1024u;
			exchangeHashData.PreferredGroupSize = 1024u;
			exchangeHashData.MaximumGroupSize = 1024u;
			exchangeHashData.Prime = _prime;
			exchangeHashData.SubGroup = _group;
			exchangeHashData.ClientExchangeValue = _clientExchangeValue;
			exchangeHashData.ServerExchangeValue = _serverExchangeValue;
			exchangeHashData.SharedKey = base.SharedKey;
			byte[] bytes = exchangeHashData.GetBytes();
			return Hash(bytes);
		}

		public override void Start(Session session, KeyExchangeInitMessage message)
		{
			base.Start(session, message);
			base.Session.RegisterMessage("SSH_MSG_KEX_DH_GEX_GROUP");
			base.Session.RegisterMessage("SSH_MSG_KEX_DH_GEX_REPLY");
			base.Session.MessageReceived += Session_MessageReceived;
			SendMessage(new KeyExchangeDhGroupExchangeRequest(1024u, 1024u, 1024u));
		}

		public override void Finish()
		{
			base.Finish();
			base.Session.MessageReceived -= Session_MessageReceived;
		}

		private void Session_MessageReceived(object sender, MessageEventArgs<Message> e)
		{
			KeyExchangeDhGroupExchangeGroup keyExchangeDhGroupExchangeGroup = e.Message as KeyExchangeDhGroupExchangeGroup;
			if (keyExchangeDhGroupExchangeGroup != null)
			{
				base.Session.UnRegisterMessage("SSH_MSG_KEX_DH_GEX_GROUP");
				_prime = keyExchangeDhGroupExchangeGroup.SafePrime;
				_group = keyExchangeDhGroupExchangeGroup.SubGroup;
				PopulateClientExchangeValue();
				SendMessage(new KeyExchangeDhGroupExchangeInit(_clientExchangeValue));
			}
			KeyExchangeDhGroupExchangeReply keyExchangeDhGroupExchangeReply = e.Message as KeyExchangeDhGroupExchangeReply;
			if (keyExchangeDhGroupExchangeReply != null)
			{
				base.Session.UnRegisterMessage("SSH_MSG_KEX_DH_GEX_REPLY");
				HandleServerDhReply(keyExchangeDhGroupExchangeReply.HostKey, keyExchangeDhGroupExchangeReply.F, keyExchangeDhGroupExchangeReply.Signature);
				Finish();
			}
		}
	}
}
