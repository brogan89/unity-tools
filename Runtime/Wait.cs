using System;
using System.Collections;
using UnityEngine;

namespace UnityTools
{
	public class Wait : Singleton<Wait>
	{
		public static void Seconds(float seconds, Action callback)
		{
			Instance.StartCoroutine(WaitSecondsCoroutine(seconds, callback));
		}

		private static IEnumerator WaitSecondsCoroutine(float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			callback();
		}
		
		public static void Until(Func<bool> predicate, Action callback)
		{
			Instance.StartCoroutine(WaitUntilCoroutine(predicate, callback));
		}

		private static IEnumerator WaitUntilCoroutine(Func<bool> predicate, Action callback)
		{
			yield return new WaitUntil(predicate);
			callback();
		}
	}
}