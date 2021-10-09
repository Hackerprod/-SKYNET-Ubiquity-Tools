using System;
using System.Collections.Generic;

namespace Renci.SshNet
{
	public abstract class AuthenticationMethod
	{
		public abstract string Name
		{
			get;
		}

		public string Username
		{
			get;
			private set;
		}

		public string ErrorMessage
		{
			get;
			private set;
		}

		public IEnumerable<string> AllowedAuthentications
		{
			get;
			protected set;
		}

		protected AuthenticationMethod(string username)
		{
			if (username.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("username");
			}
			Username = username;
		}

		public abstract AuthenticationResult Authenticate(Session session);
	}
}
