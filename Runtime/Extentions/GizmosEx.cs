using UnityEngine;

namespace UnityTools.Extensions
{
	public static class GizmosEx
	{
		/// <summary>
		/// Draws a circle around center point.
		/// Ensure you call from OnDrawGizmos()
		/// </summary>
		/// <param name="centerPosition"></param>
		/// <param name="radius"></param>
		/// <param name="colour"></param>
		public static void DrawCircleGizmo2D(Vector3 centerPosition, float radius, Color colour = default)
		{
			if (colour == default)
				colour = Color.white;

			// set gizmo colour
			Gizmos.color = colour;
			
			var theta = 0f;
			var x = radius * Mathf.Cos(theta);
			var y = radius * Mathf.Sin(theta);
		
			var pos = centerPosition + new Vector3(x, y, 0);
			var lastPos = pos;
		
			for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
			{
				x = radius * Mathf.Cos(theta);
				y = radius * Mathf.Sin(theta);
				var newPos = centerPosition + new Vector3(x, y, 0);
				Gizmos.DrawLine(pos, newPos);
				pos = newPos;
			}
		
			Gizmos.DrawLine(pos, lastPos);
			Gizmos.color = Color.white; // return to white
		}
	}
}