namespace Renci.SshNet.Messages.Authentication
{
	[Message("SSH_MSG_USERAUTH_SUCCESS", 52)]
	public class SuccessMessage : Message
	{
		protected override void LoadData()
		{
		}

		protected override void SaveData()
		{
		}
	}
}
