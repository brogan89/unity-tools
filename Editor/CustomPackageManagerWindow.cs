using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityTools;
using UnityTools.Extensions;
using Debug = UnityEngine.Debug;

namespace UnityToolsEditor
{
	public class CustomPackageManagerWindow : EditorWindow
	{
		private List<PackageInfo> packages;
		
		[MenuItem("Window/Custom Package Manager", priority = 103)]
		private static void Init()
		{
			var window = GetWindow<CustomPackageManagerWindow>("Custom Package Manager");
			window.RefreshPackages();
			window.Show();
		}

		private static List<PackageInfo> CreatePackageInfo()
		{
			var manifest = PackageManagerUtils.Manifest;
			
			return manifest["dependencies"]
				.Where(x => !((JProperty)x).Name.StartsWith("com.unity")) //TODO: will need to not rely on 'com.unity' in future
				.Select(x => new PackageInfo(((JProperty)x).Name, ((JProperty)x).Value.ToString()))
				.ToList();
		}

		private void RefreshPackages()
		{
			packages = CreatePackageInfo();
			Debug.Log($"Loaded packages: {packages.ToArrayString()}");
		}

		private void OnGUI()
		{
			GUILayout.Label("Custom Packages", EditorStyles.boldLabel);

			// create new
			if (GUILayout.Button("Create New"))
				CreateNewCustomPackage();
			
			// refresh
			if (GUILayout.Button("Refresh Packages"))
				RefreshPackages();

			// list all custom packages
			foreach (var pkg in packages)
			{
				// package name
				GUILayout.Label(pkg.isEditing ? $"{pkg.name} (EDITING)" : pkg.name, EditorStyles.boldLabel);
				GUILayout.Label(pkg.version);
				
				// -----------------------------------------------

				// git URL
				EditorGUILayout.BeginHorizontal();
				{
					pkg.url = EditorGUILayout.TextField("URL", pkg.url);
					if (GUILayout.Button("Open"))
						pkg.OpenGitUrl();
				}
				EditorGUILayout.EndHorizontal();
				
				// -----------------------------------------------
				
				// local path
				EditorGUILayout.BeginHorizontal();
				{
					pkg.localPath = EditorGUILayout.TextField("Local Path", pkg.localPath);
			
					if (GUILayout.Button("Open"))
						pkg.GoToLocalPath();
					
					if (GUILayout.Button("Save Path"))
						PackageManagerUtils.SaveCustomPackageLocalPath(pkg.name, pkg.localPath);
					
					EditorGUILayout.EndHorizontal();
					
					var isEditing = EditorGUILayout.Toggle("Edit", pkg.isEditing);
					if (isEditing != pkg.isEditing)
					{
						pkg.isEditing = isEditing;
						Debug.Log($"Toggle editing: {pkg.isEditing}");
						PackageManagerUtils.SetPackageValue(pkg.name, pkg.isEditing ? $"file:{pkg.localPath}" : pkg.url);
						RefreshPackages();
					}
				}
				
				// -----------------------------------------------
			}
		}
		
		private void CreateNewCustomPackage()
		{
			Debug.Log("Creating new custom packages is not implemented yet");
		}
		
		[Serializable]
		private class PackageInfo
		{
			public string name;
			public string version;
			public string localPath;
			public string url;
			public bool isEditing;

			public PackageInfo(string name, string path)
			{
				this.name = name;
				isEditing = path.StartsWith("file:");

				if (isEditing)
				{
					localPath = path.Replace("file:", "");
					
					// get git url from local package.json
					var packageJson = JObject.Parse(File.ReadAllText($"{localPath}/package.json"));
					url = packageJson.SelectToken("repository.url").ToString();
					version = packageJson["version"].ToString();
				}
				else
				{
					url = path;
					localPath = PackageManagerUtils.GetSavedLocalPath(name);

					var packageJson = JObject.Parse(File.ReadAllText($"{PackageManagerUtils.GetCachedPackagePath(name)}/package.json"));
					version = packageJson["version"].ToString();
				}
			}

			public override string ToString()
			{
				return JsonUtility.ToJson(this, true);
			}

			public void GoToLocalPath()
			{
				Debug.Log($"Open folder: {localPath}");
				Process.Start(localPath.Replace("file:", ""));
			}

			public void OpenGitUrl()
			{
				Debug.Log($"Open browser: {url}");
				Application.OpenURL(url);
			}
		}
	}
}