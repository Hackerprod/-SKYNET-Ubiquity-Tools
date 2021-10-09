using Renci.SshNet.Common;
using Renci.SshNet.Sftp.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Renci.SshNet.Sftp
{
	internal abstract class SftpMessage : SshData
	{
		protected override int ZeroReaderIndex => 1;

		public abstract SftpMessageTypes SftpMessageType
		{
			get;
		}

		public static SftpMessage Load(uint protocolVersion, byte[] data, Encoding encoding)
		{
			SftpMessageTypes messageType = (SftpMessageTypes)data.FirstOrDefault();
			return Load(protocolVersion, data, messageType, encoding);
		}

		protected override void LoadData()
		{
		}

		protected override void SaveData()
		{
			Write((byte)SftpMessageType);
		}

		protected SftpFileAttributes ReadAttributes()
		{
			uint num = ReadUInt32();
			long size = -1L;
			int userId = -1;
			int groupId = -1;
			uint permissions = 0u;
			DateTime lastAccessTime = DateTime.MinValue;
			DateTime lastWriteTime = DateTime.MinValue;
			IDictionary<string, string> extensions = null;
			if ((num & 1) == 1)
			{
				size = (long)ReadUInt64();
			}
			if ((num & 2) == 2)
			{
				userId = (int)ReadUInt32();
				groupId = (int)ReadUInt32();
			}
			if ((num & 4) == 4)
			{
				permissions = ReadUInt32();
			}
			if ((num & 8) == 8)
			{
				uint num2 = ReadUInt32();
				lastAccessTime = DateTime.FromFileTime((num2 + 11644473600L) * 10000000);
				num2 = ReadUInt32();
				lastWriteTime = DateTime.FromFileTime((num2 + 11644473600L) * 10000000);
			}
			if (((int)num & -2147483648) == -2147483648)
			{
				ReadUInt32();
				extensions = ReadExtensionPair();
			}
			return new SftpFileAttributes(lastAccessTime, lastWriteTime, size, userId, groupId, permissions, extensions);
		}

		protected void Write(SftpFileAttributes attributes)
		{
			if (attributes == null)
			{
				Write(0u);
			}
			else
			{
				uint num = 0u;
				if (attributes.IsSizeChanged && attributes.IsRegularFile)
				{
					num |= 1;
				}
				if (attributes.IsUserIdChanged || attributes.IsGroupIdChanged)
				{
					num |= 2;
				}
				if (attributes.IsPermissionsChanged)
				{
					num |= 4;
				}
				if (attributes.IsLastAccessTimeChanged || attributes.IsLastWriteTimeChanged)
				{
					num |= 8;
				}
				if (attributes.IsExtensionsChanged)
				{
					num = (uint)((int)num | -2147483648);
				}
				Write(num);
				if (attributes.IsSizeChanged && attributes.IsRegularFile)
				{
					Write((ulong)attributes.Size);
				}
				if (attributes.IsUserIdChanged || attributes.IsGroupIdChanged)
				{
					Write((uint)attributes.UserId);
					Write((uint)attributes.GroupId);
				}
				if (attributes.IsPermissionsChanged)
				{
					Write(attributes.Permissions);
				}
				if (attributes.IsLastAccessTimeChanged || attributes.IsLastWriteTimeChanged)
				{
					uint data = (uint)(attributes.LastAccessTime.ToFileTime() / 10000000 - 11644473600L);
					Write(data);
					data = (uint)(attributes.LastWriteTime.ToFileTime() / 10000000 - 11644473600L);
					Write(data);
				}
				if (attributes.IsExtensionsChanged)
				{
					Write(attributes.Extensions);
				}
			}
		}

		private static SftpMessage Load(uint protocolVersion, byte[] data, SftpMessageTypes messageType, Encoding encoding)
		{
			SftpMessage sftpMessage;
			switch (messageType)
			{
			case SftpMessageTypes.Version:
				sftpMessage = new SftpVersionResponse();
				break;
			case SftpMessageTypes.Status:
				sftpMessage = new SftpStatusResponse(protocolVersion);
				break;
			case SftpMessageTypes.Data:
				sftpMessage = new SftpDataResponse(protocolVersion);
				break;
			case SftpMessageTypes.Handle:
				sftpMessage = new SftpHandleResponse(protocolVersion);
				break;
			case SftpMessageTypes.Name:
				sftpMessage = new SftpNameResponse(protocolVersion, encoding);
				break;
			case SftpMessageTypes.Attrs:
				sftpMessage = new SftpAttrsResponse(protocolVersion);
				break;
			case SftpMessageTypes.ExtendedReply:
				sftpMessage = new SftpExtendedReplyResponse(protocolVersion);
				break;
			default:
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Message type '{0}' is not supported.", new object[1]
				{
					messageType
				}));
			}
			sftpMessage.LoadBytes(data);
			sftpMessage.ResetReader();
			sftpMessage.LoadData();
			return sftpMessage;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "SFTP Message : {0}", new object[1]
			{
				SftpMessageType
			});
		}
	}
}
