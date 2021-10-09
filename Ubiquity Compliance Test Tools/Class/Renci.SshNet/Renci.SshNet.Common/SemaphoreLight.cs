using System;
using System.Threading;

namespace Renci.SshNet.Common
{
	public class SemaphoreLight
	{
		private readonly object _lock = new object();

		private int _currentCount;

		public int CurrentCount => _currentCount;

		public SemaphoreLight(int initialCount)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount", "The value cannot be negative.");
			}
			_currentCount = initialCount;
		}

		public int Release()
		{
			return Release(1);
		}

		public int Release(int releaseCount)
		{
			int currentCount = _currentCount;
			lock (_lock)
			{
				_currentCount += releaseCount;
				Monitor.Pulse(_lock);
				return currentCount;
			}
		}

		public void Wait()
		{
			lock (_lock)
			{
				while (_currentCount < 1)
				{
					Monitor.Wait(_lock);
				}
				_currentCount--;
				Monitor.Pulse(_lock);
			}
		}
	}
}
