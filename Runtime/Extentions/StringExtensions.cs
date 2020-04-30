using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTools.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Prints contents as [0, 1, 2, 3] ... but on each new line for readability
		/// </summary>
		/// <param name="enumerable"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static string ToArrayString<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
				enumerable = Enumerable.Empty<T>();

			var values = enumerable as T[] ?? enumerable.ToArray();
			return values.Length == 0
				? "[]"
				: $"[\n{string.Join(",\n", values.Select(x => $"    {x}"))}\n]";
		}

		/// <summary>
		/// Converts Color32 to a hex string
		/// </summary>
		/// <param name="color"></param>
		/// <param name="startHash">Adds the # at start of string</param>
		/// <returns></returns>
		public static string ColorToHex(this Color32 color, bool startHash = false)
		{
			return $"{(startHash ? "#" : "")}{color.r:X2}{color.g:X2}{color.b:X2}";
		}

		/// <summary>
		/// Converts Color to a hex string
		/// </summary>
		/// <param name="color"></param>
		/// <param name="startHash">Adds the # at start of string</param>
		/// <returns></returns>
		public static string ColorToHex(this Color color, bool startHash = false)
		{
			return ColorToHex((Color32) color, startHash);
		}

		/// <summary>
		/// returns [frame: {Time.frameCount}]
		/// </summary>
		/// <returns></returns>
		public static string Frame()
		{
			return $"[frame: {Time.frameCount}]";
		}

		/// <summary>
		/// Parses string to int but also supplies a fallback number
		/// </summary>
		/// <param name="intString"></param>
		/// <param name="fallbackNumber"></param>
		/// <returns></returns>
		public static int ParseToInt(this string intString, int fallbackNumber = -1)
		{
			try
			{
				return int.Parse(intString);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				Debug.LogError($"string value: {intString}");
				return fallbackNumber;
			}
		}
	}
}