namespace Renci.SshNet.Messages.Authentication
{
	internal class RequestMessagePassword : RequestMessage
	{
		public override string MethodName => "password";

		public byte[] Password
		{
			get;
			private set;
		}

		public byte[] NewPassword
		{
			get;
			private set;
		}

		public RequestMessagePassword(ServiceName serviceName, string username, byte[] password)
			: base(serviceName, username)
		{
			Password = password;
		}

		public RequestMessagePassword(ServiceName serviceName, string username, byte[] password, byte[] newPassword)
			: this(serviceName, username, password)
		{
			NewPassword = newPassword;
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(NewPassword != null);
			Write((uint)Password.Length);
			Write(Password);
			if (NewPassword != null)
			{
				Write((uint)NewPassword.Length);
				Write(NewPassword);
			}
		}
	}
}
