using System;

namespace UnityTools
{
	public class Debounce
	{
		private readonly float _debounceTime;
		private readonly Action _callback;

		private float _currentTime;
		private bool _isTriggered;

		public Debounce(float debounceTime, Action callback)
		{
			_debounceTime = debounceTime;
			_callback = callback;
		}
		
		public void Tick(float dt)
		{
			if (_isTriggered)
				return;
			
			_currentTime += dt;

			if (_currentTime > _debounceTime)
			{
				_callback?.Invoke();
				_isTriggered = true;
			}
		}

		public void Reset()
		{
			_isTriggered = false;
			_currentTime = 0;
		}
	}
}