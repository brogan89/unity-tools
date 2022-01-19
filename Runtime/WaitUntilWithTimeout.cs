using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityTools
{
	/// <summary>
	///   <para>Suspends the coroutine execution until the supplied delegate evaluates to true or the given time has expired</para>
	/// </summary>
	public sealed class WaitUntilWithTimeout : CustomYieldInstruction
	{
		private readonly Func<bool> _predicate;
		private float _currentTime;
		private readonly Object _context;

		public override bool keepWaiting
		{
			get
			{
				if (_predicate())
					return false;

				_currentTime -= Time.deltaTime;

				if (_currentTime <= 0)
				{
					Debug.LogWarning("Wait until timed out", _context);
					return false;
				}

				return true;
			}
		}

		public WaitUntilWithTimeout(Func<bool> predicate, float timeout, Object context = null)
		{
			_predicate = predicate;
			_currentTime = timeout;
			_context = context;
		}
	}
}