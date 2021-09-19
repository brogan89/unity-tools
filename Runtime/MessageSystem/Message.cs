using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using Newtonsoft.Json;
using UnityEngine;
using UnityTools.Extensions;
using Debug = UnityEngine.Debug;

namespace UnityTools.MessageSystem
{
	/// <summary>
	/// A simple implementation of Event Bus system
	/// </summary>
	public class Message : NetworkBehaviour
	{
		private static Message _instance;
		private static Message Instance =>
			_instance ? _instance : _instance = FindObjectOfType<Message>();

		/// <summary>
		/// Publishes event to all subscribers
		/// </summary>
		/// <param name="eventMessage"></param>
		/// <param name="hostOnly"></param>
		/// <typeparam name="T"></typeparam>
		public static void Publish<T>(T eventMessage, bool hostOnly = false) where T : IMessage
		{
			if (eventMessage == null)
				throw new NullReferenceException($"{nameof(eventMessage)} is null. Publish failed");

			if (eventMessage is MonoBehaviour)
				throw new ArgumentException($"IMessage should not be implemented by a {nameof(MonoBehaviour)}. Use custom classes only");

			var jsonString = JsonConvert.SerializeObject(eventMessage);
			var typePath = GetPathFromType(typeof(T));
			
			Debug.Log($"[MessageSystem] Publish. typePath: {typePath}, {jsonString}");
			
			if (NetworkManager.Singleton && NetworkManager.Singleton.IsListening)
				Instance.PublishServerRpc(jsonString, typePath, hostOnly);
			else
				MessageReceivedInternal(jsonString, typePath);
		}
		
		[ServerRpc(RequireOwnership = false)]
		private void PublishServerRpc(string jsonString, string typePath, bool hostOnly)
		{
			if (hostOnly)
				MessageReceivedInternal(jsonString, typePath);
			else
				MessageReceivedClientRpc(jsonString, typePath);
		}
		
		[ClientRpc]
		// ReSharper disable once MemberCanBeMadeStatic.Local
		private void MessageReceivedClientRpc(string jsonString, string typePath)
		{
			Debug.Log($"[MessageSystem] MessageReceivedClientRpc. {typePath} {jsonString}");
			MessageReceivedInternal(jsonString, typePath);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataStr"></param>
		/// <param name="typePath"></param>
		private static void MessageReceivedInternal(string dataStr, string typePath)
		{
			Debug.Log($"[MessageSystem] MessageReceivedInternal. {typePath} {dataStr}");

			var sw = new Stopwatch();
			sw.Start();
			
			if (!TryGetTypeFromString(typePath, out var type))
			{
				Debug.LogError("Error getting type");
				return;
			}
			
			Debug.Log($"[MessageSystem] Type. {type}");
			
			var eventMessage = JsonConvert.DeserializeObject(dataStr, type);
			
			Debug.Log($"[MessageSystem] eventMessage. {eventMessage}");
			
			// this will only get active gameObjects
			var monos = FindObjectsOfType<MonoBehaviour>();

			foreach (var mono in monos)
			{
				var methods = mono.GetType()
					.GetMethods()
					.Where(m => m.GetCustomAttributes(typeof(MessageCallbackAttribute), false).Length > 0)
					.ToList();
				
				if (mono.name.Contains("Room Control"))
				{
					Debug.LogError($"Room Control. {mono.GetType()}, methods: {mono.GetType().GetMethods().ToArrayString()}", mono);
				}
			
				foreach (var methodInfo in methods)
				{
					if (methodInfo.GetParameters().Length != 1)
						continue;
					
					if (methodInfo.GetParameters()[0].ParameterType != type)
						continue;
					
					Debug.Log($"[MessageSystem] methodInfo. {methodInfo}", mono);
					
					// if coroutine
					if (methodInfo.ReturnType == typeof(IEnumerator))
						mono.StartCoroutine(methodInfo.Name, eventMessage);
					else
						methodInfo.Invoke(mono, new[]{eventMessage});
				}
			}
			
			Debug.Log($"MessageReceivedInternal done. {sw.ElapsedMilliseconds}ms");
		}

		private static string GetPathFromType(Type type)
		{
			// needs to be in this format for Type.GetType()
			// see: https://stackoverflow.com/questions/3512319/resolve-type-from-class-name-in-a-different-assembly
			return $"{type.FullName}, {type.Assembly}";
		}

		private static bool TryGetTypeFromString(string path, out Type type)
		{
			type = Type.GetType(path);
			return type != null;
		}
	}
}