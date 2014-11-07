using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(Factory), true)]
public class FactoryEditor : Editor {
	Factory factory;

	void OnEnable() {
		factory = target as Factory;
	}

	public override void OnInspectorGUI () {
		if (GUILayout.Button("Create New Object")) {
			factory.NewObject();
		}
	}
}
