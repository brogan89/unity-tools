using System.Collections.Generic;

namespace UnityTools.Extensions
{
	public static class CollectionExtensions
	{
		public static bool IsIndexInRange<T>(this IReadOnlyCollection<T> enumerable, int index)
		{
			return index >= 0 && index < enumerable.Count;
		}

		/// <summary>
		/// Adds item to list only if not already contained in list
		/// </summary>
		/// <param name="list"></param>
		/// <param name="item"></param>
		/// <typeparam name="T"></typeparam>
		public static void AddUniq<T>(this List<T> list, T item)
		{
			if (!list.Contains(item))
				list.Add(item);
		}
	}
}