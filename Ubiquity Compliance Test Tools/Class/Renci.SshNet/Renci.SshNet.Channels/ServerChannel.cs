using Renci.SshNet.Messages.Connection;

namespace Renci.SshNet.Channels
{
	internal abstract class ServerChannel : Channel
	{
		internal void Initialize(Session session, uint localWindowSize, uint localPacketSize, uint remoteChannelNumber, uint remoteWindowSize, uint remotePacketSize)
		{
			Initialize(session, localWindowSize, localPacketSize);
			InitializeRemoteInfo(remoteChannelNumber, remoteWindowSize, remotePacketSize);
		}

		protected void SendMessage(ChannelOpenConfirmationMessage message)
		{
			base.Session.SendMessage(message);
			base.IsOpen = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Session session = base.Session;
			}
			base.Dispose(disposing);
		}
	}
}
