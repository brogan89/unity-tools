using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UnityToolsEditor
{
	public class OpenPersistentDataFolder : Editor
	{
		[MenuItem("Window/Open Persistent Data Folder")]
		private static void OpenFolder()
		{
			try
			{
				Debug.Log($"Opening persistent data. path: {Application.persistentDataPath}");
				Process.Start(Application.persistentDataPath);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}
	}	
}