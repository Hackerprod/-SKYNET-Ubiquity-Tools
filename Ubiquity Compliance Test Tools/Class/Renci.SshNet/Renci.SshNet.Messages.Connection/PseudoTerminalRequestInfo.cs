using Renci.SshNet.Common;
using System.Collections.Generic;

namespace Renci.SshNet.Messages.Connection
{
	internal class PseudoTerminalRequestInfo : RequestInfo
	{
		public const string NAME = "pty-req";

		public override string RequestName => "pty-req";

		public string EnvironmentVariable
		{
			get;
			set;
		}

		public uint Columns
		{
			get;
			set;
		}

		public uint Rows
		{
			get;
			set;
		}

		public uint PixelWidth
		{
			get;
			set;
		}

		public uint PixelHeight
		{
			get;
			set;
		}

		public IDictionary<TerminalModes, uint> TerminalModeValues
		{
			get;
			set;
		}

		public PseudoTerminalRequestInfo()
		{
			base.WantReply = true;
		}

		public PseudoTerminalRequestInfo(string environmentVariable, uint columns, uint rows, uint width, uint height, IDictionary<TerminalModes, uint> terminalModeValues)
			: this()
		{
			EnvironmentVariable = environmentVariable;
			Columns = columns;
			Rows = rows;
			PixelWidth = width;
			PixelHeight = height;
			TerminalModeValues = terminalModeValues;
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(EnvironmentVariable);
			Write(Columns);
			Write(Rows);
			Write(Rows);
			Write(PixelHeight);
			if (TerminalModeValues != null)
			{
				Write((uint)(TerminalModeValues.Count * 5 + 1));
				foreach (KeyValuePair<TerminalModes, uint> terminalModeValue in TerminalModeValues)
				{
					Write((byte)terminalModeValue.Key);
					Write(terminalModeValue.Value);
				}
				Write(0);
			}
			else
			{
				Write(0u);
			}
		}
	}
}
