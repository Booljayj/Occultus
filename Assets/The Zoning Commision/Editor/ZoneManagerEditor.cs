using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Reflection;

[CustomEditor(typeof(ZoneManager))]
public class ZoneManagerEditor : Editor {
	ZoneManager manager;
	ReorderableList zones;

	void OnEnable() {
		manager = target as ZoneManager;

		zones = new ReorderableList(serializedObject, serializedObject.FindProperty("zones"));

		zones.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			Zone element = zones.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue as Zone;
			if (element == null) return;

			//positioning
			Rect nameRect = new Rect(rect.x, rect.y, rect.width - 40, EditorGUIUtility.singleLineHeight);
			Rect currentRect = new Rect(rect.x + rect.width - 40, rect.y, 20, EditorGUIUtility.singleLineHeight);
			Rect visibilityRect = new Rect(rect.x + rect.width - 20, rect.y, 20, EditorGUIUtility.singleLineHeight);

			//zone name
			EditorGUI.LabelField(nameRect, element.name);

			//current button
			if (manager.current == element) {
				if (GUI.Button(currentRect, EditorGUIUtility.IconContent("PlayButton On", "Refresh Current"), EditorStyles.whiteLargeLabel)) {
					manager.SetCurrent(element, true);
				}
			} else {
				if (GUI.Button(currentRect, EditorGUIUtility.IconContent("PlayButton", "Set As Current"), EditorStyles.whiteLargeLabel)) {
					manager.SetCurrent(element, true);
				}
			}
			
			//visibility button
			if (element.gameObject.activeSelf) {
				if (GUI.Button(visibilityRect, EditorGUIUtility.IconContent("ViewToolOrbit On", "Hide Zone"), EditorStyles.whiteLargeLabel)) {
					manager.Deactivate(element);
				}
			} else {
				if (GUI.Button(visibilityRect, EditorGUIUtility.IconContent("ViewToolOrbit", "Show Zone"), EditorStyles.whiteLargeLabel)) {
					manager.Activate(element);
				}
			}
		};

		zones.onAddCallback = (ReorderableList l) => {
			ReorderableList.defaultBehaviours.DoAddButton(l);

			GameObject newZone = CreateObject("New Zone", manager.gameObject, false, typeof(Zone));
			CreateObject("Triggers", newZone, true, typeof(ZoneTriggerFactory));
			CreateObject("Navigation", newZone, true);
			CreateObject("Static", newZone, true);
			CreateObject("Dynamic", newZone, true);

			Undo.RegisterCreatedObjectUndo(newZone, "Create New Zone");

			l.serializedProperty.GetArrayElementAtIndex(l.count-1).objectReferenceValue = newZone.GetComponent<Zone>() as Object;
		};

		zones.onRemoveCallback = (ReorderableList l) => {
			Zone element = l.serializedProperty.GetArrayElementAtIndex(l.index).objectReferenceValue as Zone;
			if (element) {
				DestroyImmediate(element.gameObject);
				ReorderableList.defaultBehaviours.DoRemoveButton(l);
			}

			ReorderableList.defaultBehaviours.DoRemoveButton(l);
		};

		zones.onSelectCallback = (ReorderableList l) => {
			Zone element = l.serializedProperty.GetArrayElementAtIndex(l.index).objectReferenceValue as Zone;
			EditorGUIUtility.PingObject(element);
		};
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		zones.DoLayoutList();

//		Hashtable contents = typeof(EditorGUIUtility).GetField("s_IconGUIContents", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Hashtable;
//		foreach(DictionaryEntry pair in contents) {
//			EditorGUILayout.BeginHorizontal();
//			EditorGUILayout.LabelField(pair.Key as string);
//			EditorGUILayout.LabelField(pair.Value as GUIContent);
//			EditorGUILayout.EndHorizontal();
//		}

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Show All")) {
			foreach (Zone z in manager.zones) {
				manager.Activate(z);
			}
		}
		if (GUILayout.Button("Hide All")) {
			manager.DeactivateAll();
		}
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	GameObject CreateObject(string name, GameObject parent, bool isStatic, params System.Type[] components) {
		GameObject go = new GameObject(name, components);
		go.transform.parent = parent.transform;
		go.isStatic = isStatic;
		return go;
	}
}
