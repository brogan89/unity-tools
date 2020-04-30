using UnityEngine;

namespace UnityTools.Extensions
{
	public static class GameObjectExtensions
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponent<T>();
			if (!component)
				component = gameObject.AddComponent<T>();
			return component;
		}
		
		public static bool IsPlayer(this GameObject gameObject)
		{
			return gameObject.CompareTag("Player");
		}

		public static bool IsEnemy(this GameObject gameObject)
		{
			return gameObject.CompareTag("Enemy");
		}
	}
}