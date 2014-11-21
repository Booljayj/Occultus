using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(Zone))]
public class ZoneEditor : Editor {
	Zone zone;
	Zone other;

	ReorderableList markerlist;

	SerializedProperty OnActivate, OnDeactivate;

	bool liveMarkerEditing;

	void OnEnable() {
		zone = target as Zone;
		
		OnActivate = serializedObject.FindProperty("OnActivate");
		OnDeactivate = serializedObject.FindProperty("OnDeactivate");

		markerlist = new ReorderableList(serializedObject, serializedObject.FindProperty("_markers"));
		markerlist.elementHeight = EditorGUIUtility.singleLineHeight*3f;

		markerlist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			rect.height = EditorGUIUtility.singleLineHeight;
			Rect zoneRect = new Rect(rect.x, rect.y, rect.width, rect.height);
			Rect posRect = new Rect(rect.x+10f, rect.y + rect.height, rect.width-10f, rect.height);
			Rect rotRect = new Rect(rect.x+10f, rect.y + rect.height*2f, rect.width-10f, rect.height);

			SerializedProperty element = markerlist.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty _zone = element.FindPropertyRelative("_zone");
			SerializedProperty position = element.FindPropertyRelative("position");
			SerializedProperty rotation = element.FindPropertyRelative("rotation");

			EditorGUI.BeginDisabledGroup(liveMarkerEditing);
			EditorGUI.PropertyField(zoneRect, element.FindPropertyRelative("_zone"), GUIContent.none);
			EditorGUI.EndDisabledGroup();

			if (_zone.objectReferenceValue) {
				if (liveMarkerEditing) {
					other = _zone.objectReferenceValue as Zone;
					position.vector3Value = other.transform.position;
					rotation.vector3Value = other.transform.eulerAngles;
				}

				if (liveMarkerEditing) EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(posRect, element.FindPropertyRelative("position"), GUIContent.none);
				EditorGUI.PropertyField(rotRect, element.FindPropertyRelative("rotation"), GUIContent.none);
				if (liveMarkerEditing && EditorGUI.EndChangeCheck()) {
					other.transform.position = position.vector3Value;
					other.transform.rotation = Quaternion.Euler(rotation.vector3Value);
				}
			}
		};

		markerlist.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField(rect, "Markers", EditorStyles.miniLabel);
//			if (GUI.Button(new Rect(rect.x + rect.width - 50f, rect.y, 50f, rect.height), "Refresh", EditorStyles.miniButton)) {
//				zone.manager.ActivateMarkers(zone);
//			}
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

		EditorGUI.BeginChangeCheck();
		liveMarkerEditing = EditorGUILayout.ToggleLeft("Live Editing", liveMarkerEditing);
		if (EditorGUI.EndChangeCheck()) {
			if (liveMarkerEditing) {
				markerlist.displayAdd = false;
				markerlist.displayRemove = false;
				markerlist.draggable = false;
				zone.manager.SetCurrent(zone, true);
			} else {
				markerlist.displayAdd = true;
				markerlist.displayRemove = true;
				markerlist.draggable = true;
			}
		}
		markerlist.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}

	void OnSceneGUI() {
		Repaint();
	}

	void AddActiveZones(Zone self) {
		ZoneManager man = self.manager;

		foreach (Zone other in man.activated) {
			if (other != self) {
				ZoneMarker otherMarker = self.markers.Find((ZoneMarker zm)=>{return zm.zone == other;});
				if (otherMarker == null) {
					otherMarker = new ZoneMarker(other);
					self.markers.Add(otherMarker);
				}
				otherMarker.position = self.transform.InverseTransformPoint(other.transform.position);
				otherMarker.rotation = (other.transform.rotation * Quaternion.Inverse(self.transform.rotation)).eulerAngles;

				ZoneMarker selfMarker = other.markers.Find((ZoneMarker zm)=>{return zm.zone == self;});
				if (selfMarker == null) {
					selfMarker = new ZoneMarker(self);
					other.markers.Add(selfMarker);
				}
				selfMarker.position = -otherMarker.position;
				selfMarker.rotation = -otherMarker.rotation;

				//be sure to set as dirty!
				EditorUtility.SetDirty(other);
			}
		}
		//be sure to set as dirty!
		EditorUtility.SetDirty(self);
	}

	void SynchronizeZone(Zone z) {
		foreach (ZoneMarker m in z.markers) {

		}
	}
}