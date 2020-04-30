using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityToolsEditor
{
	[InitializeOnLoad]
	public class ShowIconOnGameObject
	{
		private static readonly Texture2D Icon;
		private const float WIDTH = 15;

		static ShowIconOnGameObject()
		{
			var bytes = File.ReadAllBytes(@"C:\Users\brogan\Documents\GitHub\unity-tools\Resources\c.png");
			Icon = new Texture2D(1, 1);
			Icon.LoadImage(bytes);
			Icon.Apply();
			
			EditorApplication.hierarchyWindowItemOnGUI += DrawIconOnWindowItem;
		}

		private static void DrawIconOnWindowItem(int instanceID, Rect rect)
		{
			if (Icon == null)
				return;

			GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (gameObject == null)
				return;

			var script = gameObject.GetComponent<MonoBehaviour>(); // can change this to any type
			if (!script)
				return;

			var r = new Rect(
				rect.xMax - WIDTH,
				rect.yMin,
				rect.width,
				rect.height);

			GUI.color = Color.cyan;
			GUI.Label(r, Icon);
			GUI.color = Color.white;
		}
	}
}