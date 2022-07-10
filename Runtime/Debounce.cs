using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTools
{
	/// <summary>
	/// A simple debounce implementation which helps with limiting spamming a method or action.
	/// </summary>
	public class Debounce
	{
		private static readonly Dictionary<string, Debounce> _debounceMap = new();

		private readonly MonoBehaviour _monoBehaviour;

		private Action _callback;
		private float _debounceTime;
		private float _currentTime;
		private bool _isTriggered;

		private Action OnDied; 

		public Debounce(MonoBehaviour monoBehaviour, float debounceTime, Action callback)
		{
			_monoBehaviour = monoBehaviour;
			_debounceTime = debounceTime;
			_callback = callback;
			_isTriggered = true;
			_monoBehaviour.StartCoroutine(Tick());
		}

		public void SetTime(float value)
		{
			_debounceTime = value;
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
			
			OnDied?.Invoke();
		}

		/// <summary>
		/// Ping the debounce. The will start time timer.
		/// </summary>
		public void Ping()
		{
			_isTriggered = false;
			_currentTime = 0;
		}

		public static void DoDebounce(string key, MonoBehaviour monoBehaviour, float debounceTime, Action callback)
		{
			if (!_debounceMap.ContainsKey(key))
				_debounceMap[key] = new Debounce(monoBehaviour, debounceTime, callback);
			
			_debounceMap[key]._callback = callback; // need to keep updating the callback
			_debounceMap[key].OnDied = () => _debounceMap.Remove(key);
			_debounceMap[key].Ping();
		}
	}
}