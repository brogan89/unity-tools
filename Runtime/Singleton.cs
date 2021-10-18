using UnityEngine;

namespace UnityTools
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance { get; private set; }
		
		[Header("Singleton")]
		[SerializeField] private bool dontDestroyOnLoad;

		protected virtual void Awake()
		{
			if (!Instance)
			{
				Instance = GetComponent<T>();

				if (!dontDestroyOnLoad)
					return;

				if (transform.parent == null)
					DontDestroyOnLoad(gameObject);
				else
					Debug.LogError($"DontDestroyOnLoad can only be applied to root gameObjects. {name}");
			}
			else if (Instance != this)
				Destroy(gameObject);
		}
	}
}