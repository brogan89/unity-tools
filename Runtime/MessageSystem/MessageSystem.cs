using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.MessageSystem
{
	/// <summary>
	/// All messages need to implement this interface to be registered
	/// </summary>
	public interface IMessage { }

	/// <summary>
	/// Base subscriber interface
	/// </summary>
	public interface ISubscriber { }
	
	/// <summary>
	/// Subscriber interface which provides the callback method
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISubscriber<in T> : ISubscriber where T : IMessage
	{
		void OnPublished(T message);
	}

	/// <summary>
	/// A simple implementation of Event Bus system
	/// </summary>
	public static class EventMessage
	{
		/// <summary>
		/// List of subscribers
		/// </summary>
		private static readonly List<ISubscriber> _Subs = new List<ISubscriber>();

		/// <summary>
		/// Subscribe to ISubscriber callbacks
		/// </summary>
		/// <param name="sub"></param>
		public static void Sub(ISubscriber sub)
		{
			if (!_Subs.Contains(sub))
				_Subs.Add(sub);
		}
		
		/// <summary>
		/// Subscribe to ISubscriber callbacks
		/// </summary>
		/// <param name="sub"></param>
		public static void Unsub(ISubscriber sub)
		{
			_Subs.Remove(sub);
		}

		/// <summary>
		/// Bind subscriber to MonoBehaviour.
		/// This will add a script to the GameObject which in turn will sub/unsub in its OnEnable/OnDisable methods
		/// </summary>
		/// <param name="sub"></param>
		public static void Bind(MonoBehaviour sub)
		{
			sub.gameObject.AddComponent<SubscriberBinder>().Bind(sub as ISubscriber);
		}

		/// <summary>
		/// Publishes event to all subscribers
		/// </summary>
		/// <param name="eventMessage"></param>
		/// <typeparam name="T"></typeparam>
		public static void Publish<T>(T eventMessage) where T : IMessage
		{
			if (eventMessage == null)
				throw new NullReferenceException($"{nameof(eventMessage)} is null. Publish failed");

			// make a copy as subs may be removed from the OnPublished callback
			foreach (var sub in _Subs.ToArray()) 
				if (sub is ISubscriber<T> s)
					s.OnPublished(eventMessage);
		}
	}
}