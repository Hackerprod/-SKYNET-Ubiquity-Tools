using Renci.SshNet.Common;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Renci.SshNet.Messages
{
	public abstract class Message : SshData
	{
		protected override int ZeroReaderIndex => 1;

		public override byte[] GetBytes()
		{
			MessageAttribute messageAttribute = GetType().GetCustomAttributes(typeof(MessageAttribute), inherit: true).SingleOrDefault() as MessageAttribute;
			if (messageAttribute == null)
			{
				throw new SshException(string.Format(CultureInfo.CurrentCulture, "Type '{0}' is not a valid message type.", new object[1]
				{
					GetType().AssemblyQualifiedName
				}));
			}
			List<byte> list = new List<byte>(base.GetBytes());
			list.Insert(0, messageAttribute.Number);
			return list.ToArray();
		}

		public override string ToString()
		{
			MessageAttribute messageAttribute = GetType().GetCustomAttributes(typeof(MessageAttribute), inherit: true).SingleOrDefault() as MessageAttribute;
			if (messageAttribute == null)
			{
				return string.Format(CultureInfo.CurrentCulture, "'{0}' without Message attribute.", new object[1]
				{
					GetType().FullName
				});
			}
			return messageAttribute.Name;
		}
	}
}
