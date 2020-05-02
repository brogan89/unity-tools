using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace UnityTools
{
	public static class PackageManagerUtils
	{
		private const string PACKAGE_PATH = "Packages/manifest.json";
		private static string CustomPackagePath => $"{Application.persistentDataPath}/custom-packages.json";
		
		private static JObject _manifest;
		public static JObject Manifest => _manifest ?? (_manifest = JObject.Parse(File.ReadAllText(PACKAGE_PATH)));
		
		private static JObject _customPackages;
		private static JObject CustomPackages => _customPackages ?? (_customPackages = JObject.Parse(File.ReadAllText(CustomPackagePath)));

		/// <summary>
		/// Returns the local directory editing path specified in manifest.json
		/// </summary>
		/// <param name="packageName"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool TryGetEditingPath(string packageName, out string path)
		{
			var packagePath = Manifest["dependencies"][packageName]?.ToString();
			
			path = !string.IsNullOrEmpty(packagePath) && packagePath.StartsWith("file:") 
				? packagePath.Replace("file:", "")
				: null;
			
			return !string.IsNullOrEmpty(path);
		}

		/// <summary>
		/// Ret
		/// </summary>
		/// <param name="packageName"></param>
		/// <returns></returns>
		public static bool IsEditingPackage(string packageName)
		{
			return TryGetEditingPath(packageName, out _);
		}

		/// <summary>
		/// Returns SHA1 of package.
		/// NOTE: only works outside of edit mode
		/// </summary>
		/// <param name="packageName"></param>
		/// <returns></returns>
		public static string GetSHA1(string packageName)
		{
			return IsEditingPackage(packageName) ? null : Manifest?["lock"]?[packageName]?["hash"]?.ToString();
		}

		/// <summary>
		/// Sets the value of the packageName in manifest.json
		/// This can be either 'file:{localPath}' or '{gitUrl}'
		/// </summary>
		/// <param name="packageName"></param>
		/// <param name="value"></param>
		public static void SetPackageValue(string packageName, string value)
		{
			Manifest["dependencies"][packageName] = value;
			WriteManifest();
		}

		/// <summary>
		/// Saves current Manifest data to manifest.json file in Packages/
		/// </summary>
		private static void WriteManifest()
		{
			File.WriteAllText(PACKAGE_PATH, Manifest.ToString());
		}

		/// <summary>
		/// Saves local file directory to custom-packages.json in persistent data folder
		/// </summary>
		/// <param name="packageName"></param>
		/// <param name="localPath"></param>
		public static void SaveCustomPackageLocalPath(string packageName, string localPath)
		{
			if (!File.Exists(CustomPackagePath))
				_customPackages = new JObject();

			CustomPackages[packageName] = localPath;
			Debug.Log($"Saving local path. <b>{packageName}: {localPath}</b>");
			File.WriteAllText(CustomPackagePath, CustomPackages.ToString());
		}

		/// <summary>
		/// Returns the saved local path specified in CustomPackageManagerWindow
		/// </summary>
		/// <param name="packageName"></param>
		/// <returns></returns>
		public static string GetSavedLocalPath(string packageName)
		{
			var path = !File.Exists(CustomPackagePath) || CustomPackages == null 
				? null 
				: CustomPackages[packageName]?.ToString();
			
			Debug.Log(!string.IsNullOrEmpty(path)
				? $"Loaded local path. <b>{packageName}: {path}</b>"
				: $"Local path not found for package '{packageName}'");
		
			return path;
		}

		/// <summary>
		/// Returns path to cached package in Library folder.
		/// NOTE: only used outside of editing directory
		/// </summary>
		/// <param name="packageName"></param>
		/// <returns></returns>
		public static string GetCachedPackagePath(string packageName)
		{
			if (!IsEditingPackage(packageName))
			{
				var sha = GetSHA1(packageName)?.Substring(0, 10);
				return !string.IsNullOrEmpty(sha)
					? $"Library/PackageCache/{packageName}@{sha}"
					: null;
			}
			
			Debug.LogError($"Package '{packageName}' is in edit mode. Use package local path");
			return null;
		}
	}
}