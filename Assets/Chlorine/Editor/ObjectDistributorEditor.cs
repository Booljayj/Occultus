using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(ObjectDistributor))]
public class ObjectDistributorEditor : Editor {
	ObjectDistributor distributor;
	ReorderableList positionList;

	SerializedProperty pool;
	SerializedProperty objects;
	SerializedProperty position;
	SerializedProperty rotation;

	void OnEnable() {
		distributor = target as ObjectDistributor;

		positionList = new ReorderableList(serializedObject, serializedObject.FindProperty("positions"), true, false, true, true);
		pool = serializedObject.FindProperty("pool");
		objects = serializedObject.FindProperty("objects");

		float height = EditorGUIUtility.singleLineHeight;

		positionList.elementHeight = height * 2f;
		positionList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			SerializedProperty element = positionList.serializedProperty.GetArrayElementAtIndex(index);
			if (element == null) return;

			position = element.FindPropertyRelative("position");
			rotation = element.FindPropertyRelative("rotation");

			Rect positionRect = new Rect(rect.x, rect.y, rect.width, height);
			Rect rotationRect = new Rect(rect.x, rect.y + height, rect.width, height);

			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(positionRect, position, GUIContent.none);
			EditorGUI.PropertyField(rotationRect, rotation, GUIContent.none);

			if (EditorGUI.EndChangeCheck()) {
				if (index < objects.arraySize) {
					SerializedProperty obj = objects.GetArrayElementAtIndex(index);
					if (obj != null && obj.serializedObject != null) {
						GameObject gobj = obj.objectReferenceValue as GameObject;
						gobj.transform.localPosition = position.vector3Value;
						gobj.transform.localRotation = Quaternion.Euler(rotation.vector3Value);
					}
				}
			}
		};
	}

	public override void OnInspectorGUI () {
		serializedObject.Update();

		EditorGUILayout.PropertyField(pool);

		EditorGUI.BeginDisabledGroup(pool.objectReferenceValue == null);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Distribute")) {
			distributor.Distribute();
		}
		if (GUILayout.Button("Collect")) {
			distributor.Collect();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUI.EndDisabledGroup();

		positionList.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}
}
