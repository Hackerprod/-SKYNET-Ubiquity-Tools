namespace Renci.SshNet.Sftp
{
	internal enum Flags
	{
		None = 0,
		Read = 1,
		Write = 2,
		Append = 4,
		CreateNewOrOpen = 8,
		Truncate = 0x10,
		CreateNew = 40
	}
}
