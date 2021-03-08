using UnityEngine;

namespace UnityTools.Extensions
{
	public enum Collision2DSideType
	{
		None,
		Left,
		Right,
		Top,
		Bottom,
	}
	
	/*
	 * Example use case:
			public class Character : MonoBehaviour {
			    private void OnCollisionEnter2D (Collision2D collision) {
			        Collision2DSideType collisionSide = collision.GetContactSide();

			        if (collisionSide == Collision2DSideType.Bottom) {
			            // Handle Character bottom collision.
			        }
			        else if (collisionSide == Collision2DSideType.Right) {
			            // Handle Character right collision.
			        }
			    }
			}
	 */

	/// <summary>
	/// Extension method to help find which size of box collider the collision was made
	/// <para></para>
	/// src: https://www.malgol.com/how-to-detect-collision-side-in-unity-2d/
	/// </summary>
	public static class Collision2DExtensions
	{
		private static Collision2DSideType GetContactSide(Vector2 max, Vector2 center, Vector2 contact)
		{
			Collision2DSideType side = Collision2DSideType.None;
			float diagonalAngle = Mathf.Atan2(max.y - center.y, max.x - center.x) * 180 / Mathf.PI;
			float contactAngle = Mathf.Atan2(contact.y - center.y, contact.x - center.x) * 180 / Mathf.PI;

			if (contactAngle < 0)
			{
				contactAngle = 360 + contactAngle;
			}

			if (diagonalAngle < 0)
			{
				diagonalAngle = 360 + diagonalAngle;
			}

			if (
				((contactAngle >= 360 - diagonalAngle) && (contactAngle <= 360)) ||
				((contactAngle <= diagonalAngle) && (contactAngle >= 0))
			)
			{
				side = Collision2DSideType.Right;
			}
			else if (
				((contactAngle >= 180 - diagonalAngle) && (contactAngle <= 180)) ||
				((contactAngle >= 180) && (contactAngle <= 180 + diagonalAngle))
			)
			{
				side = Collision2DSideType.Left;
			}
			else if (
				((contactAngle >= diagonalAngle) && (contactAngle <= 90)) ||
				((contactAngle >= 90) && (contactAngle <= 180 - diagonalAngle))
			)
			{
				side = Collision2DSideType.Top;
			}
			else if (
				((contactAngle >= 180 + diagonalAngle) && (contactAngle <= 270)) ||
				((contactAngle >= 270) && (contactAngle <= 360 - diagonalAngle))
			)
			{
				side = Collision2DSideType.Bottom;
			}

			return Opposite(side);
		}
		
		private static Collision2DSideType Opposite(Collision2DSideType sideType)
		{
			return sideType switch
			{
				Collision2DSideType.Left => Collision2DSideType.Right,
				Collision2DSideType.Right => Collision2DSideType.Left,
				Collision2DSideType.Top => Collision2DSideType.Bottom,
				Collision2DSideType.Bottom => Collision2DSideType.Top,
				_ => Collision2DSideType.None
			};
		}

		public static Collision2DSideType GetContactSide(this Collision2D collision)
		{
			Vector2 max = collision.collider.bounds.max;
			Vector2 center = collision.collider.bounds.center;
			Vector2 contact = collision.GetContact(0).point;
			return GetContactSide(max, center, contact);
		}
	}
}