namespace Renci.SshNet.Sftp
{
	internal enum SftpMessageTypes : byte
	{
		Init = 1,
		Version = 2,
		Open = 3,
		Close = 4,
		Read = 5,
		Write = 6,
		LStat = 7,
		FStat = 8,
		SetStat = 9,
		FSetStat = 10,
		OpenDir = 11,
		ReadDir = 12,
		Remove = 13,
		MkDir = 14,
		RmDir = 0xF,
		RealPath = 0x10,
		Stat = 17,
		Rename = 18,
		ReadLink = 19,
		SymLink = 20,
		Link = 21,
		Block = 22,
		Unblock = 23,
		Status = 101,
		Handle = 102,
		Data = 103,
		Name = 104,
		Attrs = 105,
		Extended = 200,
		ExtendedReply = 201
	}
}
