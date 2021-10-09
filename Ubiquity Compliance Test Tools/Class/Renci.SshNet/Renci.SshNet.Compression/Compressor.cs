using Renci.SshNet.Security;
using System;
using System.IO;

namespace Renci.SshNet.Compression
{
	public abstract class Compressor : Algorithm, IDisposable
	{
		private readonly ZlibStream _compressor;

		private readonly ZlibStream _decompressor;

		private MemoryStream _compressorStream;

		private MemoryStream _decompressorStream;

		private bool _isDisposed;

		protected bool IsActive
		{
			get;
			set;
		}

		protected Session Session
		{
			get;
			private set;
		}

		public Compressor()
		{
			_compressorStream = new MemoryStream();
			_decompressorStream = new MemoryStream();
			_compressor = new ZlibStream(_compressorStream, CompressionMode.Compress);
			_decompressor = new ZlibStream(_decompressorStream, CompressionMode.Decompress);
		}

		public virtual void Init(Session session)
		{
			Session = session;
		}

		public virtual byte[] Compress(byte[] data)
		{
			if (!IsActive)
			{
				return data;
			}
			_compressorStream.SetLength(0L);
			_compressor.Write(data, 0, data.Length);
			return _compressorStream.ToArray();
		}

		public virtual byte[] Decompress(byte[] data)
		{
			if (!IsActive)
			{
				return data;
			}
			_decompressorStream.SetLength(0L);
			_decompressor.Write(data, 0, data.Length);
			return _decompressorStream.ToArray();
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
				if (disposing)
				{
					if (_compressorStream != null)
					{
						_compressorStream.Dispose();
						_compressorStream = null;
					}
					if (_decompressorStream != null)
					{
						_decompressorStream.Dispose();
						_decompressorStream = null;
					}
				}
				_isDisposed = true;
			}
		}

		~Compressor()
		{
			Dispose(disposing: false);
		}
	}
}
