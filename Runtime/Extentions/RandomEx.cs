using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Extensions
{
	public static class RandomEx
	{
		public static Vector3 RandomVector3(Vector3 min, Vector3 max)
		{
			return new Vector3
			{
				x = Random.Range(min.x, max.x),
				y = Random.Range(min.y, max.y),
				z = Random.Range(min.z, max.z)
			};
		}

		public static Vector2 RandomVector2(Vector2 min, Vector2 max)
		{
			return new Vector3
			{
				x = Random.Range(min.x, max.x),
				y = Random.Range(min.y, max.y)
			};
		}

		public static T GetRandom<T>(this IList<T> collection)
		{
			var randi = Random.Range(0, collection.Count);
			return collection[randi];
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			var rng = new System.Random();
			var n = list.Count;
			while (n > 1)
			{
				n--;
				var k = rng.Next(n + 1);
				var value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
		
		public static IList<T> ShuffleCopy<T>(this IEnumerable<T> list)
		{
			var newList = new List<T>();
			newList.AddRange(list);
			newList.Shuffle();
			return newList;
		}
	}
}