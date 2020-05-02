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

		public static bool TryGetEditingPath(string packageName, out string path)
		{
			var packagePath = Manifest["dependencies"][packageName]?.ToString();
			
			path = !string.IsNullOrEmpty(packagePath) && packagePath.StartsWith("file:") 
				? packagePath.Replace("file:", "")
				: null;
			
			return !string.IsNullOrEmpty(path);
		}
		
		public static bool IsEditingPackage(string packageName)
		{
			return TryGetEditingPath(packageName, out _);
		}

		public static string GetSHA1(string packageName)
		{
			return IsEditingPackage(packageName) ? null : Manifest["lock"][packageName]["hash"].ToString();
		}

		public static void SetPackageValue(string packageName, string value)
		{
			Manifest["dependencies"][packageName] = value;
			WriteManifest();
		}

		private static void WriteManifest()
		{
			File.WriteAllText(PACKAGE_PATH, Manifest.ToString());
		}

		public static void SaveCustomPackageLocalPath(string packageName, string localPath)
		{
			if (!File.Exists(CustomPackagePath))
				_customPackages = new JObject();

			CustomPackages[packageName] = localPath;
			Debug.Log($"Saving local path. <b>{packageName}: {localPath}</b>");
			File.WriteAllText(CustomPackagePath, CustomPackages.ToString());
		}

		public static string GetSavedLocalPath(string packageName)
		{
			var path = !File.Exists(CustomPackagePath) && CustomPackages != null ? null : CustomPackages[packageName]?.ToString();
			
			Debug.Log(!string.IsNullOrEmpty(path)
				? $"Saving local path. <b>{packageName}: {path}</b>"
				: $"Local path not saved for package '{packageName}'");
		
			return path;
		}

		public static string GetCachedPackagePath(string packageName)
		{
			if (!IsEditingPackage(packageName))
				return $"Library/PackageCache/{packageName}@{GetSHA1(packageName).Substring(0, 10)}";
			
			Debug.LogError($"Package '{packageName}' is in edit mode. Use package local path");
			return null;
		}
	}
}