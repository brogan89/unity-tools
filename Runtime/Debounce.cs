using System;
using System.Collections;
using UnityEngine;

namespace UnityTools
{
	/// <summary>
	/// A simple debounce implementation which helps with limiting spamming a method or action.
	/// </summary>
	public class Debounce
	{
		private readonly MonoBehaviour _monoBehaviour;
		private readonly float _debounceTime;
		private readonly Action _callback;

		private float _currentTime;
		private bool _isTriggered;

		public Debounce(MonoBehaviour monoBehaviour, float debounceTime, Action callback)
		{
			_monoBehaviour = monoBehaviour;
			_debounceTime = debounceTime;
			_callback = callback;
			_isTriggered = true;
			_monoBehaviour.StartCoroutine(Tick());
		}
		
		private IEnumerator Tick()
		{
			while (_monoBehaviour)
			{
				if (_isTriggered)
				{
					yield return null;
					continue;
				}
				
				_currentTime += Time.unscaledDeltaTime;

				if (_currentTime > _debounceTime)
				{
					_callback?.Invoke();
					_isTriggered = true;
				}
				
				yield return null;
			}
		}

		/// <summary>
		/// Ping the debounce. The will start time timer.
		/// </summary>
		public void Ping()
		{
			_isTriggered = false;
			_currentTime = 0;
		}
	}
}