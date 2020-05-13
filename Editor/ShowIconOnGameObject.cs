using UnityEditor;
using UnityEngine;

namespace UnityToolsEditor
{
	[InitializeOnLoad]
	public class ShowIconOnGameObject
	{
		static ShowIconOnGameObject()
		{
			EditorApplication.hierarchyWindowItemOnGUI += DrawIconOnWindowItem;
		}

		private static void DrawIconOnWindowItem(int instanceID, Rect rect)
		{
			var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			
			if (gameObject == null)
				return;

			if (!gameObject.TryGetComponent<MonoBehaviour>(out var script))
				return;
			
			if (!IsCustomScript(script))
				return;

			var r = new Rect(
				rect.xMax - 25,
				rect.yMin,
				rect.width,
				rect.height);

			GUI.Label(r, EditorGUIUtility.IconContent("d_cs Script Icon"));
		}

		private static bool IsCustomScript(MonoBehaviour script)
		{
			// conditions
			var nameSpace = script.GetType().Namespace;

			if (string.IsNullOrEmpty(nameSpace))
				return true;
			
			return !nameSpace.Contains("UnityEngine") && !nameSpace.Contains("TMPro");
		}
	}
}
