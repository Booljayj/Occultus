using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(Zone))]
public class ZoneEditor : Editor {
	Zone zone;

	ReorderableList markerlist;
	ZoneMarker selectedMarker;

	SerializedProperty OnActivate, OnDeactivate;

	void OnEnable() {
		zone = target as Zone;
		
		OnActivate = serializedObject.FindProperty("OnActivate");
		OnDeactivate = serializedObject.FindProperty("OnDeactivate");

		markerlist = new ReorderableList(serializedObject, serializedObject.FindProperty("_markers"));
		markerlist.elementHeight = EditorGUIUtility.singleLineHeight*3f;

		markerlist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			rect.height = EditorGUIUtility.singleLineHeight;
			Rect zoneRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
			Rect posRect = new Rect(rect.x+10f, rect.y + EditorGUIUtility.singleLineHeight, rect.width-10f, EditorGUIUtility.singleLineHeight);
			Rect rotRect = new Rect(rect.x+10f, rect.y + EditorGUIUtility.singleLineHeight*2f, rect.width-10f, EditorGUIUtility.singleLineHeight);

			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(zoneRect, markerlist.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("_zone"), GUIContent.none);
			EditorGUI.PropertyField(posRect, markerlist.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("position"), GUIContent.none);
			EditorGUI.PropertyField(rotRect, markerlist.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("rotation"), GUIContent.none);
			if (EditorGUI.EndChangeCheck() || Event.current.keyCode == KeyCode.Return) { //have to check for return to ensure proper behavior when manually entering values
				zone.ActivateMarker(zone.markers[index]);
			}
		};
		markerlist.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField(rect, "Markers", EditorStyles.miniLabel);
			if (GUI.Button(new Rect(rect.x + rect.width - 50f, rect.y, 50f, rect.height), "Refresh", EditorStyles.miniButton)) {
				foreach (ZoneMarker m in zone.markers) {
					zone.ActivateMarker(m);
				}
			}
		};
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		if (GUILayout.Button("Set as Current")) {
			zone.manager.SetCurrent(zone, true);
		}

		EditorGUILayout.PropertyField(OnActivate);
		EditorGUILayout.PropertyField(OnDeactivate);

		EditorGUILayout.Space();

		markerlist.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}

//	void OnSceneGUI() {
//		if (markerlist.index >= 0 && markerlist.index < markerlist.count) {
//
//		}
//	}
}