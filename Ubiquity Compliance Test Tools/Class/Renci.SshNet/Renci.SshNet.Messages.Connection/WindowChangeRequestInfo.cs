namespace Renci.SshNet.Messages.Connection
{
	internal class WindowChangeRequestInfo : RequestInfo
	{
		public const string NAME = "window-change";

		public override string RequestName => "window-change";

		public uint Columns
		{
			get;
			private set;
		}

		public uint Rows
		{
			get;
			private set;
		}

		public uint Width
		{
			get;
			private set;
		}

		public uint Height
		{
			get;
			private set;
		}

		public WindowChangeRequestInfo()
		{
			base.WantReply = false;
		}

		public WindowChangeRequestInfo(uint columns, uint rows, uint width, uint height)
			: this()
		{
			Columns = columns;
			Rows = rows;
			Width = width;
			Height = height;
		}

		protected override void LoadData()
		{
			base.LoadData();
			Columns = ReadUInt32();
			Rows = ReadUInt32();
			Width = ReadUInt32();
			Height = ReadUInt32();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(Columns);
			Write(Rows);
			Write(Width);
			Write(Height);
		}
	}
}
