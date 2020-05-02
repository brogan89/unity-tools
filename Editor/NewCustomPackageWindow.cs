using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace UnityToolsEditor
{
	public class NewCustomPackageWindow : OdinEditorWindow
	{
		public static void ShowWindow(Action<PackageInfo> onCreate)
		{
			var window = GetWindow<NewCustomPackageWindow>("New Custom Package");
			window.onCreate = onCreate;
			window.Show();
		}

		[InlineProperty, HideLabel]
		public PackageInfo package;

		private Action<PackageInfo> onCreate;
		
		[Button]
		public void Create()
		{
			onCreate?.Invoke(package);
			Close();
		}

		[Button, PropertyOrder(-1)]
		private void Docs()
		{
			Application.OpenURL("https://docs.unity3d.com/Manual/upm-manifestPkg.html");
		}
		
		[Serializable]
		public struct PackageInfo
		{
			[Title("Required")]
			[Tooltip("The officially registered package name. This name must conform to the Unity Package Manager naming convention, which uses reverse domain name notation.")]
			public string name;
			[Tooltip("The package version number (MAJOR.MINOR.PATCH).")]
			public string version;
			
			[Title("Mandatory")]
			[Tooltip("A user-friendly name to appear in the Unity Editor")]
			public string displayName;
			[TextArea(1, 5), Tooltip("A brief description of the package.")]
			public string description;
			[Tooltip("Indicates the lowest Unity version the package is compatible with. If omitted, the package is considered compatible with all Unity versions.")]
			public string unity;

			[Title("Optional")]
			[Tooltip("Part of a Unity version indicating the specific release of Unity that the package is compatible with")]
			public string unityRelease;
			[Tooltip("	A map of package dependencies. Keys are package names, and values are specific versions. They indicate other packages that this package depends on.")]
			public List<DependencyInfo> dependencies;
			[Tooltip("An array of keywords used by the Package Manager search APIs. This helps users find relevant packages.")]
			public List<string> keywords;
			[Tooltip("A constant that provides additional information to the Package Manager")]
			public string type;
			[Tooltip("Author of the package.")]
			public AuthorInfo author;
			public RepoInfo repository;
			
			public override string ToString()
			{
				return JsonUtility.ToJson(this, true);
			}
		}
		
		[Serializable]
		public struct DependencyInfo
		{
			public string name;
			public string version;
		}
		
		[Serializable]
		public struct RepoInfo
		{
			public string type;
			public string url;
			
			public override string ToString()
			{
				return JsonUtility.ToJson(this, true);
			}
		}
		
		[Serializable]
		public struct AuthorInfo
		{
			public string name;
			public string email;
			public string url;
			
			public override string ToString()
			{
				return JsonUtility.ToJson(this, true);
			}
		}
	}
}