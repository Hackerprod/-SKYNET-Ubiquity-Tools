using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Renci.SshNet
{
	public class ScpClient : BaseClient
	{
		private static readonly Regex _fileInfoRe = new Regex("C(?<mode>\\d{4}) (?<length>\\d+) (?<filename>.+)");

		private static char[] _byteToChar;

		private static readonly Regex _directoryInfoRe = new Regex("D(?<mode>\\d{4}) (?<length>\\d+) (?<filename>.+)");

		private static readonly Regex _timestampRe = new Regex("T(?<mtime>\\d+) 0 (?<atime>\\d+) 0");

		public TimeSpan OperationTimeout
		{
			get;
			set;
		}

		public uint BufferSize
		{
			get;
			set;
		}

		public event EventHandler<ScpDownloadEventArgs> Downloading;

		public event EventHandler<ScpUploadEventArgs> Uploading;

		public ScpClient(ConnectionInfo connectionInfo)
			: this(connectionInfo, ownsConnectionInfo: false)
		{
		}

		public ScpClient(string host, int port, string username, string password)
			: this(new PasswordConnectionInfo(host, port, username, password), ownsConnectionInfo: true)
		{
		}

		public ScpClient(string host, string username, string password)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, password)
		{
		}

		public ScpClient(string host, int port, string username, params PrivateKeyFile[] keyFiles)
			: this(new PrivateKeyConnectionInfo(host, port, username, keyFiles), ownsConnectionInfo: true)
		{
		}

		public ScpClient(string host, string username, params PrivateKeyFile[] keyFiles)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, keyFiles)
		{
		}

		private ScpClient(ConnectionInfo connectionInfo, bool ownsConnectionInfo)
			: base(connectionInfo, ownsConnectionInfo)
		{
			OperationTimeout = new TimeSpan(0, 0, 0, 0, -1);
			BufferSize = 16384u;
			if (_byteToChar == null)
			{
				_byteToChar = new char[128];
				char c = '\0';
				for (int i = 0; i < 128; i++)
				{
					char[] byteToChar = _byteToChar;
					int num = i;
					char num2 = c;
					c = (char)(num2 + 1);
					byteToChar[num] = num2;
				}
			}
		}

		public void Upload(Stream source, string path)
		{
			PipeStream input = new PipeStream();
			try
			{
				using (ChannelSession channelSession = base.Session.CreateClientChannel<ChannelSession>())
				{
					channelSession.DataReceived += delegate(object sender, ChannelDataEventArgs e)
					{
						input.Write(e.Data, 0, e.Data.Length);
						input.Flush();
					};
					channelSession.Open();
					int num = path.LastIndexOfAny(new char[2]
					{
						'\\',
						'/'
					});
					if (num != -1)
					{
						string arg = path.Substring(0, num);
						string text = path.Substring(num + 1);
						channelSession.SendExecRequest($"scp -t \"{arg}\"");
						CheckReturnCode(input);
						path = text;
					}
					InternalUpload(channelSession, input, source, path);
					channelSession.Close();
				}
			}
			finally
			{
				if (input != null)
				{
					((IDisposable)input).Dispose();
				}
			}
		}

		public void Download(string filename, Stream destination)
		{
			if (filename.IsNullOrWhiteSpace())
			{
				throw new ArgumentException("filename");
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			PipeStream input = new PipeStream();
			try
			{
				using (ChannelSession channelSession = base.Session.CreateClientChannel<ChannelSession>())
				{
					channelSession.DataReceived += delegate(object sender, ChannelDataEventArgs e)
					{
						input.Write(e.Data, 0, e.Data.Length);
						input.Flush();
					};
					channelSession.Open();
					channelSession.SendExecRequest($"scp -f \"{filename}\"");
					SendConfirmation(channelSession);
					string text = ReadString(input);
					Match match = _fileInfoRe.Match(text);
					if (match.Success)
					{
						SendConfirmation(channelSession);
						match.Result("${mode}");
						long length = long.Parse(match.Result("${length}"));
						string filename2 = match.Result("${filename}");
						InternalDownload(channelSession, input, destination, filename2, length);
					}
					else
					{
						SendConfirmation(channelSession, 1, $"\"{text}\" is not valid protocol message.");
					}
					channelSession.Close();
				}
			}
			finally
			{
				if (input != null)
				{
					((IDisposable)input).Dispose();
				}
			}
		}

		private void InternalSetTimestamp(ChannelSession channel, Stream input, DateTime lastWriteTime, DateTime lastAccessime)
		{
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			long num = (long)(lastWriteTime - d).TotalSeconds;
			long num2 = (long)(lastAccessime - d).TotalSeconds;
			SendData(channel, $"T{num} 0 {num2} 0\n");
			CheckReturnCode(input);
		}

		private void InternalUpload(ChannelSession channel, Stream input, Stream source, string filename)
		{
			long length = source.Length;
			SendData(channel, $"C0644 {length} {Path.GetFileName(filename)}\n");
			byte[] array = new byte[BufferSize];
			int num = source.Read(array, 0, array.Length);
			long num2 = 0L;
			while (num > 0)
			{
				SendData(channel, array, num);
				num2 += num;
				RaiseUploadingEvent(filename, length, num2);
				num = source.Read(array, 0, array.Length);
			}
			SendConfirmation(channel);
			CheckReturnCode(input);
		}

		private void InternalDownload(ChannelSession channel, Stream input, Stream output, string filename, long length)
		{
			byte[] buffer = new byte[Math.Min(length, BufferSize)];
			long num = length;
			do
			{
				int num2 = input.Read(buffer, 0, (int)Math.Min(num, BufferSize));
				output.Write(buffer, 0, num2);
				RaiseDownloadingEvent(filename, length, length - num);
				num -= num2;
			}
			while (num > 0);
			output.Flush();
			RaiseDownloadingEvent(filename, length, length - num);
			SendConfirmation(channel);
			CheckReturnCode(input);
		}

		private void RaiseDownloadingEvent(string filename, long size, long downloaded)
		{
			if (this.Downloading != null)
			{
				this.Downloading(this, new ScpDownloadEventArgs(filename, size, downloaded));
			}
		}

		private void RaiseUploadingEvent(string filename, long size, long uploaded)
		{
			if (this.Uploading != null)
			{
				this.Uploading(this, new ScpUploadEventArgs(filename, size, uploaded));
			}
		}

		private void SendConfirmation(ChannelSession channel)
		{
			byte[] buffer = new byte[1];
			SendData(channel, buffer);
		}

		private void SendConfirmation(ChannelSession channel, byte errorCode, string message)
		{
			SendData(channel, new byte[1]
			{
				errorCode
			});
			SendData(channel, $"{message}\n");
		}

		private void CheckReturnCode(Stream input)
		{
			int num = ReadByte(input);
			if (num > 0)
			{
				string message = ReadString(input);
				throw new ScpException(message);
			}
		}

		private void SendData(ChannelSession channel, byte[] buffer, int length)
		{
			if (length == buffer.Length)
			{
				channel.SendData(buffer);
			}
			else
			{
				channel.SendData(buffer.Take(length).ToArray());
			}
		}

		private void SendData(ChannelSession channel, byte[] buffer)
		{
			channel.SendData(buffer);
		}

		private static int ReadByte(Stream stream)
		{
			int num;
			for (num = stream.ReadByte(); num < 0; num = stream.ReadByte())
			{
				Thread.Sleep(100);
			}
			return num;
		}

		private static string ReadString(Stream stream)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			int num = ReadByte(stream);
			if (num == 1 || num == 2)
			{
				flag = true;
				num = ReadByte(stream);
			}
			for (char c = _byteToChar[num]; c != '\n'; c = _byteToChar[num])
			{
				stringBuilder.Append(c);
				num = ReadByte(stream);
			}
			if (flag)
			{
				throw new ScpException(stringBuilder.ToString());
			}
			return stringBuilder.ToString();
		}

		public void Upload(FileInfo fileInfo, string path)
		{
			if (fileInfo == null)
			{
				throw new ArgumentNullException("fileInfo");
			}
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("path");
			}
			PipeStream input = new PipeStream();
			try
			{
				using (ChannelSession channelSession = base.Session.CreateClientChannel<ChannelSession>())
				{
					channelSession.DataReceived += delegate(object sender, ChannelDataEventArgs e)
					{
						input.Write(e.Data, 0, e.Data.Length);
						input.Flush();
					};
					channelSession.Open();
					channelSession.SendExecRequest($"scp -t \"{path}\"");
					CheckReturnCode(input);
					InternalUpload(channelSession, input, fileInfo, fileInfo.Name);
					channelSession.Close();
				}
			}
			finally
			{
				if (input != null)
				{
					((IDisposable)input).Dispose();
				}
			}
		}

		public void Upload(DirectoryInfo directoryInfo, string path)
		{
			if (directoryInfo == null)
			{
				throw new ArgumentNullException("directoryInfo");
			}
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("path");
			}
			PipeStream input = new PipeStream();
			try
			{
				using (ChannelSession channelSession = base.Session.CreateClientChannel<ChannelSession>())
				{
					channelSession.DataReceived += delegate(object sender, ChannelDataEventArgs e)
					{
						input.Write(e.Data, 0, e.Data.Length);
						input.Flush();
					};
					channelSession.Open();
					channelSession.SendExecRequest($"scp -rt \"{path}\"");
					CheckReturnCode(input);
					InternalSetTimestamp(channelSession, input, directoryInfo.LastWriteTimeUtc, directoryInfo.LastAccessTimeUtc);
					SendData(channelSession, $"D0755 0 {Path.GetFileName(path)}\n");
					CheckReturnCode(input);
					InternalUpload(channelSession, input, directoryInfo);
					SendData(channelSession, "E\n");
					CheckReturnCode(input);
					channelSession.Close();
				}
			}
			finally
			{
				if (input != null)
				{
					((IDisposable)input).Dispose();
				}
			}
		}

		public void Download(string filename, FileInfo fileInfo)
		{
			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentException("filename");
			}
			if (fileInfo == null)
			{
				throw new ArgumentNullException("fileInfo");
			}
			PipeStream input = new PipeStream();
			try
			{
				using (ChannelSession channelSession = base.Session.CreateClientChannel<ChannelSession>())
				{
					channelSession.DataReceived += delegate(object sender, ChannelDataEventArgs e)
					{
						input.Write(e.Data, 0, e.Data.Length);
						input.Flush();
					};
					channelSession.Open();
					channelSession.SendExecRequest($"scp -pf \"{filename}\"");
					SendConfirmation(channelSession);
					InternalDownload(channelSession, input, fileInfo);
					channelSession.Close();
				}
			}
			finally
			{
				if (input != null)
				{
					((IDisposable)input).Dispose();
				}
			}
		}

		public void Download(string directoryName, DirectoryInfo directoryInfo)
		{
			if (string.IsNullOrEmpty(directoryName))
			{
				throw new ArgumentException("directoryName");
			}
			if (directoryInfo == null)
			{
				throw new ArgumentNullException("directoryInfo");
			}
			PipeStream input = new PipeStream();
			try
			{
				using (ChannelSession channelSession = base.Session.CreateClientChannel<ChannelSession>())
				{
					channelSession.DataReceived += delegate(object sender, ChannelDataEventArgs e)
					{
						input.Write(e.Data, 0, e.Data.Length);
						input.Flush();
					};
					channelSession.Open();
					channelSession.SendExecRequest($"scp -prf \"{directoryName}\"");
					SendConfirmation(channelSession);
					InternalDownload(channelSession, input, directoryInfo);
					channelSession.Close();
				}
			}
			finally
			{
				if (input != null)
				{
					((IDisposable)input).Dispose();
				}
			}
		}

		private void InternalUpload(ChannelSession channel, Stream input, FileInfo fileInfo, string filename)
		{
			InternalSetTimestamp(channel, input, fileInfo.LastWriteTimeUtc, fileInfo.LastAccessTimeUtc);
			using (FileStream source = fileInfo.OpenRead())
			{
				InternalUpload(channel, input, source, filename);
			}
		}

		private void InternalUpload(ChannelSession channel, Stream input, DirectoryInfo directoryInfo)
		{
			FileInfo[] files = directoryInfo.GetFiles();
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				InternalUpload(channel, input, fileInfo, fileInfo.Name);
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			DirectoryInfo[] array2 = directories;
			foreach (DirectoryInfo directoryInfo2 in array2)
			{
				InternalSetTimestamp(channel, input, directoryInfo.LastWriteTimeUtc, directoryInfo.LastAccessTimeUtc);
				SendData(channel, $"D0755 0 {directoryInfo2.Name}\n");
				CheckReturnCode(input);
				InternalUpload(channel, input, directoryInfo2);
				SendData(channel, "E\n");
				CheckReturnCode(input);
			}
		}

		private void InternalDownload(ChannelSession channel, Stream input, FileSystemInfo fileSystemInfo)
		{
			DateTime lastWriteTime = DateTime.Now;
			DateTime lastAccessTime = DateTime.Now;
			string fullName = fileSystemInfo.FullName;
			string text = fullName;
			int num = 0;
			while (true)
			{
				string text2 = ReadString(input);
				if (text2 == "E")
				{
					SendConfirmation(channel);
					num--;
					text = new DirectoryInfo(text).Parent.FullName;
					if (num == 0)
					{
						break;
					}
				}
				else
				{
					Match match = _directoryInfoRe.Match(text2);
					if (match.Success)
					{
						SendConfirmation(channel);
						long.Parse(match.Result("${mode}"));
						string arg = match.Result("${filename}");
						DirectoryInfo directoryInfo;
						if (num > 0)
						{
							directoryInfo = Directory.CreateDirectory($"{text}{Path.DirectorySeparatorChar}{arg}");
							directoryInfo.LastAccessTime = lastAccessTime;
							directoryInfo.LastWriteTime = lastWriteTime;
						}
						else
						{
							directoryInfo = (fileSystemInfo as DirectoryInfo);
						}
						num++;
						text = directoryInfo.FullName;
					}
					else
					{
						match = _fileInfoRe.Match(text2);
						if (match.Success)
						{
							SendConfirmation(channel);
							match.Result("${mode}");
							long length = long.Parse(match.Result("${length}"));
							string text3 = match.Result("${filename}");
							FileInfo fileInfo = fileSystemInfo as FileInfo;
							if (fileInfo == null)
							{
								fileInfo = new FileInfo($"{text}{Path.DirectorySeparatorChar}{text3}");
							}
							using (FileStream output = fileInfo.OpenWrite())
							{
								InternalDownload(channel, input, output, text3, length);
							}
							fileInfo.LastAccessTime = lastAccessTime;
							fileInfo.LastWriteTime = lastWriteTime;
							if (num == 0)
							{
								break;
							}
						}
						else
						{
							match = _timestampRe.Match(text2);
							if (match.Success)
							{
								SendConfirmation(channel);
								long num2 = long.Parse(match.Result("${mtime}"));
								long num3 = long.Parse(match.Result("${atime}"));
								DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
								lastWriteTime = dateTime.AddSeconds((double)num2);
								lastAccessTime = dateTime.AddSeconds((double)num3);
							}
							else
							{
								SendConfirmation(channel, 1, $"\"{text2}\" is not valid protocol message.");
							}
						}
					}
				}
			}
		}

		private void SendData(ChannelSession channel, string command)
		{
			channel.SendData(Encoding.Default.GetBytes(command));
		}
	}
}
