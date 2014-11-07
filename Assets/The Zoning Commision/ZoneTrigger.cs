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
		if (list == null) {
			return;
		} else {
			if (other.tag == "Player") {
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