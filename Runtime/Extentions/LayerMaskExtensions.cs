using UnityEngine;

namespace UnityTools.Extensions
{
	public static class LayerMaskExtensions
	{
		/// <summary>
		/// Returns true if gameObjects mask is within LayerMask values
		/// </summary>
		/// <param name="mask"></param>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static bool ContainsLayer(this LayerMask mask, GameObject gameObject)
		{
			return mask == (mask | (1 << gameObject.layer));
		}
	}
}