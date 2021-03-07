using UnityEngine;

namespace UnityTools.Extensions
{
	public static class TransformExtensions
	{
		private static Camera _mainCamera;
		private static Camera _MainCamera => _mainCamera ? _mainCamera : _mainCamera = Camera.main;
		
		/// <summary>
		/// Points a transform to a target with 2D context
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="targetPosition"></param>
		/// <param name="offsetDeg"></param>
		/// <param name="swapXAndY"></param>
		/// <param name="screenToWorld">Converts transform position from screen space to world space. Useful for pointing UI arrows at world targets</param>
		public static void LookAt2D(this Transform transform, Vector2 targetPosition, float offsetDeg = 0, bool swapXAndY = false, bool screenToWorld = false)
		{
			var pos = screenToWorld ? _MainCamera.ScreenToWorldPoint(transform.position) : transform.position;
			
			var dir = targetPosition - (Vector2)pos;
			var rad = swapXAndY ? Mathf.Atan2(dir.x, dir.y) : Mathf.Atan2(dir.y, dir.x);
			var deg = rad * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, deg + offsetDeg);
		}
	}
}