using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class ZoneTrigger : MonoBehaviour {
	ZoneTriggerFactory list;

	void Awake() {
		list = GetComponentInParent<ZoneTriggerFactory>();

		GetComponent<BoxCollider>().isTrigger = true;
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("Collision");
		if (list == null) {
			Debug.Log("Could not activate zone");
		} else {
			if (other.tag == "Player") {
				Debug.Log("Activating Zone");
				list.Triggered();
			}
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.red;

		BoxCollider box = GetComponent<BoxCollider>();
		Gizmos.DrawWireCube(box.center, box.size);

		Gizmos.matrix = Matrix4x4.identity;
	}
}