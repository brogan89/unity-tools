using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityTools.Extensions;

namespace UnityTools
{
	/// <summary>
	/// A Debug extension which adds class name to start of log to make it easier to filter in the console.
	/// </summary>
	public static class DebugEx
	{
		public delegate void LogInfo(string className, int lineNumber, LogType type, object message);
		public static event LogInfo onError;

		public static void Log(object message,
			Color color = default,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			var logMessage = PrepareLogMessage(message.ToString(), color, sourceFilePath, memberName, sourceLineNumber);
			Debug.Log(logMessage);
#endif
		}

		public static void Log(object message,
			UnityEngine.Object context,
			Color color = default,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			var logMessage = PrepareLogMessage(message.ToString(), color, sourceFilePath, memberName, sourceLineNumber);
			Debug.Log(logMessage, context);
#endif
		}
		
		public static void LogWarning(object message,
			UnityEngine.Object context = null,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			var logMessage = PrepareLogMessage(message?.ToString(), default, sourceFilePath, memberName, sourceLineNumber);
			Debug.LogWarning(logMessage, context);
#endif
		}

		public static void LogError(object message,
			UnityEngine.Object context = null,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			var logMessage = PrepareLogMessage(message?.ToString(), default, sourceFilePath, memberName, sourceLineNumber);
			Debug.LogError(logMessage, context);

			onError?.Invoke(Path.GetFileName(sourceFilePath), sourceLineNumber, LogType.Error, message);
		}

		public static void LogError(Exception exception,
			UnityEngine.Object context = null,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			var logMessage = PrepareLogMessage(exception?.Message, default, sourceFilePath, memberName, sourceLineNumber);
			logMessage += $"\n-------------\n{exception}";

			Debug.LogError(logMessage, context);
			
			onError?.Invoke(Path.GetFileName(sourceFilePath), sourceLineNumber, LogType.Exception, exception);
		}

		private static string PrepareLogMessage(string message,
			Color color,
			string sourceFilePath,
			string memberName,
			int sourceLineNumber)
			=> color == default
				? $"<b>{Path.GetFileName(sourceFilePath)}:</b> {message}\n{memberName}:{sourceLineNumber}"
				: $"<b>{Path.GetFileName(sourceFilePath)}:</b> <color=#{color.ColorToHex()}>{message}</color>\n{memberName}:{sourceLineNumber}";
	}
}