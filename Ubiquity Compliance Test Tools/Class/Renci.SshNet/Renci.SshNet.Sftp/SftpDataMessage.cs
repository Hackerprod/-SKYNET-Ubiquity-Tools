using Renci.SshNet.Messages.Connection;

namespace Renci.SshNet.Sftp
{
	internal class SftpDataMessage : ChannelDataMessage
	{
		public SftpDataMessage(uint localChannelNumber, SftpMessage sftpMessage)
		{
			base.LocalChannelNumber = localChannelNumber;
			byte[] bytes = sftpMessage.GetBytes();
			byte[] array = new byte[4 + bytes.Length];
			((uint)bytes.Length).GetBytes().CopyTo(array, 0);
			bytes.CopyTo(array, 4);
			base.Data = array;
		}
	}
}
