using UnityEngine;

namespace UnityTools.Extensions
{
	public static class TransformExtensions
	{
		public static void LookAt2D(this Transform transform, Vector2 targetPosition, float offsetDeg = 0, bool swapXAndY = false)
		{
			var dir = targetPosition - (Vector2) transform.position;
			var rad = swapXAndY ? Mathf.Atan2(dir.x, dir.y) : Mathf.Atan2(dir.y, dir.x);
			var deg = rad * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, deg + offsetDeg);
		}
	}
}