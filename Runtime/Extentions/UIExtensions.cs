using UnityEngine;
using UnityEngine.UI;

namespace UnityTools.Extensions
{
	public static class UIExtensions
	{
		public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
		{
			var c = spriteRenderer.color;
			c.a = alpha;
			spriteRenderer.color = c;
		}

		public static void SetAlpha(this Graphic graphic, float alpha)
		{
			var c = graphic.color;
			c.a = alpha;
			graphic.color = c;
		}
	}
}