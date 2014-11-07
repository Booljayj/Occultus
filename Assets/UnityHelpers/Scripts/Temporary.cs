using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Temporary : MonoBehaviour {
	[SerializeField] List<GameObject> temp = new List<GameObject>();
	
	void OnEnable () {
		//this.hideFlags = HideFlags.HideInInspector;
	}

	void OnDestroy () {
		foreach (GameObject go in temp) {
			DestroyImmediate(go);
		}
		temp.Clear();
	}

	public void Add(GameObject obj) {
		if (temp.Contains(obj)) {
			Debug.Log(string.Format("[Temporary] '{0}' has already been added"));
		} else {
			temp.Add(obj);
			obj.hideFlags = HideFlags.DontSave;
		}
	}
	
	public void Remove(GameObject obj) {
		if (temp.Contains(obj)) {
			temp.Remove(obj);
			obj.hideFlags = HideFlags.None;
		} else {
			Debug.Log(string.Format("[Temporary] '{0}' was not added"));
		}
	}
}
