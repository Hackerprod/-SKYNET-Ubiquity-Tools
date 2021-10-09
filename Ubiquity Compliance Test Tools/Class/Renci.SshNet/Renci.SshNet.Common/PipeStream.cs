using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Renci.SshNet.Common
{
	public class PipeStream : Stream
	{
		private readonly Queue<byte> _buffer = new Queue<byte>();

		private bool _isFlushed;

		private long _maxBufferLength = 209715200L;

		private bool _canBlockLastRead;

		public long MaxBufferLength
		{
			get
			{
				return _maxBufferLength;
			}
			set
			{
				_maxBufferLength = value;
			}
		}

		public bool BlockLastReadBuffer
		{
			get
			{
				return _canBlockLastRead;
			}
			set
			{
				_canBlockLastRead = value;
				if (!_canBlockLastRead)
				{
					lock (_buffer)
					{
						Monitor.Pulse(_buffer);
					}
				}
			}
		}

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length => _buffer.Count;

		public override long Position
		{
			get
			{
				return 0L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override void Flush()
		{
			_isFlushed = true;
			lock (_buffer)
			{
				Monitor.Pulse(_buffer);
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (offset != 0)
			{
				throw new NotSupportedException("Offsets with value of non-zero are not supported");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentException("The sum of offset and count is greater than the buffer length. ");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "offset or count is negative.");
			}
			if (BlockLastReadBuffer && count >= _maxBufferLength)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "count({0}) > mMaxBufferLength({1})", new object[2]
				{
					count,
					_maxBufferLength
				}));
			}
			if (count == 0)
			{
				return 0;
			}
			int i = 0;
			lock (_buffer)
			{
				while (!ReadAvailable(count))
				{
					Monitor.Wait(_buffer);
				}
				for (; i < count; i++)
				{
					if (Length <= 0)
					{
						break;
					}
					if (_buffer.Count <= 0)
					{
						break;
					}
					buffer[i] = _buffer.Dequeue();
				}
				Monitor.Pulse(_buffer);
				return i;
			}
		}

		private bool ReadAvailable(int count)
		{
			if (Length >= count || _isFlushed)
			{
				if (Length < count + 1)
				{
					return !BlockLastReadBuffer;
				}
				return true;
			}
			return false;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentException("The sum of offset and count is greater than the buffer length. ");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "offset or count is negative.");
			}
			if (count != 0)
			{
				lock (_buffer)
				{
					while (Length >= _maxBufferLength)
					{
						Monitor.Wait(_buffer);
					}
					_isFlushed = false;
					for (int i = offset; i < offset + count; i++)
					{
						_buffer.Enqueue(buffer[i]);
					}
					Monitor.Pulse(_buffer);
				}
			}
		}
	}
}
