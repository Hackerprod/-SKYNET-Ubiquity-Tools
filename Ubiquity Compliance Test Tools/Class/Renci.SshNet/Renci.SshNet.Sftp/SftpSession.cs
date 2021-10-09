using Renci.SshNet.Common;
using Renci.SshNet.Sftp.Requests;
using Renci.SshNet.Sftp.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Renci.SshNet.Sftp
{
	internal class SftpSession : SubsystemSession
	{
		private const int MAXIMUM_SUPPORTED_VERSION = 3;

		private const int MINIMUM_SUPPORTED_VERSION = 0;

		private readonly Dictionary<uint, SftpRequest> _requests = new Dictionary<uint, SftpRequest>();

		private readonly List<byte> _data = new List<byte>(16384);

		private EventWaitHandle _sftpVersionConfirmed = new AutoResetEvent(initialState: false);

		private IDictionary<string, string> _supportedExtensions;

		private long _requestId;

		public string WorkingDirectory
		{
			get;
			private set;
		}

		public uint ProtocolVersion
		{
			get;
			private set;
		}

		public uint NextRequestId => (uint)Interlocked.Increment(ref _requestId);

		public SftpSession(Session session, TimeSpan operationTimeout, Encoding encoding)
			: base(session, "sftp", operationTimeout, encoding)
		{
		}

		public void ChangeDirectory(string path)
		{
			string canonicalPath = GetCanonicalPath(path);
			byte[] handle = RequestOpenDir(canonicalPath);
			RequestClose(handle);
			WorkingDirectory = canonicalPath;
		}

		internal void SendMessage(SftpMessage sftpMessage)
		{
			byte[] bytes = sftpMessage.GetBytes();
			byte[] array = new byte[4 + bytes.Length];
			((uint)bytes.Length).GetBytes().CopyTo(array, 0);
			bytes.CopyTo(array, 4);
			SendData(array);
		}

		internal string GetCanonicalPath(string path)
		{
			string fullRemotePath = GetFullRemotePath(path);
			string text = string.Empty;
			KeyValuePair<string, SftpFileAttributes>[] array = RequestRealPath(fullRemotePath, nullOnError: true);
			if (array != null)
			{
				text = array.First().Key;
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (fullRemotePath.EndsWith("/.", StringComparison.InvariantCultureIgnoreCase) || fullRemotePath.EndsWith("/..", StringComparison.InvariantCultureIgnoreCase) || fullRemotePath.Equals("/", StringComparison.InvariantCultureIgnoreCase) || fullRemotePath.IndexOf('/') < 0)
			{
				return fullRemotePath;
			}
			string[] array2 = fullRemotePath.Split('/');
			string text2 = string.Join("/", array2, 0, array2.Length - 1);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "/";
			}
			array = RequestRealPath(text2, nullOnError: true);
			if (array != null)
			{
				text = array.First().Key;
			}
			if (string.IsNullOrEmpty(text))
			{
				return fullRemotePath;
			}
			string text3 = string.Empty;
			if (text[text.Length - 1] != '/')
			{
				text3 = "/";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", new object[3]
			{
				text,
				text3,
				array2[array2.Length - 1]
			});
		}

		internal string GetFullRemotePath(string path)
		{
			string result = path;
			if (!string.IsNullOrEmpty(path) && path[0] != '/' && WorkingDirectory != null)
			{
				result = ((WorkingDirectory[WorkingDirectory.Length - 1] != '/') ? string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[2]
				{
					WorkingDirectory,
					path
				}) : string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[2]
				{
					WorkingDirectory,
					path
				}));
			}
			return result;
		}

		protected override void OnChannelOpen()
		{
			SendMessage(new SftpInitRequest(3u));
			WaitOnHandle(_sftpVersionConfirmed, _operationTimeout);
			if (ProtocolVersion > 3 || ProtocolVersion < 0)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Server SFTP version {0} is not supported.", new object[1]
				{
					ProtocolVersion
				}));
			}
			WorkingDirectory = RequestRealPath(".").First().Key;
		}

		protected override void OnDataReceived(uint dataTypeCode, byte[] data)
		{
			_data.AddRange(data);
			while (_data.Count > 5)
			{
				int num = (_data[0] << 24) | (_data[1] << 16) | (_data[2] << 8) | _data[3];
				if (_data.Count < num + 4)
				{
					break;
				}
				_data.RemoveRange(0, 4);
				byte[] array = new byte[num];
				_data.CopyTo(0, array, 0, num);
				_data.RemoveRange(0, num);
				SftpMessage sftpMessage = SftpMessage.Load(ProtocolVersion, array, base.Encoding);
				try
				{
					SftpVersionResponse sftpVersionResponse = sftpMessage as SftpVersionResponse;
					if (sftpVersionResponse != null)
					{
						ProtocolVersion = sftpVersionResponse.Version;
						_supportedExtensions = sftpVersionResponse.Extentions;
						_sftpVersionConfirmed.Set();
					}
					else
					{
						HandleResponse(sftpMessage as SftpResponse);
					}
				}
				catch (Exception error)
				{
					RaiseError(error);
					return;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing && _sftpVersionConfirmed != null)
			{
				Extensions.Dispose(_sftpVersionConfirmed);
				_sftpVersionConfirmed = null;
			}
		}

		private void SendRequest(SftpRequest request)
		{
			lock (_requests)
			{
				_requests.Add(request.RequestId, request);
			}
			SendMessage(request);
		}

		internal byte[] RequestOpen(string path, Flags flags, bool nullOnError = false)
		{
			byte[] handle = null;
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpOpenRequest request = new SftpOpenRequest(ProtocolVersion, NextRequestId, path, base.Encoding, flags, delegate(SftpHandleResponse response)
				{
					handle = response.Handle;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (!nullOnError && exception != null)
			{
				throw exception;
			}
			return handle;
		}

		internal void RequestClose(byte[] handle)
		{
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpCloseRequest request = new SftpCloseRequest(ProtocolVersion, NextRequestId, handle, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal byte[] RequestRead(byte[] handle, ulong offset, uint length)
		{
			SshException exception = null;
			byte[] data = new byte[0];
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpReadRequest request = new SftpReadRequest(ProtocolVersion, NextRequestId, handle, offset, length, delegate(SftpDataResponse response)
				{
					data = response.Data;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					if (response.StatusCode != StatusCodes.Eof)
					{
						exception = GetSftpException(response);
					}
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
			return data;
		}

		internal void RequestWrite(byte[] handle, ulong offset, byte[] data, EventWaitHandle wait, Action<SftpStatusResponse> writeCompleted = null)
		{
			SshException exception = null;
			SftpWriteRequest request = new SftpWriteRequest(ProtocolVersion, NextRequestId, handle, offset, data, delegate(SftpStatusResponse response)
			{
				if (writeCompleted != null)
				{
					writeCompleted(response);
				}
				exception = GetSftpException(response);
				if (wait != null)
				{
					wait.Set();
				}
			});
			SendRequest(request);
			if (wait != null)
			{
				WaitOnHandle(wait, _operationTimeout);
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal SftpFileAttributes RequestLStat(string path, bool nullOnError = false)
		{
			SshException exception = null;
			SftpFileAttributes attributes = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpLStatRequest request = new SftpLStatRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpAttrsResponse response)
				{
					attributes = response.Attributes;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
			return attributes;
		}

		internal SftpFileAttributes RequestFStat(byte[] handle, bool nullOnError = false)
		{
			SshException exception = null;
			SftpFileAttributes attributes = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpFStatRequest request = new SftpFStatRequest(ProtocolVersion, NextRequestId, handle, delegate(SftpAttrsResponse response)
				{
					attributes = response.Attributes;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
			return attributes;
		}

		internal void RequestSetStat(string path, SftpFileAttributes attributes)
		{
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpSetStatRequest request = new SftpSetStatRequest(ProtocolVersion, NextRequestId, path, base.Encoding, attributes, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal void RequestFSetStat(byte[] handle, SftpFileAttributes attributes)
		{
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpFSetStatRequest request = new SftpFSetStatRequest(ProtocolVersion, NextRequestId, handle, attributes, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal byte[] RequestOpenDir(string path, bool nullOnError = false)
		{
			SshException exception = null;
			byte[] handle = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpOpenDirRequest request = new SftpOpenDirRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpHandleResponse response)
				{
					handle = response.Handle;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (!nullOnError && exception != null)
			{
				throw exception;
			}
			return handle;
		}

		internal KeyValuePair<string, SftpFileAttributes>[] RequestReadDir(byte[] handle)
		{
			SshException exception = null;
			KeyValuePair<string, SftpFileAttributes>[] result = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpReadDirRequest request = new SftpReadDirRequest(ProtocolVersion, NextRequestId, handle, delegate(SftpNameResponse response)
				{
					result = response.Files;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					if (response.StatusCode != StatusCodes.Eof)
					{
						exception = GetSftpException(response);
					}
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
			return result;
		}

		internal void RequestRemove(string path)
		{
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpRemoveRequest request = new SftpRemoveRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal void RequestMkDir(string path)
		{
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpMkDirRequest request = new SftpMkDirRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal void RequestRmDir(string path)
		{
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpRmDirRequest request = new SftpRmDirRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal KeyValuePair<string, SftpFileAttributes>[] RequestRealPath(string path, bool nullOnError = false)
		{
			SshException exception = null;
			KeyValuePair<string, SftpFileAttributes>[] result = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpRealPathRequest request = new SftpRealPathRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpNameResponse response)
				{
					result = response.Files;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (!nullOnError && exception != null)
			{
				throw exception;
			}
			return result;
		}

		internal SftpFileAttributes RequestStat(string path, bool nullOnError = false)
		{
			SshException exception = null;
			SftpFileAttributes attributes = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpStatRequest request = new SftpStatRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpAttrsResponse response)
				{
					attributes = response.Attributes;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (!nullOnError && exception != null)
			{
				throw exception;
			}
			return attributes;
		}

		internal void RequestRename(string oldPath, string newPath)
		{
			if (ProtocolVersion < 2)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "SSH_FXP_RENAME operation is not supported in {0} version that server operates in.", new object[1]
				{
					ProtocolVersion
				}));
			}
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpRenameRequest request = new SftpRenameRequest(ProtocolVersion, NextRequestId, oldPath, newPath, base.Encoding, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal KeyValuePair<string, SftpFileAttributes>[] RequestReadLink(string path, bool nullOnError = false)
		{
			if (ProtocolVersion < 3)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "SSH_FXP_READLINK operation is not supported in {0} version that server operates in.", new object[1]
				{
					ProtocolVersion
				}));
			}
			SshException exception = null;
			KeyValuePair<string, SftpFileAttributes>[] result = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpReadLinkRequest request = new SftpReadLinkRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpNameResponse response)
				{
					result = response.Files;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (!nullOnError && exception != null)
			{
				throw exception;
			}
			return result;
		}

		internal void RequestSymLink(string linkpath, string targetpath)
		{
			if (ProtocolVersion < 3)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "SSH_FXP_SYMLINK operation is not supported in {0} version that server operates in.", new object[1]
				{
					ProtocolVersion
				}));
			}
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				SftpSymLinkRequest request = new SftpSymLinkRequest(ProtocolVersion, NextRequestId, linkpath, targetpath, base.Encoding, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				SendRequest(request);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal void RequestPosixRename(string oldPath, string newPath)
		{
			if (ProtocolVersion < 3)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "SSH_FXP_EXTENDED operation is not supported in {0} version that server operates in.", new object[1]
				{
					ProtocolVersion
				}));
			}
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				PosixRenameRequest posixRenameRequest = new PosixRenameRequest(ProtocolVersion, NextRequestId, oldPath, newPath, base.Encoding, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				if (!_supportedExtensions.ContainsKey(posixRenameRequest.Name))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Extension method {0} currently not supported by the server.", new object[1]
					{
						posixRenameRequest.Name
					}));
				}
				SendRequest(posixRenameRequest);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal SftpFileSytemInformation RequestStatVfs(string path, bool nullOnError = false)
		{
			if (ProtocolVersion < 3)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "SSH_FXP_EXTENDED operation is not supported in {0} version that server operates in.", new object[1]
				{
					ProtocolVersion
				}));
			}
			SshException exception = null;
			SftpFileSytemInformation information = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				StatVfsRequest statVfsRequest = new StatVfsRequest(ProtocolVersion, NextRequestId, path, base.Encoding, delegate(SftpExtendedReplyResponse response)
				{
					information = response.GetReply<StatVfsReplyInfo>().Information;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				if (!_supportedExtensions.ContainsKey(statVfsRequest.Name))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Extension method {0} currently not supported by the server.", new object[1]
					{
						statVfsRequest.Name
					}));
				}
				SendRequest(statVfsRequest);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (!nullOnError && exception != null)
			{
				throw exception;
			}
			return information;
		}

		internal SftpFileSytemInformation RequestFStatVfs(byte[] handle, bool nullOnError = false)
		{
			if (ProtocolVersion < 3)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "SSH_FXP_EXTENDED operation is not supported in {0} version that server operates in.", new object[1]
				{
					ProtocolVersion
				}));
			}
			SshException exception = null;
			SftpFileSytemInformation information = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				FStatVfsRequest fStatVfsRequest = new FStatVfsRequest(ProtocolVersion, NextRequestId, handle, delegate(SftpExtendedReplyResponse response)
				{
					information = response.GetReply<StatVfsReplyInfo>().Information;
					wait.Set();
				}, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				if (!_supportedExtensions.ContainsKey(fStatVfsRequest.Name))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Extension method {0} currently not supported by the server.", new object[1]
					{
						fStatVfsRequest.Name
					}));
				}
				SendRequest(fStatVfsRequest);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (!nullOnError && exception != null)
			{
				throw exception;
			}
			return information;
		}

		internal void HardLink(string oldPath, string newPath)
		{
			if (ProtocolVersion < 3)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "SSH_FXP_EXTENDED operation is not supported in {0} version that server operates in.", new object[1]
				{
					ProtocolVersion
				}));
			}
			SshException exception = null;
			AutoResetEvent wait = new AutoResetEvent(initialState: false);
			try
			{
				HardLinkRequest hardLinkRequest = new HardLinkRequest(ProtocolVersion, NextRequestId, oldPath, newPath, delegate(SftpStatusResponse response)
				{
					exception = GetSftpException(response);
					wait.Set();
				});
				if (!_supportedExtensions.ContainsKey(hardLinkRequest.Name))
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Extension method {0} currently not supported by the server.", new object[1]
					{
						hardLinkRequest.Name
					}));
				}
				SendRequest(hardLinkRequest);
				WaitOnHandle(wait, _operationTimeout);
			}
			finally
			{
				if (wait != null)
				{
					((IDisposable)wait).Dispose();
				}
			}
			if (exception != null)
			{
				throw exception;
			}
		}

		internal uint CalculateOptimalReadLength(uint bufferSize)
		{
			uint localPacketSize = base.Channel.LocalPacketSize;
			return Math.Min(bufferSize, localPacketSize) - 13;
		}

		internal uint CalculateOptimalWriteLength(uint bufferSize, byte[] handle)
		{
			uint num = (uint)(25 + handle.Length);
			uint remotePacketSize = base.Channel.RemotePacketSize;
			return Math.Min(bufferSize, remotePacketSize) - num;
		}

		private SshException GetSftpException(SftpStatusResponse response)
		{
			if (response.StatusCode == StatusCodes.Ok)
			{
				return null;
			}
			if (response.StatusCode == StatusCodes.PermissionDenied)
			{
				return new SftpPermissionDeniedException(response.ErrorMessage);
			}
			if (response.StatusCode == StatusCodes.NoSuchFile)
			{
				return new SftpPathNotFoundException(response.ErrorMessage);
			}
			return new SshException(response.ErrorMessage);
		}

		private void HandleResponse(SftpResponse response)
		{
			SftpRequest value;
			lock (_requests)
			{
				_requests.TryGetValue(response.ResponseId, out value);
				if (value != null)
				{
					_requests.Remove(response.ResponseId);
				}
			}
			if (value == null)
			{
				throw new InvalidOperationException("Invalid response.");
			}
			value.Complete(response);
		}
	}
}
