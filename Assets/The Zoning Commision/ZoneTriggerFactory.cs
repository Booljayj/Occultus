using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneTriggerFactory : Factory {
	[SerializeField] List<ZoneTrigger> triggers;

	[SerializeField] Zone zone;
	[SerializeField] ZoneManager manager;

	void Awake() {
		zone = GetComponentInParent<Zone>();
		manager = GetComponentInParent<ZoneManager>();
	}

	public void Triggered() {
		if (zone && manager) {
			manager.SetCurrent(zone);
		}
	}

	protected override GameObject Create() {
		return new GameObject("Trigger", typeof(ZoneTrigger), typeof(BoxCollider));
	}
}
