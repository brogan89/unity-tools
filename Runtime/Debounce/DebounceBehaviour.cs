using UnityEngine;
using UnityEngine.Events;

namespace UnityTools
{
	/// <summary>
	/// A serialised version of <see cref="Debounce"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class DebounceBehaviour<T> : MonoBehaviour
	{
		public float time = 0.2f;
		public UnityEvent<T> onInvoke;
		private Debounce _debounce;

		protected abstract T Value { get; }

		protected virtual void Awake()
		{
			_debounce = new Debounce(this, time, () => onInvoke?.Invoke(Value));
		}

		protected virtual void OnValidate()
		{
			_debounce?.SetTime(time);
		}

		protected void Ping()
		{
			_debounce?.Ping();
		}
	}
}