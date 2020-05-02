using System.IO;
using UnityEditor;
using UnityEngine;
using UnityTools;

namespace UnityToolsEditor
{
	[InitializeOnLoad]
	public class ShowIconOnGameObject
	{
		private static readonly Texture2D Icon;
		private const float WIDTH = 25;

		static ShowIconOnGameObject()
		{
			var bytes = Utils.TryGetEditingPath(Config.PACKAGE_NAME, out var editingPath)
				? File.ReadAllBytes($"{editingPath}/Resources/c.png")
				: File.ReadAllBytes($"Library/PackageCache/{Config.PACKAGE_NAME}@{Utils.GetSHA1(Config.PACKAGE_NAME).Substring(0, 10)}/Resources/c.png");
			
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
