using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using Renci.SshNet.Sftp.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Renci.SshNet
{
	public class SftpClient : BaseClient
	{
		private SftpSession _sftpSession;

		private TimeSpan _operationTimeout;

		private uint _bufferSize;

		public TimeSpan OperationTimeout
		{
			get
			{
				CheckDisposed();
				return _operationTimeout;
			}
			set
			{
				CheckDisposed();
				_operationTimeout = value;
			}
		}

		public uint BufferSize
		{
			get
			{
				CheckDisposed();
				return _bufferSize;
			}
			set
			{
				CheckDisposed();
				_bufferSize = value;
			}
		}

		public string WorkingDirectory
		{
			get
			{
				CheckDisposed();
				if (_sftpSession == null)
				{
					throw new SshConnectionException("Client not connected.");
				}
				return _sftpSession.WorkingDirectory;
			}
		}

		public int ProtocolVersion
		{
			get
			{
				CheckDisposed();
				if (_sftpSession == null)
				{
					throw new SshConnectionException("Client not connected.");
				}
				return (int)_sftpSession.ProtocolVersion;
			}
		}

		public SftpClient(ConnectionInfo connectionInfo)
			: this(connectionInfo, ownsConnectionInfo: false)
		{
		}

		public SftpClient(string host, int port, string username, string password)
			: this(new PasswordConnectionInfo(host, port, username, password), ownsConnectionInfo: true)
		{
		}

		public SftpClient(string host, string username, string password)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, password)
		{
		}

		public SftpClient(string host, int port, string username, params PrivateKeyFile[] keyFiles)
			: this(new PrivateKeyConnectionInfo(host, port, username, keyFiles), ownsConnectionInfo: true)
		{
		}

		public SftpClient(string host, string username, params PrivateKeyFile[] keyFiles)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, keyFiles)
		{
		}

		private SftpClient(ConnectionInfo connectionInfo, bool ownsConnectionInfo)
			: base(connectionInfo, ownsConnectionInfo)
		{
			OperationTimeout = new TimeSpan(0, 0, 0, 0, -1);
			BufferSize = 65536u;
		}

		public void ChangeDirectory(string path)
		{
			CheckDisposed();
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			_sftpSession.ChangeDirectory(path);
		}

		public void ChangePermissions(string path, short mode)
		{
			SftpFile sftpFile = Get(path);
			sftpFile.SetPermissions(mode);
		}

		public void CreateDirectory(string path)
		{
			CheckDisposed();
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException(path);
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			_sftpSession.RequestMkDir(canonicalPath);
		}

		public void DeleteDirectory(string path)
		{
			CheckDisposed();
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			_sftpSession.RequestRmDir(canonicalPath);
		}

		public void DeleteFile(string path)
		{
			CheckDisposed();
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			_sftpSession.RequestRemove(canonicalPath);
		}

		public void RenameFile(string oldPath, string newPath)
		{
			RenameFile(oldPath, newPath, isPosix: false);
		}

		public void RenameFile(string oldPath, string newPath, bool isPosix)
		{
			CheckDisposed();
			if (oldPath == null)
			{
				throw new ArgumentNullException("oldPath");
			}
			if (newPath == null)
			{
				throw new ArgumentNullException("newPath");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(oldPath);
			string canonicalPath2 = _sftpSession.GetCanonicalPath(newPath);
			if (isPosix)
			{
				_sftpSession.RequestPosixRename(canonicalPath, canonicalPath2);
			}
			else
			{
				_sftpSession.RequestRename(canonicalPath, canonicalPath2);
			}
		}

		public void SymbolicLink(string path, string linkPath)
		{
			CheckDisposed();
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("path");
			}
			if (linkPath.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("linkPath");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			string canonicalPath2 = _sftpSession.GetCanonicalPath(linkPath);
			_sftpSession.RequestSymLink(canonicalPath, canonicalPath2);
		}

		public IEnumerable<SftpFile> ListDirectory(string path, Action<int> listCallback = null)
		{
			CheckDisposed();
			return InternalListDirectory(path, listCallback);
		}

		public IAsyncResult BeginListDirectory(string path, AsyncCallback asyncCallback, object state, Action<int> listCallback = null)
		{
			CheckDisposed();
			SftpListDirectoryAsyncResult asyncResult = new SftpListDirectoryAsyncResult(asyncCallback, state);
			ExecuteThread(delegate
			{
				try
				{
					IEnumerable<SftpFile> result = InternalListDirectory(path, delegate(int count)
					{
						asyncResult.Update(count);
						if (listCallback != null)
						{
							listCallback(count);
						}
					});
					asyncResult.SetAsCompleted(result, completedSynchronously: false);
				}
				catch (Exception exception)
				{
					asyncResult.SetAsCompleted(exception, completedSynchronously: false);
				}
			});
			return asyncResult;
		}

		public IEnumerable<SftpFile> EndListDirectory(IAsyncResult asyncResult)
		{
			SftpListDirectoryAsyncResult sftpListDirectoryAsyncResult = asyncResult as SftpListDirectoryAsyncResult;
			if (sftpListDirectoryAsyncResult == null || sftpListDirectoryAsyncResult.EndInvokeCalled)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndExecute was called multiple times with the same IAsyncResult.");
			}
			return sftpListDirectoryAsyncResult.EndInvoke();
		}

		public SftpFile Get(string path)
		{
			CheckDisposed();
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			SftpFileAttributes attributes = _sftpSession.RequestLStat(canonicalPath);
			return new SftpFile(_sftpSession, canonicalPath, attributes);
		}

		public bool Exists(string path)
		{
			CheckDisposed();
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string fullRemotePath = _sftpSession.GetFullRemotePath(path);
			if (_sftpSession.RequestRealPath(fullRemotePath, nullOnError: true) == null)
			{
				return false;
			}
			return true;
		}

		public void DownloadFile(string path, Stream output, Action<ulong> downloadCallback = null)
		{
			CheckDisposed();
			InternalDownloadFile(path, output, null, downloadCallback);
		}

		public IAsyncResult BeginDownloadFile(string path, Stream output)
		{
			return BeginDownloadFile(path, output, null, null);
		}

		public IAsyncResult BeginDownloadFile(string path, Stream output, AsyncCallback asyncCallback)
		{
			return BeginDownloadFile(path, output, asyncCallback, null);
		}

		public IAsyncResult BeginDownloadFile(string path, Stream output, AsyncCallback asyncCallback, object state, Action<ulong> downloadCallback = null)
		{
			CheckDisposed();
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("path");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			SftpDownloadAsyncResult asyncResult = new SftpDownloadAsyncResult(asyncCallback, state);
			ExecuteThread(delegate
			{
				try
				{
					InternalDownloadFile(path, output, asyncResult, delegate(ulong offset)
					{
						asyncResult.Update(offset);
						if (downloadCallback != null)
						{
							downloadCallback(offset);
						}
					});
					asyncResult.SetAsCompleted(null, completedSynchronously: false);
				}
				catch (Exception exception)
				{
					asyncResult.SetAsCompleted(exception, completedSynchronously: false);
				}
			});
			return asyncResult;
		}

		public void EndDownloadFile(IAsyncResult asyncResult)
		{
			SftpDownloadAsyncResult sftpDownloadAsyncResult = asyncResult as SftpDownloadAsyncResult;
			if (sftpDownloadAsyncResult == null || sftpDownloadAsyncResult.EndInvokeCalled)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndExecute was called multiple times with the same IAsyncResult.");
			}
			sftpDownloadAsyncResult.EndInvoke();
		}

		public void UploadFile(Stream input, string path, Action<ulong> uploadCallback = null)
		{
			UploadFile(input, path, canOverride: true, uploadCallback);
		}

		public void UploadFile(Stream input, string path, bool canOverride, Action<ulong> uploadCallback = null)
		{
			CheckDisposed();
			Flags flags = (Flags)18;
			flags = ((!canOverride) ? (flags | Flags.CreateNew) : (flags | Flags.CreateNewOrOpen));
			InternalUploadFile(input, path, flags, null, uploadCallback);
		}

		public IAsyncResult BeginUploadFile(Stream input, string path)
		{
			return BeginUploadFile(input, path, canOverride: true, null, null);
		}

		public IAsyncResult BeginUploadFile(Stream input, string path, AsyncCallback asyncCallback)
		{
			return BeginUploadFile(input, path, canOverride: true, asyncCallback, null);
		}

		public IAsyncResult BeginUploadFile(Stream input, string path, AsyncCallback asyncCallback, object state, Action<ulong> uploadCallback = null)
		{
			return BeginUploadFile(input, path, canOverride: true, asyncCallback, state, uploadCallback);
		}

		public IAsyncResult BeginUploadFile(Stream input, string path, bool canOverride, AsyncCallback asyncCallback, object state, Action<ulong> uploadCallback = null)
		{
			CheckDisposed();
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("path");
			}
			Flags flags = (Flags)18;
			if (canOverride)
			{
				flags |= Flags.CreateNewOrOpen;
			}
			else
			{
				flags |= Flags.CreateNew;
			}
			SftpUploadAsyncResult asyncResult = new SftpUploadAsyncResult(asyncCallback, state);
			ExecuteThread(delegate
			{
				try
				{
					InternalUploadFile(input, path, flags, asyncResult, delegate(ulong offset)
					{
						asyncResult.Update(offset);
						if (uploadCallback != null)
						{
							uploadCallback(offset);
						}
					});
					asyncResult.SetAsCompleted(null, completedSynchronously: false);
				}
				catch (Exception exception)
				{
					asyncResult.SetAsCompleted(exception, completedSynchronously: false);
				}
			});
			return asyncResult;
		}

		public void EndUploadFile(IAsyncResult asyncResult)
		{
			SftpUploadAsyncResult sftpUploadAsyncResult = asyncResult as SftpUploadAsyncResult;
			if (sftpUploadAsyncResult == null || sftpUploadAsyncResult.EndInvokeCalled)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndExecute was called multiple times with the same IAsyncResult.");
			}
			sftpUploadAsyncResult.EndInvoke();
		}

		public SftpFileSytemInformation GetStatus(string path)
		{
			CheckDisposed();
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			return _sftpSession.RequestStatVfs(canonicalPath);
		}

		public void AppendAllLines(string path, IEnumerable<string> contents)
		{
			CheckDisposed();
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			using (StreamWriter streamWriter = AppendText(path))
			{
				foreach (string content in contents)
				{
					streamWriter.WriteLine(content);
				}
			}
		}

		public void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			CheckDisposed();
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			using (StreamWriter streamWriter = AppendText(path, encoding))
			{
				foreach (string content in contents)
				{
					streamWriter.WriteLine(content);
				}
			}
		}

		public void AppendAllText(string path, string contents)
		{
			using (StreamWriter streamWriter = AppendText(path))
			{
				streamWriter.Write(contents);
			}
		}

		public void AppendAllText(string path, string contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = AppendText(path, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		public StreamWriter AppendText(string path)
		{
			return AppendText(path, Encoding.UTF8);
		}

		public StreamWriter AppendText(string path, Encoding encoding)
		{
			CheckDisposed();
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			return new StreamWriter(new SftpFileStream(_sftpSession, path, FileMode.Append, FileAccess.Write), encoding);
		}

		public SftpFileStream Create(string path)
		{
			CheckDisposed();
			return new SftpFileStream(_sftpSession, path, FileMode.Create, FileAccess.ReadWrite);
		}

		public SftpFileStream Create(string path, int bufferSize)
		{
			CheckDisposed();
			return new SftpFileStream(_sftpSession, path, FileMode.Create, FileAccess.ReadWrite, bufferSize);
		}

		public StreamWriter CreateText(string path)
		{
			return CreateText(path, Encoding.UTF8);
		}

		public StreamWriter CreateText(string path, Encoding encoding)
		{
			CheckDisposed();
			return new StreamWriter(OpenWrite(path), encoding);
		}

		public void Delete(string path)
		{
			SftpFile sftpFile = Get(path);
			sftpFile.Delete();
		}

		public DateTime GetLastAccessTime(string path)
		{
			SftpFile sftpFile = Get(path);
			return sftpFile.LastAccessTime;
		}

		public DateTime GetLastAccessTimeUtc(string path)
		{
			return GetLastAccessTime(path).ToUniversalTime();
		}

		public DateTime GetLastWriteTime(string path)
		{
			SftpFile sftpFile = Get(path);
			return sftpFile.LastWriteTime;
		}

		public DateTime GetLastWriteTimeUtc(string path)
		{
			return GetLastWriteTime(path).ToUniversalTime();
		}

		public SftpFileStream Open(string path, FileMode mode)
		{
			return Open(path, mode, FileAccess.ReadWrite);
		}

		public SftpFileStream Open(string path, FileMode mode, FileAccess access)
		{
			CheckDisposed();
			return new SftpFileStream(_sftpSession, path, mode, access, (int)_bufferSize);
		}

		public SftpFileStream OpenRead(string path)
		{
			return Open(path, FileMode.Open, FileAccess.Read);
		}

		public StreamReader OpenText(string path)
		{
			return new StreamReader(OpenRead(path), Encoding.UTF8);
		}

		public SftpFileStream OpenWrite(string path)
		{
			CheckDisposed();
			return new SftpFileStream(_sftpSession, path, FileMode.OpenOrCreate, FileAccess.Write, (int)_bufferSize);
		}

		public byte[] ReadAllBytes(string path)
		{
			using (SftpFileStream sftpFileStream = OpenRead(path))
			{
				byte[] array = new byte[sftpFileStream.Length];
				sftpFileStream.Read(array, 0, array.Length);
				return array;
			}
		}

		public string[] ReadAllLines(string path)
		{
			return ReadAllLines(path, Encoding.UTF8);
		}

		public string[] ReadAllLines(string path, Encoding encoding)
		{
			List<string> list = new List<string>();
			using (StreamReader streamReader = new StreamReader(OpenRead(path), encoding))
			{
				while (!streamReader.EndOfStream)
				{
					list.Add(streamReader.ReadLine());
				}
			}
			return list.ToArray();
		}

		public string ReadAllText(string path)
		{
			return ReadAllText(path, Encoding.UTF8);
		}

		public string ReadAllText(string path, Encoding encoding)
		{
			using (StreamReader streamReader = new StreamReader(OpenRead(path), encoding))
			{
				return streamReader.ReadToEnd();
			}
		}

		public IEnumerable<string> ReadLines(string path)
		{
			return ReadAllLines(path);
		}

		public IEnumerable<string> ReadLines(string path, Encoding encoding)
		{
			return ReadAllLines(path, encoding);
		}

		[Obsolete("Note: This method currently throws NotImplementedException because it has not yet been implemented.")]
		public void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			throw new NotImplementedException();
		}

		[Obsolete("Note: This method currently throws NotImplementedException because it has not yet been implemented.")]
		public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			throw new NotImplementedException();
		}

		[Obsolete("Note: This method currently throws NotImplementedException because it has not yet been implemented.")]
		public void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			throw new NotImplementedException();
		}

		[Obsolete("Note: This method currently throws NotImplementedException because it has not yet been implemented.")]
		public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			throw new NotImplementedException();
		}

		public void WriteAllBytes(string path, byte[] bytes)
		{
			using (SftpFileStream sftpFileStream = OpenWrite(path))
			{
				sftpFileStream.Write(bytes, 0, bytes.Length);
			}
		}

		public void WriteAllLines(string path, IEnumerable<string> contents)
		{
			WriteAllLines(path, contents, Encoding.UTF8);
		}

		public void WriteAllLines(string path, string[] contents)
		{
			WriteAllLines(path, contents, Encoding.UTF8);
		}

		public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = CreateText(path, encoding))
			{
				foreach (string content in contents)
				{
					streamWriter.WriteLine(content);
				}
			}
		}

		public void WriteAllLines(string path, string[] contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = CreateText(path, encoding))
			{
				foreach (string value in contents)
				{
					streamWriter.WriteLine(value);
				}
			}
		}

		public void WriteAllText(string path, string contents)
		{
			using (StreamWriter streamWriter = CreateText(path))
			{
				streamWriter.Write(contents);
			}
		}

		public void WriteAllText(string path, string contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = CreateText(path, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		public SftpFileAttributes GetAttributes(string path)
		{
			CheckDisposed();
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			return _sftpSession.RequestLStat(canonicalPath);
		}

		public void SetAttributes(string path, SftpFileAttributes fileAttributes)
		{
			CheckDisposed();
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			_sftpSession.RequestSetStat(canonicalPath, fileAttributes);
		}

		private IEnumerable<SftpFile> InternalListDirectory(string path, Action<int> listCallback)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			byte[] handle = _sftpSession.RequestOpenDir(canonicalPath);
			string basePath = canonicalPath;
			if (!basePath.EndsWith("/"))
			{
				basePath = $"{canonicalPath}/";
			}
			List<SftpFile> result = new List<SftpFile>();
			for (KeyValuePair<string, SftpFileAttributes>[] array = _sftpSession.RequestReadDir(handle); array != null; array = _sftpSession.RequestReadDir(handle))
			{
				result.AddRange(from f in array
				select new SftpFile(_sftpSession, string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[2]
				{
					basePath,
					f.Key
				}), f.Value));
				if (listCallback != null)
				{
					ExecuteThread(delegate
					{
						listCallback(result.Count);
					});
				}
			}
			_sftpSession.RequestClose(handle);
			return result;
		}

		private void InternalDownloadFile(string path, Stream output, SftpDownloadAsyncResult asyncResult, Action<ulong> downloadCallback)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			byte[] handle = _sftpSession.RequestOpen(canonicalPath, Flags.Read);
			ulong offset = 0uL;
			uint length = _sftpSession.CalculateOptimalReadLength(_bufferSize);
			byte[] array = _sftpSession.RequestRead(handle, offset, length);
			while (array.Length > 0 && (asyncResult == null || !asyncResult.IsDownloadCanceled))
			{
				output.Write(array, 0, array.Length);
				output.Flush();
				offset = (ulong)((long)offset + (long)array.Length);
				if (downloadCallback != null)
				{
					ExecuteThread(delegate
					{
						downloadCallback(offset);
					});
				}
				array = _sftpSession.RequestRead(handle, offset, length);
			}
			_sftpSession.RequestClose(handle);
		}

		private void InternalUploadFile(Stream input, string path, Flags flags, SftpUploadAsyncResult asyncResult, Action<ulong> uploadCallback)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (path.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("path");
			}
			if (_sftpSession == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			string canonicalPath = _sftpSession.GetCanonicalPath(path);
			byte[] handle = _sftpSession.RequestOpen(canonicalPath, flags);
			ulong num = 0uL;
			byte[] array = new byte[_sftpSession.CalculateOptimalWriteLength(_bufferSize, handle)];
			int num2 = input.Read(array, 0, array.Length);
			int expectedResponses = 0;
			AutoResetEvent responseReceivedWaitHandle = new AutoResetEvent(initialState: false);
			while (asyncResult == null || !asyncResult.IsUploadCanceled)
			{
				if (num2 > 0)
				{
					if (num2 < array.Length)
					{
						byte[] array2 = new byte[num2];
						Buffer.BlockCopy(array, 0, array2, 0, num2);
						array = array2;
					}
					ulong writtenBytes = (ulong)((long)num + (long)array.Length);
					_sftpSession.RequestWrite(handle, num, array, null, delegate(SftpStatusResponse s)
					{
						if (s.StatusCode == StatusCodes.Ok)
						{
							Interlocked.Decrement(ref expectedResponses);
							responseReceivedWaitHandle.Set();
							if (uploadCallback != null)
							{
								ExecuteThread(delegate
								{
									uploadCallback(writtenBytes);
								});
							}
						}
					});
					Interlocked.Increment(ref expectedResponses);
					num += (uint)num2;
					num2 = input.Read(array, 0, array.Length);
				}
				else if (expectedResponses > 0)
				{
					_sftpSession.WaitOnHandle(responseReceivedWaitHandle, OperationTimeout);
				}
				if (expectedResponses <= 0 && num2 <= 0)
				{
					break;
				}
			}
			_sftpSession.RequestClose(handle);
		}

		protected override void OnConnected()
		{
			base.OnConnected();
			_sftpSession = new SftpSession(base.Session, OperationTimeout, base.ConnectionInfo.Encoding);
			_sftpSession.Connect();
		}

		protected override void OnDisconnecting()
		{
			base.OnDisconnecting();
			if (_sftpSession != null)
			{
				_sftpSession.Disconnect();
				_sftpSession.Dispose();
				_sftpSession = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_sftpSession != null)
			{
				_sftpSession.Dispose();
				_sftpSession = null;
			}
			base.Dispose(disposing);
		}

		public IEnumerable<FileInfo> SynchronizeDirectories(string sourcePath, string destinationPath, string searchPattern)
		{
			return InternalSynchronizeDirectories(sourcePath, destinationPath, searchPattern, null);
		}

		public IAsyncResult BeginSynchronizeDirectories(string sourcePath, string destinationPath, string searchPattern, AsyncCallback asyncCallback, object state)
		{
			if (sourcePath == null)
			{
				throw new ArgumentNullException("sourcePath");
			}
			if (destinationPath.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("destDir");
			}
			SftpSynchronizeDirectoriesAsyncResult asyncResult = new SftpSynchronizeDirectoriesAsyncResult(asyncCallback, state);
			ExecuteThread(delegate
			{
				try
				{
					IEnumerable<FileInfo> result = InternalSynchronizeDirectories(sourcePath, destinationPath, searchPattern, asyncResult);
					asyncResult.SetAsCompleted(result, completedSynchronously: false);
				}
				catch (Exception exception)
				{
					asyncResult.SetAsCompleted(exception, completedSynchronously: false);
				}
			});
			return asyncResult;
		}

		public IEnumerable<FileInfo> EndSynchronizeDirectories(IAsyncResult asyncResult)
		{
			SftpSynchronizeDirectoriesAsyncResult sftpSynchronizeDirectoriesAsyncResult = asyncResult as SftpSynchronizeDirectoriesAsyncResult;
			if (sftpSynchronizeDirectoriesAsyncResult == null || sftpSynchronizeDirectoriesAsyncResult.EndInvokeCalled)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndExecute was called multiple times with the same IAsyncResult.");
			}
			return sftpSynchronizeDirectoriesAsyncResult.EndInvoke();
		}

		private IEnumerable<FileInfo> InternalSynchronizeDirectories(string sourcePath, string destinationPath, string searchPattern, SftpSynchronizeDirectoriesAsyncResult asynchResult)
		{
			if (destinationPath.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("destinationPath");
			}
			if (!Directory.Exists(sourcePath))
			{
				throw new FileNotFoundException($"Source directory not found: {sourcePath}");
			}
			IList<FileInfo> list = new List<FileInfo>();
			DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
			FileInfo[] files = directoryInfo.GetFiles(searchPattern);
			if (files == null || !files.Any())
			{
				return list;
			}
			IEnumerable<SftpFile> enumerable = InternalListDirectory(destinationPath, null);
			Dictionary<string, SftpFile> dictionary = new Dictionary<string, SftpFile>();
			foreach (SftpFile item in enumerable)
			{
				if (!item.IsDirectory)
				{
					dictionary.Add(item.Name, item);
				}
			}
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				bool flag = !dictionary.ContainsKey(fileInfo.Name);
				if (!flag)
				{
					SftpFile sftpFile = dictionary[fileInfo.Name];
					flag = (fileInfo.Length != sftpFile.Length);
				}
				if (flag)
				{
					string text = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[2]
					{
						destinationPath,
						fileInfo.Name
					});
					try
					{
						using (FileStream input = File.OpenRead(fileInfo.FullName))
						{
							InternalUploadFile(input, text, (Flags)26, null, null);
						}
						list.Add(fileInfo);
						asynchResult?.Update(list.Count);
					}
					catch (Exception innerException)
					{
						throw new Exception($"Failed to upload {fileInfo.FullName} to {text}", innerException);
					}
				}
			}
			return list;
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
