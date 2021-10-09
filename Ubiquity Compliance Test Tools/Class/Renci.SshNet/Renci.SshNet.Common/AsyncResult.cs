using System;
using System.Threading;

namespace Renci.SshNet.Common
{
	public abstract class AsyncResult : IAsyncResult
	{
		private const int _statePending = 0;

		private const int _stateCompletedSynchronously = 1;

		private const int _stateCompletedAsynchronously = 2;

		private readonly AsyncCallback _asyncCallback;

		private readonly object _asyncState;

		private int _completedState;

		private ManualResetEvent _asyncWaitHandle;

		private Exception _exception;

		public bool EndInvokeCalled
		{
			get;
			private set;
		}

		public object AsyncState => _asyncState;

		public bool CompletedSynchronously => _completedState == 1;

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (_asyncWaitHandle == null)
				{
					bool isCompleted = IsCompleted;
					ManualResetEvent manualResetEvent = new ManualResetEvent(isCompleted);
					if (Interlocked.CompareExchange(ref _asyncWaitHandle, manualResetEvent, null) != null)
					{
						manualResetEvent.Close();
					}
					else if (!isCompleted && IsCompleted)
					{
						_asyncWaitHandle.Set();
					}
				}
				return _asyncWaitHandle;
			}
		}

		public bool IsCompleted => _completedState != 0;

		public AsyncResult(AsyncCallback asyncCallback, object state)
		{
			_asyncCallback = asyncCallback;
			_asyncState = state;
		}

		public void SetAsCompleted(Exception exception, bool completedSynchronously)
		{
			_exception = exception;
			if (Interlocked.Exchange(ref _completedState, completedSynchronously ? 1 : 2) != 0)
			{
				throw new InvalidOperationException("You can set a result only once");
			}
			if (_asyncWaitHandle != null)
			{
				_asyncWaitHandle.Set();
			}
			if (_asyncCallback != null)
			{
				_asyncCallback(this);
			}
		}

		public void EndInvoke()
		{
			if (!IsCompleted)
			{
				AsyncWaitHandle.WaitOne();
				AsyncWaitHandle.Close();
				_asyncWaitHandle = null;
			}
			EndInvokeCalled = true;
			if (_exception != null)
			{
				throw new SshException(_exception.Message, _exception);
			}
		}
	}
	public abstract class AsyncResult<TResult> : AsyncResult
	{
		private TResult _result = default(TResult);

		public AsyncResult(AsyncCallback asyncCallback, object state)
			: base(asyncCallback, state)
		{
		}

		public void SetAsCompleted(TResult result, bool completedSynchronously)
		{
			_result = result;
			SetAsCompleted(null, completedSynchronously);
		}

		public new TResult EndInvoke()
		{
			base.EndInvoke();
			return _result;
		}
	}
}
