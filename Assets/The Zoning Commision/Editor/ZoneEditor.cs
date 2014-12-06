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

	bool editing;

	void OnEnable() {
		zone = target as Zone;
		
		OnActivate = serializedObject.FindProperty("OnActivate");
		OnDeactivate = serializedObject.FindProperty("OnDeactivate");

		markerlist = new ReorderableList(serializedObject, serializedObject.FindProperty("_markers"), false, true, false, false);
		markerlist.elementHeight = EditorGUIUtility.singleLineHeight*3f;

		markerlist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			float single = EditorGUIUtility.singleLineHeight;
			Rect zoneRect = new Rect(rect.x, rect.y, rect.width - 40f, single);
			Rect syncRect = new Rect(rect.x + rect.width - 40f, rect.y, 40f, single);
			Rect posRect = new Rect(rect.x+10f, rect.y + single, rect.width-10f, single);
			Rect rotRect = new Rect(rect.x+10f, rect.y + single*2f, rect.width-10f, single);

			SerializedProperty element = markerlist.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty _zone = element.FindPropertyRelative("_zone");
			SerializedProperty position = element.FindPropertyRelative("position");
			SerializedProperty rotation = element.FindPropertyRelative("rotation");

			EditorGUI.BeginDisabledGroup(!editing);
			if (_zone.objectReferenceValue) {
				EditorGUI.PropertyField(zoneRect, _zone, GUIContent.none);
				other = _zone.objectReferenceValue as Zone;

				if (GUI.Button(syncRect, "Sync", EditorStyles.miniButton)) {
					SyncMarker(other);
				}

				if (editing && other.transform.hasChanged) {
					position.vector3Value = other.transform.position;
					rotation.vector3Value = other.transform.eulerAngles;
				}

				if (editing) EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(posRect, element.FindPropertyRelative("position"), GUIContent.none);
				EditorGUI.PropertyField(rotRect, element.FindPropertyRelative("rotation"), GUIContent.none);
				if (editing && EditorGUI.EndChangeCheck()) {
					other.transform.position = position.vector3Value;
					other.transform.rotation = Quaternion.Euler(rotation.vector3Value);
					other.transform.hasChanged = false;
				}
			} else {
				EditorGUI.HelpBox(rect, "Zone Deleted", MessageType.Error);
			}
			EditorGUI.EndDisabledGroup();
		};

		markerlist.drawHeaderCallback = (Rect rect) => {
			Rect buttonRect = new Rect(rect.x, rect.y, 40f, rect.height);
			Rect labelRect = new Rect(rect.x + rect.width - 40f, rect.y, 40f, rect.height);

			EditorGUI.BeginChangeCheck();
			if (editing) {
				if (GUI.Button(buttonRect, "Done", EditorStyles.miniButton)) {
					EndEditing();
				}
			} else {
				if (GUI.Button(buttonRect, "Edit", EditorStyles.miniButton)) {
					StartEditing();
				}
			}

			EditorGUI.LabelField(labelRect, "Markers", EditorStyles.miniLabel);
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

	void OnSceneGUI() {
		Repaint();
	}

	void StartEditing() {
		editing = true;
		markerlist.draggable = true;
		markerlist.displayAdd = true;
		markerlist.displayRemove = true;
		zone.manager.SetCurrent(zone, true);
	}

	void EndEditing() {
		editing = false;
		markerlist.draggable = false;
		markerlist.displayAdd = false;
		markerlist.displayRemove = false;
	}

	void SyncMarker(Zone other) {
		ZoneMarker otherMarker = other.markers.Find(m => (m.zone == zone));
		ZoneMarker selfMarker = zone.markers.Find(m => (m.zone == other));

		if (otherMarker == null) {
			otherMarker = new ZoneMarker(zone);
			other.markers.Add(otherMarker);
		}

		otherMarker.position = -selfMarker.position;
		otherMarker.rotation = -selfMarker.rotation;

		EditorUtility.SetDirty(selfMarker.zone);
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
}