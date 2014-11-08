using UnityEngine;
using System.Collections;

public class ZoneStarter : MonoBehaviour {
	[SerializeField] ZoneManager manager;
	[SerializeField] Zone zone;

	void Start () {
		if (manager && zone) {
			manager.SetCurrent(zone);
		}
	}
}
