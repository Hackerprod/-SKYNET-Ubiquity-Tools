namespace Renci.SshNet.Sftp
{
	internal enum StatusCodes : uint
	{
		Ok,
		Eof,
		NoSuchFile,
		PermissionDenied,
		Failure,
		BadMessage,
		NoConnection,
		ConnectionLost,
		OperationUnsupported,
		InvalidHandle,
		NoSuchPath,
		FileAlreadyExists,
		WriteProtect,
		NoMedia,
		NoSpaceOnFilesystem,
		QuotaExceeded,
		UnknownPrincipal,
		LockConflict,
		DirNotEmpty,
		NotDirectory,
		InvalidFilename,
		LinkLoop,
		CannotDelete,
		InvalidParameter,
		FileIsADirectory,
		ByteRangeLockConflict,
		ByteRangeLockRefused,
		DeletePending,
		FileCorrupt,
		OwnerInvalid,
		GroupInvalid,
		NoMatchingByteRangeLock
	}
}