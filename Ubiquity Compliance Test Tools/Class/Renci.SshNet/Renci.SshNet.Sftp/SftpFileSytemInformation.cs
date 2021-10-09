namespace Renci.SshNet.Sftp
{
	public class SftpFileSytemInformation
	{
		private const ulong SSH_FXE_STATVFS_ST_RDONLY = 1uL;

		private const ulong SSH_FXE_STATVFS_ST_NOSUID = 2uL;

		private readonly ulong _flag;

		public ulong BlockSize
		{
			get;
			private set;
		}

		public ulong TotalBlocks
		{
			get;
			private set;
		}

		public ulong FreeBlocks
		{
			get;
			private set;
		}

		public ulong AvailableBlocks
		{
			get;
			private set;
		}

		public ulong TotalNodes
		{
			get;
			private set;
		}

		public ulong FreeNodes
		{
			get;
			private set;
		}

		public ulong AvailableNodes
		{
			get;
			private set;
		}

		public ulong Sid
		{
			get;
			private set;
		}

		public bool IsReadOnly => (_flag & 1) == 1;

		public bool SupportsSetUid => (_flag & 2) == 0;

		public ulong MaxNameLenght
		{
			get;
			private set;
		}

		internal SftpFileSytemInformation(ulong bsize, ulong frsize, ulong blocks, ulong bfree, ulong bavail, ulong files, ulong ffree, ulong favail, ulong sid, ulong flag, ulong namemax)
		{
			BlockSize = frsize;
			TotalBlocks = blocks;
			FreeBlocks = bfree;
			AvailableBlocks = bavail;
			TotalNodes = files;
			FreeNodes = ffree;
			AvailableNodes = favail;
			Sid = sid;
			_flag = flag;
			MaxNameLenght = namemax;
		}
	}
}
