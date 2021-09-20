using UnityEngine;

namespace UnityTools.MessageSystem
{
	public struct Vec2
	{
		public float X { get; set; }
		public float Y { get; set; }

		public static implicit operator Vector2(Vec2 v) => new Vector2(v.X, v.Y);
		public static implicit operator Vec2(Vector2 v) => new Vec2{ X= v.x, Y = v.y };
		public static implicit operator Vec2(Vector3 v) => new Vec2{ X= v.x, Y = v.y };
		
		// public static explicit operator Vec2(Vector2 v) => new Vec2{ X= v.x, Y = v.y };
		// public static explicit operator Vec2(Vector3 v) => new Vec2{ X= v.x, Y = v.y };
	}
}