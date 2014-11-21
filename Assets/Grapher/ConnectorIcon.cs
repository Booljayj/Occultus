using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas), typeof(Image))]
public class ConnectorIcon : MonoBehaviour {
	[Range(0,1)] public float percent;
	public bool tangent;

	Connector connector;
	
	void Awake() {
		connector = GetComponentInParent<Connector>();
	}

	void OnValidate() {
		UpdatePosition();
	}

	public void UpdatePosition() {
		if (connector == null) connector = GetComponentInParent<Connector>();
		if (connector) {
			transform.position = connector.GetBezierPoint(percent);
			if (tangent) transform.rotation = Quaternion.LookRotation(connector.GetBezierPoint(percent, 1));
		}
	}
}
