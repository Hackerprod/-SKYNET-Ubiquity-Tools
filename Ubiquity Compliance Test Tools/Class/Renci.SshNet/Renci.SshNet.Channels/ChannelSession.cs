using Renci.SshNet.Common;
using Renci.SshNet.Messages.Connection;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Renci.SshNet.Channels
{
	internal class ChannelSession : ClientChannel
	{
		private int _failedOpenAttempts;

		private EventWaitHandle _channelOpenResponseWaitHandle = new AutoResetEvent(initialState: false);

		private EventWaitHandle _channelRequestResponse = new ManualResetEvent(initialState: false);

		private bool _channelRequestSucces;

		public override ChannelTypes ChannelType => ChannelTypes.Session;

		public virtual void Open()
		{
			if (!base.IsOpen)
			{
				while (_failedOpenAttempts < base.ConnectionInfo.RetryAttempts && !base.IsOpen)
				{
					SendChannelOpenMessage();
					WaitOnHandle(_channelOpenResponseWaitHandle);
				}
				if (!base.IsOpen)
				{
					throw new SshException(string.Format(CultureInfo.CurrentCulture, "Failed to open a channel after {0} attempts.", new object[1]
					{
						_failedOpenAttempts
					}));
				}
			}
		}

		protected override void OnOpenConfirmation(uint remoteChannelNumber, uint initialWindowSize, uint maximumPacketSize)
		{
			base.OnOpenConfirmation(remoteChannelNumber, initialWindowSize, maximumPacketSize);
			_channelOpenResponseWaitHandle.Set();
		}

		protected override void OnOpenFailure(uint reasonCode, string description, string language)
		{
			_failedOpenAttempts++;
			base.SessionSemaphore.Release();
			_channelOpenResponseWaitHandle.Set();
		}

		protected override void OnClose()
		{
			base.OnClose();
			Thread.Sleep(100);
			base.SessionSemaphore.Release();
		}

		protected override void Close(bool wait)
		{
			base.Close(wait);
			if (!wait)
			{
				base.SessionSemaphore.Release();
			}
		}

		public bool SendPseudoTerminalRequest(string environmentVariable, uint columns, uint rows, uint width, uint height, IDictionary<TerminalModes, uint> terminalModeValues)
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new PseudoTerminalRequestInfo(environmentVariable, columns, rows, width, height, terminalModeValues)));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		public bool SendX11ForwardingRequest(bool isSingleConnection, string protocol, byte[] cookie, uint screenNumber)
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new X11ForwardingRequestInfo(isSingleConnection, protocol, cookie, screenNumber)));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		public bool SendEnvironmentVariableRequest(string variableName, string variableValue)
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new EnvironmentVariableRequestInfo(variableName, variableValue)));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		public bool SendShellRequest()
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new ShellRequestInfo()));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		public bool SendExecRequest(string command)
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new ExecRequestInfo(command, base.ConnectionInfo.Encoding)));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		public bool SendBreakRequest(uint breakLength)
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new BreakRequestInfo(breakLength)));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		public bool SendSubsystemRequest(string subsystem)
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new SubsystemRequestInfo(subsystem)));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		public bool SendWindowChangeRequest(uint columns, uint rows, uint width, uint height)
		{
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new WindowChangeRequestInfo(columns, rows, width, height)));
			return true;
		}

		public bool SendLocalFlowRequest(bool clientCanDo)
		{
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new XonXoffRequestInfo(clientCanDo)));
			return true;
		}

		public bool SendSignalRequest(string signalName)
		{
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new SignalRequestInfo(signalName)));
			return true;
		}

		public bool SendExitStatusRequest(uint exitStatus)
		{
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new ExitStatusRequestInfo(exitStatus)));
			return true;
		}

		public bool SendExitSignalRequest(string signalName, bool coreDumped, string errorMessage, string language)
		{
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new ExitSignalRequestInfo(signalName, coreDumped, errorMessage, language)));
			return true;
		}

		public bool SendEndOfWriteRequest()
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new EndOfWriteRequestInfo()));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		public bool SendKeepAliveRequest()
		{
			_channelRequestResponse.Reset();
			SendMessage(new ChannelRequestMessage(base.RemoteChannelNumber, new KeepAliveRequestInfo()));
			WaitOnHandle(_channelRequestResponse);
			return _channelRequestSucces;
		}

		protected override void OnSuccess()
		{
			base.OnSuccess();
			_channelRequestSucces = true;
			_channelRequestResponse?.Set();
		}

		protected override void OnFailure()
		{
			base.OnFailure();
			_channelRequestSucces = false;
			_channelRequestResponse?.Set();
		}

		protected void SendChannelOpenMessage()
		{
			lock (base.SessionSemaphore)
			{
				base.SessionSemaphore.Wait();
				SendMessage(new ChannelOpenMessage(base.LocalChannelNumber, base.LocalWindowSize, base.LocalPacketSize, new SessionChannelOpenInfo()));
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_channelOpenResponseWaitHandle != null)
			{
				Extensions.Dispose(_channelOpenResponseWaitHandle);
				_channelOpenResponseWaitHandle = null;
			}
			if (_channelRequestResponse != null)
			{
				Extensions.Dispose(_channelRequestResponse);
				_channelRequestResponse = null;
			}
			base.Dispose(disposing);
		}
	}
}
