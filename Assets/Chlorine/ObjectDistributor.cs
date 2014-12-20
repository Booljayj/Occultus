using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectDistributor : MonoBehaviour {
	public ObjectPool pool;

	[SerializeField] List<GameObject> objects = new List<GameObject>();

	public void Distribute() {
		GameObject obj;
		foreach (Transform child in transform) {
			obj = pool.Spawn();

			obj.transform.parent = child;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;

			objects.Add(obj);
		}
	}

	public void Collect() {
		foreach (GameObject obj in objects) {
			pool.Recycle(obj);
		}
		objects.Clear();
	}
}
