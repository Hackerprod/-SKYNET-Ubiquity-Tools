using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Authentication;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace Renci.SshNet
{
	public class PasswordAuthenticationMethod : AuthenticationMethod, IDisposable
	{
		private AuthenticationResult _authenticationResult = AuthenticationResult.Failure;

		private Session _session;

		private EventWaitHandle _authenticationCompleted = new AutoResetEvent(initialState: false);

		private Exception _exception;

		private readonly RequestMessage _requestMessage;

		private readonly byte[] _password;

		private bool _isDisposed;

		public override string Name => _requestMessage.MethodName;

		public event EventHandler<AuthenticationPasswordChangeEventArgs> PasswordExpired;

		public PasswordAuthenticationMethod(string username, string password)
			: this(username, Encoding.UTF8.GetBytes(password))
		{
		}

		public PasswordAuthenticationMethod(string username, byte[] password)
			: base(username)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			_password = password;
			_requestMessage = new RequestMessagePassword(ServiceName.Connection, base.Username, _password);
		}

		public override AuthenticationResult Authenticate(Session session)
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			_session = session;
			session.UserAuthenticationSuccessReceived += Session_UserAuthenticationSuccessReceived;
			session.UserAuthenticationFailureReceived += Session_UserAuthenticationFailureReceived;
			session.MessageReceived += Session_MessageReceived;
			session.RegisterMessage("SSH_MSG_USERAUTH_PASSWD_CHANGEREQ");
			session.SendMessage(_requestMessage);
			session.WaitOnHandle(_authenticationCompleted);
			session.UserAuthenticationSuccessReceived -= Session_UserAuthenticationSuccessReceived;
			session.UserAuthenticationFailureReceived -= Session_UserAuthenticationFailureReceived;
			session.MessageReceived -= Session_MessageReceived;
			if (_exception != null)
			{
				throw _exception;
			}
			return _authenticationResult;
		}

		private void Session_UserAuthenticationSuccessReceived(object sender, MessageEventArgs<SuccessMessage> e)
		{
			_authenticationResult = AuthenticationResult.Success;
			_authenticationCompleted.Set();
		}

		private void Session_UserAuthenticationFailureReceived(object sender, MessageEventArgs<FailureMessage> e)
		{
			if (e.Message.PartialSuccess)
			{
				_authenticationResult = AuthenticationResult.PartialSuccess;
			}
			else
			{
				_authenticationResult = AuthenticationResult.Failure;
			}
			base.AllowedAuthentications = e.Message.AllowedAuthentications.ToList();
			_authenticationCompleted.Set();
		}

		private void Session_MessageReceived(object sender, MessageEventArgs<Message> e)
		{
			if (e.Message is PasswordChangeRequiredMessage)
			{
				_session.UnRegisterMessage("SSH_MSG_USERAUTH_PASSWD_CHANGEREQ");
				ExecuteThread(delegate
				{
					try
					{
						AuthenticationPasswordChangeEventArgs authenticationPasswordChangeEventArgs = new AuthenticationPasswordChangeEventArgs(base.Username);
						if (this.PasswordExpired != null)
						{
							this.PasswordExpired(this, authenticationPasswordChangeEventArgs);
						}
						_session.SendMessage(new RequestMessagePassword(ServiceName.Connection, base.Username, _password, authenticationPasswordChangeEventArgs.NewPassword));
					}
					catch (Exception exception)
					{
						Exception ex = _exception = exception;
						_authenticationCompleted.Set();
					}
				});
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing && _authenticationCompleted != null)
				{
					Extensions.Dispose(_authenticationCompleted);
					_authenticationCompleted = null;
				}
				_isDisposed = true;
			}
		}

		~PasswordAuthenticationMethod()
		{
			Dispose(disposing: false);
		}

		private void ExecuteThread(Action action)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				action();
			});
		}
	}
}
