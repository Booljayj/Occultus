using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(ObjectPool))]
public class ObjectPoolEditor : Editor {
	ReorderableList prefabList;
	SerializedProperty poolSize;

	void OnEnable() {
		prefabList = new ReorderableList(serializedObject, serializedObject.FindProperty("prefabs"));
		poolSize = serializedObject.FindProperty("poolSize");
	}

	public override void OnInspectorGUI () {
		serializedObject.Update();

		EditorGUILayout.IntSlider(poolSize, 1, 200);
		prefabList.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}
}
