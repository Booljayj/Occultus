using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MonoScript))]
public class ScriptInspector : Editor {
	MonoScript ms;
	System.Type type;

	bool isScriptableObject;

	void OnEnable() {
		ms = target as MonoScript;
		type = ms.GetClass();

		isScriptableObject = 
			(type != null &&
			type.IsSubclassOf(typeof(ScriptableObject)) &&
			!type.IsSubclassOf(typeof(Editor)));
	}

	public override void OnInspectorGUI () {
		if (isScriptableObject) {
			if (GUILayout.Button("Create Instance")) {
				ScriptableObject asset = ScriptableObject.CreateInstance(type);
				string path = AssetDatabase.GenerateUniqueAssetPath("Assets/"+type.Name+".asset");
				AssetDatabase.CreateAsset(asset, path);
				AssetDatabase.Refresh();
				EditorGUIUtility.PingObject(asset);
			}
		}

		GUI.enabled = false;
		EditorGUILayout.TextArea(ms.text);
	}
}
