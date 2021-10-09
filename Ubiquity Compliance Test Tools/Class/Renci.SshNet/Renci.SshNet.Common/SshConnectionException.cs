using Renci.SshNet.Messages.Transport;
using System;
using System.Runtime.Serialization;

namespace Renci.SshNet.Common
{
	[Serializable]
	public class SshConnectionException : SshException
	{
		public DisconnectReason DisconnectReason
		{
			get;
			private set;
		}

		public SshConnectionException()
		{
		}

		public SshConnectionException(string message)
			: base(message)
		{
			DisconnectReason = DisconnectReason.None;
		}

		public SshConnectionException(string message, DisconnectReason disconnectReasonCode)
			: base(message)
		{
			DisconnectReason = disconnectReasonCode;
		}

		public SshConnectionException(string message, DisconnectReason disconnectReasonCode, Exception inner)
			: base(message, inner)
		{
			DisconnectReason = disconnectReasonCode;
		}

		public SshConnectionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected SshConnectionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
