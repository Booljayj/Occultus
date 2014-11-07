using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Zone : MonoBehaviour {
	ZoneManager _manager;
	public ZoneManager manager {
		get {
			if (_manager) {
				return _manager;
			} else {
				_manager = GetComponentInParent<ZoneManager>();
				if (!_manager) {
					Debug.LogWarning("Attempted to access manager on orphaned zone "+name);
				}
				return _manager;
			}
		}
	}

	[SerializeField] List<ZoneMarker> _markers = new List<ZoneMarker>();
	public List<ZoneMarker> markers {
		get {return _markers;}
	}

	public UnityEvent OnActivate = new UnityEvent();
	public UnityEvent OnDeactivate = new UnityEvent();

	#region Unity Events
	void OnDestroy() {
		if (_manager) {
			_manager.zones.Remove(this);
		}
	}
	#endregion

	#region Activation
	public void Activate() {
		gameObject.SetActive(true);
		OnActivate.Invoke();
	}

	public void ActivateMarker(ZoneMarker marker) {
		#if UNITY_EDITOR
		if (marker.zone == null) {
			Debug.LogWarning(name + " contains a marker with an empty reference.");
			return;
		}
		if (marker.zone == this) {
			Debug.LogWarning(name + " contains a marker which refers to itself.");
			return;
		}
		#endif
		marker.zone.Activate();

		marker.zone.transform.position = transform.TransformPoint(marker.position);
		marker.zone.transform.rotation = transform.rotation * Quaternion.Euler(marker.rotation);
	}

	public void Deactivate() {
		gameObject.SetActive(false);
		OnDeactivate.Invoke();
	}
	#endregion
}
