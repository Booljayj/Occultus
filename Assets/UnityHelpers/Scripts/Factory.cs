using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Factory : MonoBehaviour {
	public void NewObject() {
		Create().transform.parent = transform;
	}

	protected virtual GameObject Create() {
		return new GameObject("New Object");
	}
}