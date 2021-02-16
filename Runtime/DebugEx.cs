using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityTools.Extensions;

namespace UnityTools
{
	/// <summary>
	/// A Debug extension which adds class name to start of log to make it easier to filter in the console.
	/// </summary>
	public static class DebugEx
	{
		public delegate void LogDelegate(string className, int lineNumber, LogType type, object message);
		public static event LogDelegate onError;

		// Technically this could still be not storing the main thread, but as long as 
		// DebugEx is called in any MonoBehaviour Start() or Awake() then we'll be fine.
		private static readonly Thread _unityThread;
		
		static DebugEx()
		{
			_unityThread = Thread.CurrentThread;
		}

		public static void Log(object message,
			Object context = null,
			Color? color = null,
			string prefix = "",
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			var logMessage = PrepareLogMessage(prefix, message, color, sourceFilePath, memberName, sourceLineNumber);
			Debug.Log(logMessage, context);
#endif
		}
		
		public static void LogWarning(object message,
			Object context = null,
			string prefix = "",
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			var logMessage = PrepareLogMessage(prefix, message, null, sourceFilePath, memberName, sourceLineNumber);
			Debug.LogWarning(logMessage, context);
#endif
		}

		public static void LogError(object message,
			Object context = null,
			string prefix = "",
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			var logMessage = PrepareLogMessage(prefix, message, null, sourceFilePath, memberName, sourceLineNumber);
			Debug.LogError(logMessage, context);
			onError?.Invoke(Path.GetFileName(sourceFilePath), sourceLineNumber, LogType.Error, message);
		}

		private static string PrepareLogMessage(string prefix, object message, Color? colour, string sourceFilePath,
			string memberName, int sourceLineNumber)
		{
			var frameCount = Thread.CurrentThread == _unityThread ? Time.frameCount.ToString() : "Not on main thread";
			
			return colour == null
				? $"{prefix} <b>{Path.GetFileName(sourceFilePath)}:</b> {message}\n{memberName}:{sourceLineNumber}\nFrame: {frameCount}".Trim()
				: $"{prefix} <b>{Path.GetFileName(sourceFilePath)}:</b> <color=#{((Color)colour).ColorToHex()}>{message}</color>\n{memberName}:{sourceLineNumber}\nFrame: {frameCount}".Trim();
		}

		/// <summary>
		/// Icons you can use in the console.
		/// </summary>
		public struct Icons
		{
			/*
			 * Got the idea from a twitter thread. https://twitter.com/Sleepless_Dev/status/1359217604037074945
			 * Thanks @Sleepless_Dev <3
			 *
			 * Get more icons from: https://www.w3schools.com/charsets/ref_utf_symbols.asp
			 */
			public const string Sun = "<color=yellow>☀</color>";
			public const string ArrowRight = "<color=yellow>→</color>";
			public const string Diamond = "<color=yellow>♦</color>";
			public const string Audio = "<color=yellow>♫</color>";
			public const string Square = "<color=yellow>▣</color>";
			public const string Tilde = "<color=yellow>∾</color>";
			public const string Skull = "<color=red>☠</color>";
			public const string Phone = "<color=cyan>☎</color>";
		}
	}
}