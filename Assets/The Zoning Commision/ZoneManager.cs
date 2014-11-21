using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ZoneManager : MonoBehaviour {
	public List<Zone> zones = new List<Zone>();
	
	[SerializeField] List<Zone> _activated = new List<Zone>();
	public List<Zone> activated {
		get {return _activated;}
	}

	[SerializeField] Zone _current;
	public Zone current {
		get {return _current;}
	}

	void Awake() {
		zones.Clear();
		GetComponentsInChildren<Zone>(true, zones);

		foreach (Zone z in zones) z.Deactivate();
		_activated.Clear();
	}

	public void SetCurrent(Zone zone, bool resetPosition = false) {
		DeactivateAll();
		
		_current = zone;

		if (resetPosition) {
			_current.transform.position = Vector3.zero;
			_current.transform.rotation = Quaternion.identity;
		}

		Activate(zone);
		ActivateMarkers(zone);
	}

	public void Activate(Zone z) {
		if (!_activated.Contains(z)) {
			z.Activate();
			_activated.Add(z);
		}
	}

	public void ActivateMarkers(Zone z, int index = -1) {
		if (index >= 0) {
			if (index < z.markers.Count) {
				ActivateMarker(z, z.markers[index]);
			} else {
				Debug.LogError(string.Format("Index out of range, index: {0}, zone: {1}", index, z.name));
			}
		} else {
			foreach (ZoneMarker m in z.markers) {
				ActivateMarker(z, m);
			}
		}
	}

	void ActivateMarker(Zone z, ZoneMarker m) {
		if (m.zone && m.zone != z) {
			Activate(m.zone);
			m.zone.transform.position = z.transform.TransformPoint(m.position);
			m.zone.transform.rotation = z.transform.rotation * Quaternion.Euler(m.rotation);
		} else {
			Debug.LogWarning(string.Format("Invalid ZoneMarker detected on {0}", z.name));
		}
	}

	public void Deactivate(Zone z) {
		if (_activated.Remove(z)) {
			z.Deactivate();
			if (z == _current) _current = null;
		}
	}

	public void DeactivateAll() {
		foreach (Zone z in _activated) {
			if (z) z.Deactivate();
		}
		_activated.Clear();
		_current = null;
	}

	public Zone FindByName(string name) {
		return zones.Find((Zone z) => {return z.name == name;});
	}
}