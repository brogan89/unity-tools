using UnityEngine;

namespace UnityTools.Extensions
{
	public static class TextureExtensions
	{
		public static Sprite CreateSprite(this Texture2D texture)
		{
			if (texture != null)
				return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);

			Debug.LogError("Texture2D can not be null");
			return null;
		}
	}
}