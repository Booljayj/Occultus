using UnityEngine;
using System.Collections;

[System.Serializable]
public class ZoneMarker {
	[SerializeField] Zone _zone;
	public Zone zone {
		get {return _zone;}
	}

	public Vector3 position;
	public Vector3 rotation;

	public ZoneMarker(Zone z) {
		_zone = z;
	}
}