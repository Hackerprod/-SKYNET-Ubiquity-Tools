using Renci.SshNet.Common;
using System;
using System.IO;
using System.Threading;

namespace Renci.SshNet.Sftp
{
	public class SftpFileStream : Stream
	{
		private byte[] _handle;

		private readonly FileAccess _access;

		private readonly bool _ownsHandle;

		private readonly bool _isAsync;

		private SftpSession _session;

		private readonly int _readBufferSize;

		private readonly byte[] _readBuffer;

		private readonly int _writeBufferSize;

		private readonly byte[] _writeBuffer;

		private int _bufferPosition;

		private int _bufferLen;

		private long _position;

		private bool _bufferOwnedByWrite;

		private readonly bool _canSeek;

		private ulong _serverFilePosition;

		private SftpFileAttributes _attributes;

		private readonly object _lock = new object();

		public override bool CanRead => (_access & FileAccess.Read) != (FileAccess)0;

		public override bool CanSeek => _canSeek;

		public override bool CanWrite => (_access & FileAccess.Write) != (FileAccess)0;

		public override long Length
		{
			get
			{
				if (!_canSeek)
				{
					throw new NotSupportedException("Seek operation is not supported.");
				}
				lock (_lock)
				{
					if (_handle == null)
					{
						throw new IOException("Stream is closed.");
					}
					if (_bufferOwnedByWrite)
					{
						FlushWriteBuffer();
					}
					_attributes = _session.RequestFStat(_handle);
					if (_attributes == null || _attributes.Size <= -1)
					{
						throw new IOException("Seek operation failed.");
					}
					return _attributes.Size;
				}
			}
		}

		public override long Position
		{
			get
			{
				if (!_canSeek)
				{
					throw new NotSupportedException("Seek operation not supported.");
				}
				return _position;
			}
			set
			{
				Seek(value, SeekOrigin.Begin);
			}
		}

		public virtual bool IsAsync => _isAsync;

		public string Name
		{
			get;
			private set;
		}

		public virtual byte[] Handle
		{
			get
			{
				Flush();
				return _handle;
			}
		}

		public TimeSpan Timeout
		{
			get;
			set;
		}

		internal SftpFileStream(SftpSession session, string path, FileMode mode)
			: this(session, path, mode, FileAccess.ReadWrite)
		{
		}

		internal SftpFileStream(SftpSession session, string path, FileMode mode, FileAccess access)
			: this(session, path, mode, access, 4096)
		{
		}

		internal SftpFileStream(SftpSession session, string path, FileMode mode, FileAccess access, int bufferSize)
			: this(session, path, mode, access, bufferSize, useAsync: false)
		{
		}

		internal SftpFileStream(SftpSession session, string path, FileMode mode, FileAccess access, int bufferSize, bool useAsync)
		{
			if (session == null)
			{
				throw new SshConnectionException("Client not connected.");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access");
			}
			if (mode < FileMode.CreateNew || mode > FileMode.Append)
			{
				throw new ArgumentOutOfRangeException("mode");
			}
			Timeout = TimeSpan.FromSeconds(30.0);
			Name = path;
			_session = session;
			_access = access;
			_ownsHandle = true;
			_isAsync = useAsync;
			_bufferPosition = 0;
			_bufferLen = 0;
			_bufferOwnedByWrite = false;
			_canSeek = true;
			_position = 0L;
			_serverFilePosition = 0uL;
			_session.Disconnected += Session_Disconnected;
			Flags flags = Flags.None;
			switch (access)
			{
			case FileAccess.Read:
				flags |= Flags.Read;
				break;
			case FileAccess.Write:
				flags |= Flags.Write;
				break;
			case FileAccess.ReadWrite:
				flags |= Flags.Read;
				flags |= Flags.Write;
				break;
			}
			switch (mode)
			{
			case FileMode.Append:
				flags |= Flags.Append;
				break;
			case FileMode.Create:
				_handle = _session.RequestOpen(path, flags | Flags.Truncate, nullOnError: true);
				flags = ((_handle != null) ? (flags | Flags.Truncate) : (flags | Flags.CreateNew));
				break;
			case FileMode.CreateNew:
				flags |= Flags.CreateNew;
				break;
			case FileMode.OpenOrCreate:
				flags |= Flags.CreateNewOrOpen;
				break;
			case FileMode.Truncate:
				flags |= Flags.Truncate;
				break;
			}
			if (_handle == null)
			{
				_handle = _session.RequestOpen(path, flags);
			}
			_attributes = _session.RequestFStat(_handle);
			_readBufferSize = (int)session.CalculateOptimalReadLength((uint)bufferSize);
			_readBuffer = new byte[_readBufferSize];
			_writeBufferSize = (int)session.CalculateOptimalWriteLength((uint)bufferSize, _handle);
			_writeBuffer = new byte[_writeBufferSize];
			if (mode == FileMode.Append)
			{
				_position = _attributes.Size;
				_serverFilePosition = (ulong)_attributes.Size;
			}
		}

		~SftpFileStream()
		{
			Dispose(disposing: false);
		}

		public override void Close()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public override void Flush()
		{
			lock (_lock)
			{
				if (_handle == null)
				{
					throw new ObjectDisposedException("Stream is closed.");
				}
				if (_bufferOwnedByWrite)
				{
					FlushWriteBuffer();
				}
				else
				{
					FlushReadBuffer();
				}
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Invalid array range.");
			}
			lock (_lock)
			{
				SetupRead();
				while (true)
				{
					if (count <= 0)
					{
						return num;
					}
					int num2 = _bufferLen - _bufferPosition;
					if (num2 <= 0)
					{
						_bufferPosition = 0;
						byte[] array = _session.RequestRead(_handle, (ulong)_position, (uint)_readBufferSize);
						_bufferLen = array.Length;
						Buffer.BlockCopy(array, 0, _readBuffer, 0, _bufferLen);
						_serverFilePosition = (ulong)_position;
						if (_bufferLen < 0)
						{
							_bufferLen = 0;
							throw new IOException("Read operation failed.");
						}
						if (_bufferLen == 0)
						{
							break;
						}
						num2 = _bufferLen;
					}
					if (num2 > count)
					{
						num2 = count;
					}
					Buffer.BlockCopy(_readBuffer, _bufferPosition, buffer, offset, num2);
					num += num2;
					offset += num2;
					count -= num2;
					_bufferPosition += num2;
					_position += num2;
				}
				return num;
			}
		}

		public override int ReadByte()
		{
			lock (_lock)
			{
				SetupRead();
				if (_bufferPosition >= _bufferLen)
				{
					_bufferPosition = 0;
					byte[] array = _session.RequestRead(_handle, (ulong)_position, (uint)_readBufferSize);
					_bufferLen = array.Length;
					Buffer.BlockCopy(array, 0, _readBuffer, 0, _readBufferSize);
					_serverFilePosition = (ulong)_position;
					if (_bufferLen < 0)
					{
						_bufferLen = 0;
						throw new IOException("Read operation failed.");
					}
					if (_bufferLen == 0)
					{
						return -1;
					}
				}
				_position++;
				return _readBuffer[_bufferPosition++];
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = -1L;
			if (!_canSeek)
			{
				throw new NotSupportedException("Seek is not supported.");
			}
			lock (_lock)
			{
				if (_handle == null)
				{
					throw new ObjectDisposedException("Stream is closed.");
				}
				if (origin == SeekOrigin.Begin && offset == _position)
				{
					return offset;
				}
				if (origin == SeekOrigin.Current && offset == 0)
				{
					return _position;
				}
				_attributes = _session.RequestFStat(_handle);
				if (_bufferOwnedByWrite)
				{
					FlushWriteBuffer();
					switch (origin)
					{
					case SeekOrigin.Begin:
						num = offset;
						break;
					case SeekOrigin.Current:
						num = _position + offset;
						break;
					case SeekOrigin.End:
						num = _attributes.Size - offset;
						break;
					}
					if (num == -1)
					{
						throw new EndOfStreamException("End of stream.");
					}
					_position = num;
					_serverFilePosition = (ulong)num;
				}
				else
				{
					switch (origin)
					{
					case SeekOrigin.Begin:
						num = _position - _bufferPosition;
						if (offset >= num && offset < num + _bufferLen)
						{
							_bufferPosition = (int)(offset - num);
							_position = offset;
							return _position;
						}
						break;
					case SeekOrigin.Current:
						num = _position + offset;
						if (num >= _position - _bufferPosition && num < _position - _bufferPosition + _bufferLen)
						{
							_bufferPosition = (int)(num - (_position - _bufferPosition));
							_position = num;
							return _position;
						}
						break;
					}
					_bufferPosition = 0;
					_bufferLen = 0;
					switch (origin)
					{
					case SeekOrigin.Begin:
						num = offset;
						break;
					case SeekOrigin.Current:
						num = _position + offset;
						break;
					case SeekOrigin.End:
						num = _attributes.Size - offset;
						break;
					}
					if (num < 0)
					{
						throw new EndOfStreamException();
					}
					_position = num;
				}
				return _position;
			}
		}

		public override void SetLength(long value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (!_canSeek)
			{
				throw new NotSupportedException("Seek is not supported.");
			}
			lock (_lock)
			{
				SetupWrite();
				_attributes.Size = value;
				_session.RequestFSetStat(_handle, _attributes);
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Invalid array range.");
			}
			lock (_lock)
			{
				SetupWrite();
				while (count > 0)
				{
					int num = _writeBufferSize - _bufferPosition;
					if (num <= 0)
					{
						byte[] array = new byte[_bufferPosition];
						Buffer.BlockCopy(_writeBuffer, 0, array, 0, _bufferPosition);
						using (AutoResetEvent wait = new AutoResetEvent(initialState: false))
						{
							_session.RequestWrite(_handle, _serverFilePosition, array, wait);
							_serverFilePosition += (ulong)array.Length;
						}
						_bufferPosition = 0;
						num = _writeBufferSize;
					}
					if (num > count)
					{
						num = count;
					}
					if (_bufferPosition == 0 && num == _writeBufferSize)
					{
						byte[] array2 = new byte[num];
						Buffer.BlockCopy(buffer, offset, array2, 0, num);
						using (AutoResetEvent wait2 = new AutoResetEvent(initialState: false))
						{
							_session.RequestWrite(_handle, _serverFilePosition, array2, wait2);
							_serverFilePosition += (ulong)array2.Length;
						}
					}
					else
					{
						Buffer.BlockCopy(buffer, offset, _writeBuffer, _bufferPosition, num);
						_bufferPosition += num;
					}
					_position += num;
					offset += num;
					count -= num;
				}
				if (_bufferPosition >= _writeBufferSize)
				{
					byte[] array3 = new byte[_bufferPosition];
					Buffer.BlockCopy(_writeBuffer, 0, array3, 0, _bufferPosition);
					using (AutoResetEvent wait3 = new AutoResetEvent(initialState: false))
					{
						_session.RequestWrite(_handle, _serverFilePosition, array3, wait3);
						_serverFilePosition += (ulong)array3.Length;
					}
					_bufferPosition = 0;
				}
			}
		}

		public override void WriteByte(byte value)
		{
			lock (_lock)
			{
				SetupWrite();
				if (_bufferPosition >= _writeBufferSize)
				{
					byte[] array = new byte[_bufferPosition];
					Buffer.BlockCopy(_writeBuffer, 0, array, 0, _bufferPosition);
					using (AutoResetEvent wait = new AutoResetEvent(initialState: false))
					{
						_session.RequestWrite(_handle, _serverFilePosition, array, wait);
						_serverFilePosition += (ulong)array.Length;
					}
					_bufferPosition = 0;
				}
				_writeBuffer[_bufferPosition++] = value;
				_position++;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (_session != null)
			{
				lock (_lock)
				{
					if (_session != null)
					{
						if (_handle != null)
						{
							if (_bufferOwnedByWrite)
							{
								FlushWriteBuffer();
							}
							if (_ownsHandle)
							{
								_session.RequestClose(_handle);
							}
							_handle = null;
						}
						_session.Disconnected -= Session_Disconnected;
						_session = null;
					}
				}
			}
		}

		private void FlushReadBuffer()
		{
			if (_canSeek)
			{
				if (_bufferPosition < _bufferLen)
				{
					_position -= _bufferPosition;
				}
				_bufferPosition = 0;
				_bufferLen = 0;
			}
		}

		private void FlushWriteBuffer()
		{
			if (_bufferPosition > 0)
			{
				byte[] array = new byte[_bufferPosition];
				Buffer.BlockCopy(_writeBuffer, 0, array, 0, _bufferPosition);
				using (AutoResetEvent wait = new AutoResetEvent(initialState: false))
				{
					_session.RequestWrite(_handle, _serverFilePosition, array, wait);
					_serverFilePosition += (ulong)array.Length;
				}
				_bufferPosition = 0;
			}
		}

		private void SetupRead()
		{
			if ((_access & FileAccess.Read) == (FileAccess)0)
			{
				throw new NotSupportedException("Read not supported.");
			}
			if (_handle == null)
			{
				throw new ObjectDisposedException("Stream is closed.");
			}
			if (_bufferOwnedByWrite)
			{
				FlushWriteBuffer();
				_bufferOwnedByWrite = false;
			}
		}

		private void SetupWrite()
		{
			if ((_access & FileAccess.Write) == (FileAccess)0)
			{
				throw new NotSupportedException("Write not supported.");
			}
			if (_handle == null)
			{
				throw new ObjectDisposedException("Stream is closed.");
			}
			if (!_bufferOwnedByWrite)
			{
				FlushReadBuffer();
				_bufferOwnedByWrite = true;
			}
		}

		private void Session_Disconnected(object sender, EventArgs e)
		{
			lock (_lock)
			{
				_session.Disconnected -= Session_Disconnected;
				_session = null;
			}
		}
	}
}
