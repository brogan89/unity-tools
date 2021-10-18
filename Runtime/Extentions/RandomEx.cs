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
	}
}