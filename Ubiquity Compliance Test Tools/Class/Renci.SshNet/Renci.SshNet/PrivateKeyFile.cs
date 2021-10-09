using Renci.SshNet.Common;
using Renci.SshNet.Security;
using Renci.SshNet.Security.Cryptography;
using Renci.SshNet.Security.Cryptography.Ciphers;
using Renci.SshNet.Security.Cryptography.Ciphers.Modes;
using Renci.SshNet.Security.Cryptography.Ciphers.Paddings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Renci.SshNet
{
	public class PrivateKeyFile : IDisposable
	{
		private class SshDataReader : SshData
		{
			public SshDataReader(byte[] data)
			{
				LoadBytes(data);
			}

			public new uint ReadUInt32()
			{
				return base.ReadUInt32();
			}

			public new string ReadString()
			{
				return base.ReadString();
			}

			public new byte[] ReadBytes(int length)
			{
				return base.ReadBytes(length);
			}

			public BigInteger ReadBigIntWithBits()
			{
				int num = (int)base.ReadUInt32();
				num = (num + 7) / 8;
				byte[] array = base.ReadBytes(num);
				byte[] array2 = new byte[array.Length + 1];
				Buffer.BlockCopy(array, 0, array2, 1, array.Length);
				return new BigInteger(array2.Reverse().ToArray());
			}

			protected override void LoadData()
			{
			}

			protected override void SaveData()
			{
			}
		}

		private static readonly Regex _privateKeyRegex = new Regex("^-+ *BEGIN (?<keyName>\\w+( \\w+)*) PRIVATE KEY *-+\\r?\\n(Proc-Type: 4,ENCRYPTED\\r?\\nDEK-Info: (?<cipherName>[A-Z0-9-]+),(?<salt>[A-F0-9]+)\\r?\\n\\r?\\n)?(?<data>([a-zA-Z0-9/+=]{1,80}\\r?\\n)+)-+ *END \\k<keyName> PRIVATE KEY *-+", RegexOptions.Multiline | RegexOptions.Compiled);

		private Key _key;

		private bool _isDisposed;

		public HostAlgorithm HostKey
		{
			get;
			private set;
		}

		public PrivateKeyFile(Stream privateKey)
		{
			Open(privateKey, null);
		}

		public PrivateKeyFile(string fileName)
			: this(fileName, null)
		{
		}

		public PrivateKeyFile(string fileName, string passPhrase)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullException("fileName");
			}
			using (FileStream privateKey = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				Open(privateKey, passPhrase);
			}
		}

		public PrivateKeyFile(Stream privateKey, string passPhrase)
		{
			Open(privateKey, passPhrase);
		}

		private void Open(Stream privateKey, string passPhrase)
		{
			if (privateKey == null)
			{
				throw new ArgumentNullException("privateKey");
			}
			Match match;
			using (StreamReader streamReader = new StreamReader(privateKey))
			{
				string input = streamReader.ReadToEnd();
				match = _privateKeyRegex.Match(input);
			}
			if (!match.Success)
			{
				throw new SshException("Invalid private key file.");
			}
			string text = match.Result("${keyName}");
			string text2 = match.Result("${cipherName}");
			string text3 = match.Result("${salt}");
			string s = match.Result("${data}");
			byte[] array = Convert.FromBase64String(s);
			byte[] array3;
			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				if (string.IsNullOrEmpty(passPhrase))
				{
					throw new SshPassPhraseNullOrEmptyException("Private key is encrypted but passphrase is empty.");
				}
				byte[] array2 = new byte[text3.Length / 2];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = Convert.ToByte(text3.Substring(i * 2, 2), 16);
				}
				CipherInfo cipherInfo;
				switch (text2)
				{
				case "DES-EDE3-CBC":
					cipherInfo = new CipherInfo(192, (byte[] key, byte[] iv) => new TripleDesCipher(key, new CbcCipherMode(iv), new PKCS7Padding()));
					break;
				case "DES-EDE3-CFB":
					cipherInfo = new CipherInfo(192, (byte[] key, byte[] iv) => new TripleDesCipher(key, new CfbCipherMode(iv), new PKCS7Padding()));
					break;
				case "DES-CBC":
					cipherInfo = new CipherInfo(64, (byte[] key, byte[] iv) => new DesCipher(key, new CbcCipherMode(iv), new PKCS7Padding()));
					break;
				case "AES-128-CBC":
					cipherInfo = new CipherInfo(128, (byte[] key, byte[] iv) => new AesCipher(key, new CbcCipherMode(iv), new PKCS7Padding()));
					break;
				case "AES-192-CBC":
					cipherInfo = new CipherInfo(192, (byte[] key, byte[] iv) => new AesCipher(key, new CbcCipherMode(iv), new PKCS7Padding()));
					break;
				case "AES-256-CBC":
					cipherInfo = new CipherInfo(256, (byte[] key, byte[] iv) => new AesCipher(key, new CbcCipherMode(iv), new PKCS7Padding()));
					break;
				default:
					throw new SshException(string.Format(CultureInfo.CurrentCulture, "Private key cipher \"{0}\" is not supported.", new object[1]
					{
						text2
					}));
				}
				array3 = DecryptKey(cipherInfo, array, passPhrase, array2);
			}
			else
			{
				array3 = array;
			}
			switch (text)
			{
			case "RSA":
				_key = new RsaKey(array3.ToArray());
				HostKey = new KeyHostAlgorithm("ssh-rsa", _key);
				break;
			case "DSA":
				_key = new DsaKey(array3.ToArray());
				HostKey = new KeyHostAlgorithm("ssh-dss", _key);
				break;
			case "SSH2 ENCRYPTED":
			{
				SshDataReader sshDataReader = new SshDataReader(array3);
				uint num = sshDataReader.ReadUInt32();
				if (num != 1064303083)
				{
					throw new SshException("Invalid SSH2 private key.");
				}
				sshDataReader.ReadUInt32();
				string text4 = sshDataReader.ReadString();
				string a = sshDataReader.ReadString();
				int num2 = (int)sshDataReader.ReadUInt32();
				if (!(a == "none"))
				{
					throw new SshException($"Cipher method '{text2}' is not supported.");
				}
				byte[] data = sshDataReader.ReadBytes(num2);
				sshDataReader = new SshDataReader(data);
				uint num3 = sshDataReader.ReadUInt32();
				if (num3 + 4 != num2)
				{
					throw new SshException("Invalid passphrase.");
				}
				if (text4 == "if-modn{sign{rsa-pkcs1-sha1},encrypt{rsa-pkcs1v2-oaep}}")
				{
					BigInteger exponent = sshDataReader.ReadBigIntWithBits();
					BigInteger d = sshDataReader.ReadBigIntWithBits();
					BigInteger modulus = sshDataReader.ReadBigIntWithBits();
					BigInteger inverseQ = sshDataReader.ReadBigIntWithBits();
					BigInteger q = sshDataReader.ReadBigIntWithBits();
					BigInteger p = sshDataReader.ReadBigIntWithBits();
					_key = new RsaKey(modulus, exponent, d, p, q, inverseQ);
					HostKey = new KeyHostAlgorithm("ssh-rsa", _key);
				}
				else
				{
					if (!(text4 == "dl-modp{sign{dsa-nist-sha1},dh{plain}}"))
					{
						throw new NotSupportedException($"Key type '{text4}' is not supported.");
					}
					if (sshDataReader.ReadUInt32() != 0)
					{
						throw new SshException("Invalid private key");
					}
					BigInteger p2 = sshDataReader.ReadBigIntWithBits();
					BigInteger g = sshDataReader.ReadBigIntWithBits();
					BigInteger q2 = sshDataReader.ReadBigIntWithBits();
					BigInteger y = sshDataReader.ReadBigIntWithBits();
					BigInteger x = sshDataReader.ReadBigIntWithBits();
					_key = new DsaKey(p2, q2, g, y, x);
					HostKey = new KeyHostAlgorithm("ssh-dss", _key);
				}
				break;
			}
			default:
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Key '{0}' is not supported.", new object[1]
				{
					text
				}));
			}
		}

		private static byte[] GetCipherKey(string passphrase, int length)
		{
			List<byte> list = new List<byte>();
			using (MD5Hash mD5Hash = new MD5Hash())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(passphrase);
				IEnumerable<byte> enumerable = mD5Hash.ComputeHash(bytes.ToArray()).AsEnumerable();
				list.AddRange(enumerable);
				while (list.Count < length)
				{
					enumerable = bytes.Concat(enumerable);
					enumerable = mD5Hash.ComputeHash(enumerable.ToArray());
					list.AddRange(enumerable);
				}
			}
			return list.Take(length).ToArray();
		}

		private static byte[] DecryptKey(CipherInfo cipherInfo, byte[] cipherData, string passPhrase, byte[] binarySalt)
		{
			if (cipherInfo == null)
			{
				throw new ArgumentNullException("cipherInfo");
			}
			if (cipherData == null)
			{
				throw new ArgumentNullException("cipherData");
			}
			if (binarySalt == null)
			{
				throw new ArgumentNullException("binarySalt");
			}
			List<byte> list = new List<byte>();
			using (MD5Hash mD5Hash = new MD5Hash())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(passPhrase);
				IEnumerable<byte> enumerable = bytes.Concat(binarySalt.Take(8));
				IEnumerable<byte> enumerable2 = mD5Hash.ComputeHash(enumerable.ToArray()).AsEnumerable();
				list.AddRange(enumerable2);
				while (list.Count < cipherInfo.KeySize / 8)
				{
					enumerable2 = enumerable2.Concat(enumerable);
					enumerable2 = mD5Hash.ComputeHash(enumerable2.ToArray());
					list.AddRange(enumerable2);
				}
			}
			Cipher cipher = cipherInfo.Cipher(list.ToArray(), binarySalt);
			return cipher.Decrypt(cipherData);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing && _key != null)
				{
					((IDisposable)_key).Dispose();
					_key = null;
				}
				_isDisposed = true;
			}
		}

		~PrivateKeyFile()
		{
			Dispose(disposing: false);
		}
	}
}
