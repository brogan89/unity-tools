using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace UnityTools
{
	public static class Utils
	{
		private static JObject _manifest;
		private static JObject Manifest => _manifest ?? (_manifest = JObject.Parse(File.ReadAllText("Packages/manifest.json")));

		public static bool TryGetEditingPath(string packageName, out string path)
		{
			var packagePath = Manifest["dependencies"][packageName]?.ToString();
			
			path = !string.IsNullOrEmpty(packagePath) && packagePath.StartsWith("file:") 
				? packagePath.Replace("file:", "")
				: null;
			
			return !string.IsNullOrEmpty(path);
		}
		
		public static bool IsEditingPackage(string packageName)
		{
			return TryGetEditingPath(packageName, out _);
		}

		public static string GetSHA1(string packageName)
		{
			return IsEditingPackage(packageName) ? null : Manifest["lock"][packageName]["hash"].ToString();
		}

		#region Math

		public static float GetPercentage(float min, float max, float input)
		{
			return (input - min) / (max - min);
		}

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

		#endregion

		#region Gizmos
		
		/// <summary>
		/// Draws a circle around center point.
		/// Ensure you call from OnDrawGizmos()
		/// </summary>
		/// <param name="centerPosition"></param>
		/// <param name="radius"></param>
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

		#endregion
	}
}