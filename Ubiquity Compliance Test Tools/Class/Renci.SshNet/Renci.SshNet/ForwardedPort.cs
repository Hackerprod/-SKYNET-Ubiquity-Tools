using Renci.SshNet.Common;
using System;

namespace Renci.SshNet
{
	public abstract class ForwardedPort
	{
		internal Session Session
		{
			get;
			set;
		}

		public bool IsStarted
		{
			get;
			protected set;
		}

		public event EventHandler<ExceptionEventArgs> Exception;

		public event EventHandler<PortForwardEventArgs> RequestReceived;

		public virtual void Start()
		{
			if (Session == null)
			{
				throw new InvalidOperationException("Session property is null.");
			}
			if (!Session.IsConnected)
			{
				throw new SshConnectionException("Not connected.");
			}
			Session.ErrorOccured += Session_ErrorOccured;
		}

		public virtual void Stop()
		{
			if (Session != null)
			{
				Session.ErrorOccured -= Session_ErrorOccured;
			}
		}

		protected void RaiseExceptionEvent(Exception execption)
		{
			if (this.Exception != null)
			{
				this.Exception(this, new ExceptionEventArgs(execption));
			}
		}

		protected void RaiseRequestReceived(string host, uint port)
		{
			if (this.RequestReceived != null)
			{
				this.RequestReceived(this, new PortForwardEventArgs(host, port));
			}
		}

		private void Session_ErrorOccured(object sender, ExceptionEventArgs e)
		{
			RaiseExceptionEvent(e.Exception);
		}
	}
}
