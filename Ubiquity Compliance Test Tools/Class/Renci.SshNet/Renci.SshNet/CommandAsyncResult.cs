using System;
using System.Threading;

namespace Renci.SshNet
{
	public class CommandAsyncResult : IAsyncResult
	{
		private SshCommand _command;

		public int BytesReceived
		{
			get;
			set;
		}

		public int BytesSent
		{
			get;
			set;
		}

		public object AsyncState
		{
			get;
			internal set;
		}

		public WaitHandle AsyncWaitHandle
		{
			get;
			internal set;
		}

		public bool CompletedSynchronously
		{
			get;
			internal set;
		}

		public bool IsCompleted
		{
			get;
			internal set;
		}

		internal CommandAsyncResult(SshCommand command)
		{
			_command = command;
		}
	}
}
