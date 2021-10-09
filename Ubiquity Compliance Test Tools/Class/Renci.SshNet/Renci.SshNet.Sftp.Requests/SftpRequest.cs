using Renci.SshNet.Sftp.Responses;
using System;

namespace Renci.SshNet.Sftp.Requests
{
	internal abstract class SftpRequest : SftpMessage
	{
		private readonly Action<SftpStatusResponse> _statusAction;

		private Action<SftpAttrsResponse> _attrsAction;

		private Action<SftpDataResponse> _dataAction;

		private Action<SftpExtendedReplyResponse> _extendedReplyAction;

		private Action<SftpHandleResponse> _handleAction;

		private Action<SftpNameResponse> _nameAction;

		public uint RequestId
		{
			get;
			private set;
		}

		public uint ProtocolVersion
		{
			get;
			private set;
		}

		public SftpRequest(uint protocolVersion, uint requestId, Action<SftpStatusResponse> statusAction)
		{
			RequestId = requestId;
			ProtocolVersion = protocolVersion;
			_statusAction = statusAction;
		}

		public void Complete(SftpResponse response)
		{
			if (response is SftpStatusResponse)
			{
				_statusAction(response as SftpStatusResponse);
			}
			else if (response is SftpAttrsResponse)
			{
				_attrsAction(response as SftpAttrsResponse);
			}
			else if (response is SftpDataResponse)
			{
				_dataAction(response as SftpDataResponse);
			}
			else if (response is SftpExtendedReplyResponse)
			{
				_extendedReplyAction(response as SftpExtendedReplyResponse);
			}
			else if (response is SftpHandleResponse)
			{
				_handleAction(response as SftpHandleResponse);
			}
			else
			{
				if (!(response is SftpNameResponse))
				{
					throw new InvalidOperationException($"Response of type '{response.GetType().Name}' is not expected.");
				}
				_nameAction(response as SftpNameResponse);
			}
		}

		protected void SetAction(Action<SftpAttrsResponse> action)
		{
			_attrsAction = action;
		}

		protected void SetAction(Action<SftpDataResponse> action)
		{
			_dataAction = action;
		}

		protected void SetAction(Action<SftpExtendedReplyResponse> action)
		{
			_extendedReplyAction = action;
		}

		protected void SetAction(Action<SftpHandleResponse> action)
		{
			_handleAction = action;
		}

		protected void SetAction(Action<SftpNameResponse> action)
		{
			_nameAction = action;
		}

		protected override void LoadData()
		{
			throw new InvalidOperationException("Request cannot be saved.");
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(RequestId);
		}
	}
}
