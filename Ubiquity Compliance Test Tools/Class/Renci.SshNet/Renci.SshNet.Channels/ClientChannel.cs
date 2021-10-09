using Renci.SshNet.Common;
using Renci.SshNet.Messages.Connection;
using System;

namespace Renci.SshNet.Channels
{
	internal abstract class ClientChannel : Channel
	{
		public event EventHandler<ChannelOpenConfirmedEventArgs> OpenConfirmed;

		public event EventHandler<ChannelOpenFailedEventArgs> OpenFailed;

		internal override void Initialize(Session session, uint localWindowSize, uint localPacketSize)
		{
			base.Initialize(session, localWindowSize, localPacketSize);
			base.Session.ChannelOpenConfirmationReceived += OnChannelOpenConfirmation;
			base.Session.ChannelOpenFailureReceived += OnChannelOpenFailure;
		}

		protected virtual void OnOpenConfirmation(uint remoteChannelNumber, uint initialWindowSize, uint maximumPacketSize)
		{
			InitializeRemoteInfo(remoteChannelNumber, initialWindowSize, maximumPacketSize);
			base.IsOpen = true;
			this.OpenConfirmed?.Invoke(this, new ChannelOpenConfirmedEventArgs(remoteChannelNumber, initialWindowSize, maximumPacketSize));
		}

		protected void SendMessage(ChannelOpenMessage message)
		{
			base.Session.SendMessage(message);
		}

		protected virtual void OnOpenFailure(uint reasonCode, string description, string language)
		{
			this.OpenFailed?.Invoke(this, new ChannelOpenFailedEventArgs(base.LocalChannelNumber, reasonCode, description, language));
		}

		private void OnChannelOpenConfirmation(object sender, MessageEventArgs<ChannelOpenConfirmationMessage> e)
		{
			if (e.Message.LocalChannelNumber == base.LocalChannelNumber)
			{
				OnOpenConfirmation(e.Message.RemoteChannelNumber, e.Message.InitialWindowSize, e.Message.MaximumPacketSize);
			}
		}

		private void OnChannelOpenFailure(object sender, MessageEventArgs<ChannelOpenFailureMessage> e)
		{
			if (e.Message.LocalChannelNumber == base.LocalChannelNumber)
			{
				OnOpenFailure(e.Message.ReasonCode, e.Message.Description, e.Message.Language);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Session session = base.Session;
				if (session != null)
				{
					session.ChannelOpenConfirmationReceived -= OnChannelOpenConfirmation;
					session.ChannelOpenFailureReceived -= OnChannelOpenFailure;
				}
			}
		}
	}
}
