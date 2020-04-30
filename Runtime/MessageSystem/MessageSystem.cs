using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTools.MessageSystem
{
	/// <summary>
	/// All messages need to implement this interface to be registered
	/// </summary>
	public interface IMessage { }

	/// <summary>
	/// A simple implementation of Event Bus system
	/// </summary>
	public static class EventMessage
	{
		private struct Subscriber
		{
			public MonoBehaviour caller;
			public Delegate callback;
		}

		private static readonly Dictionary<Type, List<Subscriber>> Subscribers = new Dictionary<Type, List<Subscriber>>();

		#region Subscribe

		public static void Subscribe<T>(MonoBehaviour caller, Action callback) where T : IMessage
		{
			AddSubscriber<T>(caller, callback);
		}

		public static void Subscribe<T>(MonoBehaviour caller, Action<T> callback) where T : IMessage
		{
			AddSubscriber<T>(caller, callback);
		}

		private static void AddSubscriber<T>(MonoBehaviour caller, Delegate callback) where T : IMessage
		{
			var type = typeof(T);
			if (!Subscribers.ContainsKey(type))
				Subscribers[type] = new List<Subscriber>();

			// skip if double up
			var subs = Subscribers[type];
			foreach (var sub in subs)
			{
				if (sub.caller != caller || sub.callback != callback)
					continue;

				Debug.LogWarning($"Event has already been subscribed ({typeof(T)}). caller: {caller}. callback: {callback}");
				return;
			}

			// add new subscriber
			Subscribers[type].Add(new Subscriber{ caller = caller, callback = callback});
		}

		#endregion


		#region Unsubscribe

		public static void Unsubscribe<T>(MonoBehaviour caller, Action callback) where T : IMessage
		{
			RemoveSubscriber<T>(caller, callback);
		}

		public static void Unsubscribe<T>(MonoBehaviour caller, Action<T> callback) where T : IMessage
		{
			RemoveSubscriber<T>(caller, callback);
		}

		private static void RemoveSubscriber<T>(MonoBehaviour caller, Delegate callback) where T : IMessage
		{
			var type = typeof(T);
			if (!Subscribers.TryGetValue(type, out var subsList))
				return;

			var item = subsList.FirstOrDefault(x => ReferenceEquals(x.caller, caller) && ReferenceEquals(x.callback, callback));
			if (item.caller)
				Subscribers[type].Remove(item);
		}

		#endregion
		public static void Publish<T>() where T : IMessage
		{
			Publish(Activator.CreateInstance<T>());
		}

		public static void Publish<T>(T eventMessage) where T : IMessage
		{
			var type = typeof(T);
			if (!Subscribers.ContainsKey(type))
				return;

			// filter subscribers - removers all null callers
			Subscribers[type].RemoveAll(x => !x.caller);
			var subs = Subscribers[type];

			// invoke callbacks
			foreach (var sub in subs)
			{
				if(sub.callback.Method.GetParameters().Length > 0)
					sub.callback.DynamicInvoke(eventMessage);
				else
					sub.callback.DynamicInvoke();
			}
		}
	}

	public class ExampleMessage : IMessage
	{
		public object SomeData { get; set; }
	}
}
