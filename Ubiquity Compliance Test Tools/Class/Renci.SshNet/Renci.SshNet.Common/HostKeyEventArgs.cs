using Renci.SshNet.Security;
using Renci.SshNet.Security.Cryptography;
using System;

namespace Renci.SshNet.Common
{
	public class HostKeyEventArgs : EventArgs
	{
		public bool CanTrust
		{
			get;
			set;
		}

		public byte[] HostKey
		{
			get;
			private set;
		}

		public string HostKeyName
		{
			get;
			private set;
		}

		public byte[] FingerPrint
		{
			get;
			private set;
		}

		public int KeyLength
		{
			get;
			private set;
		}

		public HostKeyEventArgs(KeyHostAlgorithm host)
		{
			CanTrust = true;
			HostKey = host.Data;
			HostKeyName = host.Name;
			KeyLength = host.Key.KeyLength;
			using (MD5Hash mD5Hash = new MD5Hash())
			{
				FingerPrint = mD5Hash.ComputeHash(host.Data);
			}
		}
	}
}
