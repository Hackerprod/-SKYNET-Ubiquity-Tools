using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Renci.SshNet
{
	public static class Extensions
	{
		private static readonly Regex _rehost = new Regex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$|^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\\-]*[a-zA-Z0-9])\\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\\-]*[A-Za-z0-9])$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex _reIPv6 = new Regex("^(((?=(?>.*?::)(?!.*::)))(::)?([0-9A-F]{1,4}::?){0,5}|([0-9A-F]{1,4}:){6})(\\2([0-9A-F]{1,4}(::?|$)){0,2}|((25[0-5]|(2[0-4]|1\\d|[1-9])?\\d)(\\.|$)){4}|[0-9A-F]{1,4}:[0-9A-F]{1,4})(?<![^:]:|\\.)\\z", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		internal static bool IsEqualTo<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList, IEqualityComparer<TSource> comparer)
		{
			if (value == compareList)
			{
				return true;
			}
			if (value != null && compareList != null)
			{
				if (comparer == null)
				{
					comparer = EqualityComparer<TSource>.Default;
				}
				IEnumerator<TSource> enumerator = value.GetEnumerator();
				IEnumerator<TSource> enumerator2 = compareList.GetEnumerator();
				bool flag = enumerator.MoveNext();
				bool flag2 = enumerator2.MoveNext();
				try
				{
					while (flag && flag2)
					{
						if (!comparer.Equals(enumerator.Current, enumerator2.Current))
						{
							return false;
						}
						flag = enumerator.MoveNext();
						flag2 = enumerator2.MoveNext();
					}
					return !flag && !flag2;
				}
				finally
				{
					enumerator.Dispose();
					enumerator2.Dispose();
				}
			}
			return false;
		}

		internal static bool IsEqualTo<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList)
		{
			return value.IsEqualTo(compareList, null);
		}

		internal static void DebugPrint(this IEnumerable<byte> bytes)
		{
			foreach (byte @byte in bytes)
			{
				byte b = @byte;
			}
		}

		internal static IEnumerable<byte> TrimLeadingZero(this IEnumerable<byte> data)
		{
			bool leadingZero = true;
			foreach (byte datum in data)
			{
				if (datum != 0 || !leadingZero)
				{
					leadingZero = false;
					yield return datum;
				}
			}
		}

		internal static T CreateInstance<T>(this Type type) where T : class
		{
			if ((object)type == null)
			{
				return null;
			}
			return Activator.CreateInstance(type) as T;
		}

		internal static byte[] GetBytes(this ushort value)
		{
			return new byte[2]
			{
				(byte)(value >> 8),
				(byte)(value & 0xFF)
			};
		}

		internal static byte[] GetBytes(this uint value)
		{
			return new byte[4]
			{
				(byte)(value >> 24),
				(byte)(value >> 16),
				(byte)(value >> 8),
				(byte)(value & 0xFF)
			};
		}

		internal static byte[] GetBytes(this ulong value)
		{
			return new byte[8]
			{
				(byte)(value >> 56),
				(byte)(value >> 48),
				(byte)(value >> 40),
				(byte)(value >> 32),
				(byte)(value >> 24),
				(byte)(value >> 16),
				(byte)(value >> 8),
				(byte)(value & 0xFF)
			};
		}

		internal static byte[] GetBytes(this long value)
		{
			return new byte[8]
			{
				(byte)(value >> 56),
				(byte)(value >> 48),
				(byte)(value >> 40),
				(byte)(value >> 32),
				(byte)(value >> 24),
				(byte)(value >> 16),
				(byte)(value >> 8),
				(byte)(value & 0xFF)
			};
		}

		internal static bool IsValidHost(this string value)
		{
			if (value == null)
			{
				return false;
			}
			if (value == string.Empty)
			{
				return true;
			}
			if (_rehost.Match(value).Success)
			{
				return true;
			}
			if (_reIPv6.Match(value).Success)
			{
				return true;
			}
			return false;
		}

		internal static bool IsValidPort(this uint value)
		{
			if (value < 0)
			{
				return false;
			}
			if (value > 65535)
			{
				return false;
			}
			return true;
		}

		internal static bool IsValidPort(this int value)
		{
			if (value < 0)
			{
				return false;
			}
			if (value > 65535)
			{
				return false;
			}
			return true;
		}

		internal static bool IsNullOrWhiteSpace(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return true;
			}
			return value.All(char.IsWhiteSpace);
		}

		internal static bool CanRead(this Socket socket)
		{
			if (socket.Connected && socket.Poll(-1, SelectMode.SelectRead))
			{
				return socket.Available > 0;
			}
			return false;
		}

		internal static bool CanWrite(this Socket socket)
		{
			if (socket.Connected)
			{
				return socket.Poll(-1, SelectMode.SelectWrite);
			}
			return false;
		}

		internal static IPAddress GetIPAddress(this string host)
		{
			if (!IPAddress.TryParse(host, out IPAddress address))
			{
				return Dns.GetHostAddresses(host).First();
			}
			return address;
		}

		[DebuggerNonUserCode]
		internal static void Dispose(this Socket socket)
		{
			if (socket == null)
			{
				throw new NullReferenceException();
			}
			socket.Close();
		}

		[DebuggerNonUserCode]
		internal static void Dispose(this WaitHandle handle)
		{
			if (handle == null)
			{
				throw new NullReferenceException();
			}
			handle.Close();
		}

		[DebuggerNonUserCode]
		internal static void Dispose(this HashAlgorithm algorithm)
		{
			if (algorithm == null)
			{
				throw new NullReferenceException();
			}
			algorithm.Clear();
		}

		public static void Clear(this StringBuilder value)
		{
			value.Length = 0;
			value.Capacity = 16;
		}
	}
}
