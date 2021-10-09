using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Renci.SshNet
{
	public class SshClient : BaseClient
	{
		private readonly List<ForwardedPort> _forwardedPorts;

		private Stream _inputStream;

		public IEnumerable<ForwardedPort> ForwardedPorts => _forwardedPorts.AsReadOnly();

		public SshClient(ConnectionInfo connectionInfo)
			: this(connectionInfo, ownsConnectionInfo: false)
		{
		}

		public SshClient(string host, int port, string username, string password)
			: this(new PasswordConnectionInfo(host, port, username, password), ownsConnectionInfo: true)
		{
		}

		public SshClient(string host, string username, string password)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, password)
		{
		}

		public SshClient(string host, int port, string username, params PrivateKeyFile[] keyFiles)
			: this(new PrivateKeyConnectionInfo(host, port, username, keyFiles), ownsConnectionInfo: true)
		{
		}

		public SshClient(string host, string username, params PrivateKeyFile[] keyFiles)
			: this(host, ConnectionInfo.DEFAULT_PORT, username, keyFiles)
		{
		}

		private SshClient(ConnectionInfo connectionInfo, bool ownsConnectionInfo)
			: base(connectionInfo, ownsConnectionInfo)
		{
			_forwardedPorts = new List<ForwardedPort>();
		}

		protected override void OnDisconnecting()
		{
			base.OnDisconnecting();
			foreach (ForwardedPort forwardedPort in _forwardedPorts)
			{
				forwardedPort.Stop();
			}
		}

		public void AddForwardedPort(ForwardedPort port)
		{
			if (port == null)
			{
				throw new ArgumentNullException("port");
			}
			if (port.Session != null && port.Session != base.Session)
			{
				throw new InvalidOperationException("Forwarded port is already added to a different client.");
			}
			port.Session = base.Session;
			_forwardedPorts.Add(port);
		}

		public void RemoveForwardedPort(ForwardedPort port)
		{
			if (port == null)
			{
				throw new ArgumentNullException("port");
			}
			port.Stop();
			port.Session = null;
			_forwardedPorts.Remove(port);
		}

		public SshCommand CreateCommand(string commandText)
		{
			return CreateCommand(commandText, base.ConnectionInfo.Encoding);
		}

		public SshCommand CreateCommand(string commandText, Encoding encoding)
		{
			base.ConnectionInfo.Encoding = encoding;
			return new SshCommand(base.Session, commandText);
		}

		public SshCommand RunCommand(string commandText)
		{
			SshCommand sshCommand = CreateCommand(commandText);
			sshCommand.Execute();
			return sshCommand;
		}

		public Shell CreateShell(Stream input, Stream output, Stream extendedOutput, string terminalName, uint columns, uint rows, uint width, uint height, IDictionary<TerminalModes, uint> terminalModes, int bufferSize)
		{
			return new Shell(base.Session, input, output, extendedOutput, terminalName, columns, rows, width, height, terminalModes, bufferSize);
		}

		public Shell CreateShell(Stream input, Stream output, Stream extendedOutput, string terminalName, uint columns, uint rows, uint width, uint height, IDictionary<TerminalModes, uint> terminalModes)
		{
			return CreateShell(input, output, extendedOutput, terminalName, columns, rows, width, height, terminalModes, 1024);
		}

		public Shell CreateShell(Stream input, Stream output, Stream extendedOutput)
		{
			return CreateShell(input, output, extendedOutput, string.Empty, 0u, 0u, 0u, 0u, null, 1024);
		}

		public Shell CreateShell(Encoding encoding, string input, Stream output, Stream extendedOutput, string terminalName, uint columns, uint rows, uint width, uint height, IDictionary<TerminalModes, uint> terminalModes, int bufferSize)
		{
			_inputStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(_inputStream, encoding);
			streamWriter.Write(input);
			streamWriter.Flush();
			_inputStream.Seek(0L, SeekOrigin.Begin);
			return CreateShell(_inputStream, output, extendedOutput, terminalName, columns, rows, width, height, terminalModes, bufferSize);
		}

		public Shell CreateShell(Encoding encoding, string input, Stream output, Stream extendedOutput, string terminalName, uint columns, uint rows, uint width, uint height, IDictionary<TerminalModes, uint> terminalModes)
		{
			return CreateShell(encoding, input, output, extendedOutput, terminalName, columns, rows, width, height, terminalModes, 1024);
		}

		public Shell CreateShell(Encoding encoding, string input, Stream output, Stream extendedOutput)
		{
			return CreateShell(encoding, input, output, extendedOutput, string.Empty, 0u, 0u, 0u, 0u, null, 1024);
		}

		public ShellStream CreateShellStream(string terminalName, uint columns, uint rows, uint width, uint height, int bufferSize)
		{
			return CreateShellStream(terminalName, columns, rows, width, height, bufferSize, null);
		}

		public ShellStream CreateShellStream(string terminalName, uint columns, uint rows, uint width, uint height, int bufferSize, IDictionary<TerminalModes, uint> terminalModeValues)
		{
			return new ShellStream(base.Session, terminalName, columns, rows, width, height, bufferSize, terminalModeValues);
		}

		protected override void OnDisconnected()
		{
			base.OnDisconnected();
			ForwardedPort[] array = _forwardedPorts.ToArray();
			foreach (ForwardedPort port in array)
			{
				RemoveForwardedPort(port);
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (_inputStream != null)
			{
				_inputStream.Dispose();
				_inputStream = null;
			}
		}
	}
}
