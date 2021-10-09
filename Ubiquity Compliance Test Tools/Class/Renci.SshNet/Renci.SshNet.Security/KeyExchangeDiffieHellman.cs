using Renci.SshNet.Common;
using Renci.SshNet.Messages.Transport;
using System;
using System.Linq;
using System.Text;

namespace Renci.SshNet.Security
{
	public abstract class KeyExchangeDiffieHellman : KeyExchange
	{
		protected BigInteger _group;

		protected BigInteger _prime;

		protected byte[] _clientPayload;

		protected byte[] _serverPayload;

		protected BigInteger _clientExchangeValue;

		protected BigInteger _serverExchangeValue;

		protected BigInteger _randomValue;

		protected byte[] _hostKey;

		protected byte[] _signature;

		protected override bool ValidateExchangeHash()
		{
			byte[] data = CalculateHash();
			uint count = (uint)((_hostKey[0] << 24) | (_hostKey[1] << 16) | (_hostKey[2] << 8) | _hostKey[3]);
			string @string = Encoding.UTF8.GetString(_hostKey, 4, (int)count);
			KeyHostAlgorithm keyHostAlgorithm = base.Session.ConnectionInfo.HostKeyAlgorithms[@string](_hostKey);
			base.Session.ConnectionInfo.CurrentHostKeyAlgorithm = @string;
			if (CanTrustHostKey(keyHostAlgorithm))
			{
				return keyHostAlgorithm.VerifySignature(data, _signature);
			}
			return false;
		}

		public override void Start(Session session, KeyExchangeInitMessage message)
		{
			base.Start(session, message);
			_serverPayload = message.GetBytes().ToArray();
			_clientPayload = base.Session.ClientInitMessage.GetBytes().ToArray();
		}

		protected void PopulateClientExchangeValue()
		{
			if (_group.IsZero)
			{
				throw new ArgumentNullException("_group");
			}
			if (_prime.IsZero)
			{
				throw new ArgumentNullException("_prime");
			}
			int bitLength = _prime.BitLength;
			do
			{
				_randomValue = BigInteger.Random(bitLength);
				_clientExchangeValue = BigInteger.ModPow(_group, _randomValue, _prime);
			}
			while (_clientExchangeValue < 1L || _clientExchangeValue > _prime - 1);
		}

		protected virtual void HandleServerDhReply(byte[] hostKey, BigInteger serverExchangeValue, byte[] signature)
		{
			_serverExchangeValue = serverExchangeValue;
			_hostKey = hostKey;
			base.SharedKey = BigInteger.ModPow(serverExchangeValue, _randomValue, _prime);
			_signature = signature;
		}
	}
}
