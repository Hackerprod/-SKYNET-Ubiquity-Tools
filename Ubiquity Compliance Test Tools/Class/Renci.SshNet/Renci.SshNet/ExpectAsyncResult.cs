using Renci.SshNet.Common;
using System;

namespace Renci.SshNet
{
	public class ExpectAsyncResult : AsyncResult<string>
	{
		internal ExpectAsyncResult(AsyncCallback asyncCallback, object state)
			: base(asyncCallback, state)
		{
		}
	}
}
