using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ZoneManager : MonoBehaviour {
	public List<Zone> zones = new List<Zone>();
	[SerializeField] List<Zone> activated = new List<Zone>();

	[SerializeField] Zone _current;
	public Zone current {
		get {return _current;}
	}

	void OnEnable() {
		zones.Clear();
		GetComponentsInChildren<Zone>(zones);
		foreach (Zone z in zones) z.Deactivate();
		activated.Clear();
	}

	public void SetCurrent(Zone zone, bool resetPosition = false) {
		//deactivate all zones to prepare for the new configuration
		foreach (Zone z in activated) {
			z.Deactivate();
		}
		activated.Clear();

		//set the new zone as current
		_current = zone;

		if (resetPosition) {
			//move the current zone to 0,0,0 with no rotation
			_current.transform.position = Vector3.zero;
			_current.transform.rotation = Quaternion.identity;
		}

		//activate the current zone
		_current.Activate();
		activated.Add(_current);
		//activate all markers on the current zone
		foreach (ZoneMarker m in _current.markers) {
			//attempt to activate the marker
			_current.ActivateMarker(m);
			#if UNITY_EDITOR
			if (m.zone) activated.Add(m.zone);
			#else
			activated.Add(m.zone);
			#endif
		}
	}

	public Zone FindByName(string name) {
		return zones.Find((Zone z) => {return z.name == name;});
	}
}