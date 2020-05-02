using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityTools;
using UnityTools.Extensions;
using Debug = UnityEngine.Debug;

namespace UnityToolsEditor
{
	public class CustomPackageManagerWindow : OdinEditorWindow
	{
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
		
		[Button, PropertyOrder(0)]
		private void CreateNewCustomPackage()
		{
			NewCustomPackageWindow.ShowWindow(info =>
			{
				Debug.Log($"Created new package: {info}");
			});
		}

		[Button, PropertyOrder(1)]
		private void RefreshPackages()
		{
			packages = CreatePackageInfo();
			Debug.Log($"Loaded packages: {packages.ToArrayString()}");
		}
		
		[SerializeField, PropertyOrder(2)] 
		private List<PackageInfo> packages;
		
		[Serializable]
		private class PackageInfo
		{
			public string name;
			public string version;
			
			[InlineButton(nameof(GoToLocalPath), "Open")]
			[InlineButton(nameof(SaveLocalPath), "Save")]
			public string localPath;
			
			[InlineButton(nameof(OpenGitUrl), "Open")]
			public string url;
			
			[DisableIf("@string.IsNullOrEmpty(localPath)")]
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

					try
					{
						var cachedPath = PackageManagerUtils.GetCachedPackagePath(name);
						var jsonText = File.ReadAllText($"{cachedPath}/package.json");
						var packageJson = JObject.Parse(jsonText);
						version = packageJson["version"].ToString();
					}
					catch(Exception e)
					{
						Debug.LogError(e);
					}
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

			public void SaveLocalPath()
			{
				PackageManagerUtils.SaveCustomPackageLocalPath(name, localPath);
			}

			public void OpenGitUrl()
			{
				Debug.Log($"Open browser: {url}");
				Application.OpenURL(url);
			}
		}
	}
}