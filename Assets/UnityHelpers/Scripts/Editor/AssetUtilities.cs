using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class AssetUtilities {
	[MenuItem("Assets/Show Path")]
	public static void PathName() {
		if (Selection.activeObject && AssetDatabase.Contains(Selection.activeObject)) {
			EditorUtility.DisplayDialog(Selection.activeObject.name, AssetDatabase.GetAssetPath(Selection.activeObject), "Close");
		}
	}

	[MenuItem("Assets/Add As Subasset")]
	public static void AddAsSubasset() {
		if (Selection.activeObject && AssetDatabase.Contains(Selection.activeObject)) {
			string fullPath = EditorUtility.OpenFilePanel("Select Base Asset", "Assets", "asset");
			string basePath = fullPath.Replace(Application.dataPath, "Assets");
			Debug.Log(basePath);
			Object baseObj = AssetDatabase.LoadAssetAtPath(basePath, typeof(Object));

			if (baseObj == null) {
				EditorUtility.DisplayDialog("Invalid Base Object", "The selected base object is invalid", "Close");
				return;
			}

			if (EditorUtility.DisplayDialog(Selection.activeObject.name,
			                                "This object will be added to "+basePath+".\nThis can sometimes cause unwanted behavior. Continue?",
			                                "Continue",
			                                "Cancel")) {
				Object subObj = Selection.activeObject;
				Selection.activeObject = null;
				AssetDatabase.AddObjectToAsset(subObj, baseObj);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(baseObj));
				Selection.activeObject = baseObj;
			}
		}
	}

	public static string CurrentDirectory() {
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);

		if (path == "") {
			path = "Assets";
		} else if (Path.GetExtension(path) != "") {
			path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
		}

		return path;
	}
}
