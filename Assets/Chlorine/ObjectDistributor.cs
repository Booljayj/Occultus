using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct ObjectPosition {
	public Vector3 position;
	public Vector3 rotation;
}

public class ObjectDistributor : MonoBehaviour {
	public ObjectPool pool;

	[SerializeField] List<ObjectPosition> positions = new List<ObjectPosition>();
	[SerializeField] List<GameObject> objects = new List<GameObject>();

	public void Distribute() {
		GameObject obj;
		foreach (ObjectPosition pos in positions) {
			obj = pool.Spawn();
			ConnectObject(obj, pos);
			objects.Add(obj);
		}
	}

	public void Collect() {
		foreach (GameObject obj in objects) {
			pool.Recycle(obj);
		}
		objects.Clear();
	}

	public void OnDrawGizmosSelected() {
		foreach (ObjectPosition pos in positions) {
			Gizmos.color = new Color(1.0f, .5f, 0f);
			Gizmos.DrawWireSphere(transform.TransformPoint(pos.position), .1f);
			Gizmos.DrawRay(transform.TransformPoint(pos.position), Quaternion.Euler(pos.rotation)*transform.forward);
		}
	}

	void ConnectObject(GameObject obj, ObjectPosition pos) {
		obj.transform.parent = transform;
		obj.transform.localPosition = pos.position;
		obj.transform.localRotation = Quaternion.Euler(pos.rotation);
	}
}
