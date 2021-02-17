using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTools.Extensions;

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
		/// <summary>
		/// Subscriber data
		/// </summary>
		private struct Subscriber
		{
			/// <summary>
			/// The script which the event has been subscribed
			/// </summary>
			public MonoBehaviour caller;
			
			/// <summary>
			/// The callback for the published event
			/// </summary>
			public Delegate callback;
		}

		private static readonly Dictionary<Type, List<Subscriber>> Subscribers = new Dictionary<Type, List<Subscriber>>();

		#region Subscribe

		/// <summary>
		/// Subscribe a callback to an event
		/// </summary>
		/// <param name="caller"></param>
		/// <param name="callback"></param>
		/// <typeparam name="T"></typeparam>
		public static void Subscribe<T>(MonoBehaviour caller, Action callback) where T : IMessage
		{
			AddSubscriber<T>(caller, callback);
		}

		/// <summary>
		/// Subscribe a callback to an event
		/// </summary>
		/// <param name="caller"></param>
		/// <param name="callback"></param>
		/// <typeparam name="T"></typeparam>
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
			var newSub = new Subscriber
			{
				caller = caller,
				callback = callback,
			};
			
			Subscribers[type].Add(newSub);
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

			var item = subsList.FirstOrDefault(x => x.caller == caller && x.callback == callback);
			
			if (item.caller)
			{
				// DebugEx.Log($"Removing sub from {type.Name} : {item.caller}", prefix: DebugEx.Icons.Phone);
				Subscribers[type].Remove(item);
			}
		}

		#endregion
		
		public static void Publish<T>() where T : IMessage
		{
			Publish(Activator.CreateInstance<T>());
		}

		public static void Publish<T>(T eventMessage) where T : IMessage
		{
			if (eventMessage == null)
			{
				DebugEx.LogError($"{nameof(eventMessage)} is null. Publish failed.", prefix: DebugEx.Icons.Phone);
				return;
			}
			
			var type = typeof(T);
			
			if (!Subscribers.ContainsKey(type))
				return;

			// filter subscribers - removers all null callers
			Subscribers[type].RemoveAll(x => !x.caller);
			
			var subs = Subscribers[type];
			
			DebugEx.Log($"Publishing event: {type.Name}. subs: {subs.Select(x => x.caller ? x.caller.name : "").ToArrayString()}", prefix: DebugEx.Icons.Phone);

			// invoke callbacks
			foreach (var sub in subs)
			{
				// skip if caller is disabled
				if (!sub.caller.enabled)
					continue;
				
				if(sub.callback.Method.GetParameters().Length > 0)
					sub.callback.DynamicInvoke(eventMessage);
				else
					sub.callback.DynamicInvoke();
			}
		}
	}
}