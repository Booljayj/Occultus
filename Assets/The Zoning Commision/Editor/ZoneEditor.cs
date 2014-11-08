using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(Zone))]
public class ZoneEditor : Editor {
	Zone zone;

	ReorderableList markerlist;

	SerializedProperty OnActivate, OnDeactivate;

	void OnEnable() {
		zone = target as Zone;
		
		OnActivate = serializedObject.FindProperty("OnActivate");
		OnDeactivate = serializedObject.FindProperty("OnDeactivate");

		markerlist = new ReorderableList(serializedObject, serializedObject.FindProperty("_markers"));
		markerlist.elementHeight = EditorGUIUtility.singleLineHeight*3f;

		markerlist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			SerializedProperty element = markerlist.serializedProperty.GetArrayElementAtIndex(index);
			rect.height = EditorGUIUtility.singleLineHeight;
			Rect zoneRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
			Rect pullRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, 10f, EditorGUIUtility.singleLineHeight);
			Rect syncRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight*2f, 10f, EditorGUIUtility.singleLineHeight);
			Rect posRect = new Rect(rect.x+10f, rect.y + EditorGUIUtility.singleLineHeight, rect.width-10f, EditorGUIUtility.singleLineHeight);
			Rect rotRect = new Rect(rect.x+10f, rect.y + EditorGUIUtility.singleLineHeight*2f, rect.width-10f, EditorGUIUtility.singleLineHeight);

			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(zoneRect, element.FindPropertyRelative("_zone"), GUIContent.none);
			EditorGUI.PropertyField(posRect, element.FindPropertyRelative("position"), GUIContent.none);
			EditorGUI.PropertyField(rotRect, element.FindPropertyRelative("rotation"), GUIContent.none);
			if (EditorGUI.EndChangeCheck() || Event.current.keyCode == KeyCode.Return) { //have to check for return to ensure proper behavior when manually entering values
				zone.manager.ActivateMarkers(zone, index);
			}

			if (GUI.Button(pullRect, "P")) {
				Zone z = element.FindPropertyRelative("_zone").objectReferenceValue as Zone;
				element.FindPropertyRelative("position").vector3Value = zone.transform.InverseTransformPoint(z.transform.position);
				element.FindPropertyRelative("rotation").vector3Value = (z.transform.rotation * Quaternion.Inverse(zone.transform.rotation)).eulerAngles;
			}
			if (GUI.Button(syncRect, "S")) {
				//WIP
//				Zone z = element.FindPropertyRelative("_zone").objectReferenceValue as Zone;
//				ZoneMarker m = z.markers.Find((ZoneMarker zm)=>{return zm.zone == zone;});
//				if (m == null) {
//					m = new ZoneMarker();
//					z.markers.Add(m);
//				}
//				m.position = 
			}
		};
		markerlist.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField(rect, "Markers", EditorStyles.miniLabel);
			if (GUI.Button(new Rect(rect.x + rect.width - 50f, rect.y, 50f, rect.height), "Refresh", EditorStyles.miniButton)) {
				zone.manager.ActivateMarkers(zone);
			}
		};
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Set as Current")) {
			zone.manager.SetCurrent(zone, true);
		}
		if (GUILayout.Button("Isolate")) {
			zone.manager.DeactivateAll();
			zone.manager.Activate(zone);
		}
		EditorGUILayout.EndHorizontal();

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