using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer)),
 ExecuteInEditMode]
public class Connector : MonoBehaviour {
	public enum ConnectionType {North, East, South, West, Polar};
	[System.Serializable]
	public struct ConnectionPoint {
		public RectTransform trans;
		public ConnectionType type;
		[Range(-1, 1)] public float pos;
		[Range(0, 10)] public float weight;
		public Color color;
	}

	public ConnectionPoint start, end;

	LineRenderer line;
	Vector3 p1, c1, p2, c2;
	bool sleep;

	static int vertices = 24;

	void Awake() {
		line = GetComponent<LineRenderer>();
	}

	void Update() {
		if (start.trans && end.trans) {
			if (start.trans.hasChanged || end.trans.hasChanged) {
				UpdateCurve();
			}
		}
	}

	void OnValidate() {
		if (!line) return;

		if (start.trans && end.trans) {
			line.enabled = true;
			UpdateCurve();
		} else {
			line.enabled = false;
		}
	}

	void UpdateCurve() {
		bool startActive = start.trans.gameObject.activeInHierarchy;
		bool endActive = end.trans.gameObject.activeInHierarchy;

		if (!startActive && !endActive) {
			line.enabled = false;
		} else {
			line.enabled = true;
			if (startActive && !endActive) {
				line.SetColors(start.color, Color.clear);
			} else if (!startActive && endActive) {
				line.SetColors(Color.clear, end.color);
			} else {
				line.SetColors(start.color, end.color);
			}
		}

		CalculatePoints(start, ref p1, ref c1);
		CalculatePoints(end, ref p2, ref c2);

		line.SetVertexCount(vertices);
		for (int i = 0; i < vertices; i++) {
			line.SetPosition(i, GetBezierPoint((float)i/(float)(vertices-1)));
		}

		foreach (ConnectorIcon icon in GetComponentsInChildren<ConnectorIcon>(true)) {
			icon.UpdatePosition();
		}

		transform.position = GetBezierPoint(.5f);
	}

	public Vector3 GetBezierPoint(float t, int derivative = 0) {
		derivative = Mathf.Clamp(derivative, 0, 2);
		float u = (1f-t);
		if (derivative == 0) {
			return u*u*u*p1 + 3f*u*u*t*c1 + 3f*u*t*t*c2 + t*t*t*p2;
		} else if (derivative == 1) {
			return 3f*u*u*(c1-p1) + 6f*u*t*(c2-c1) + 3f*t*t*(p2-c2);
		} else if (derivative == 2) {
			return 6f*u*(c2-2f*c1+p1) + 6f*t*(p2-2f*c2+c1);
		} else {
			return Vector3.zero;
		}
	}

	void CalculatePoints(ConnectionPoint conn, ref Vector3 p, ref Vector3 c) {
		if (conn.type == ConnectionType.North) {
			p = conn.trans.TransformPoint(conn.trans.rect.width/2f * conn.pos,
			                          conn.trans.rect.height/2f,
			                          0);
			c = p + conn.trans.up*conn.weight;

		} else if (conn.type == ConnectionType.South) {
			p = conn.trans.TransformPoint(conn.trans.sizeDelta.x/2f * conn.pos,
			                          -conn.trans.sizeDelta.y/2f,
			                          0);
			c = p - conn.trans.up*conn.weight;

		} else if (conn.type == ConnectionType.East) {
			p = conn.trans.TransformPoint(conn.trans.sizeDelta.x/2f,
			                          conn.trans.sizeDelta.y/2f * conn.pos,
			                          0);
			c = p + conn.trans.right*conn.weight;

		} else if (conn.type == ConnectionType.West) {
			p = conn.trans.TransformPoint(-conn.trans.sizeDelta.x/2f,
			                          conn.trans.sizeDelta.y/2f * conn.pos,
			                          0);
			c = p - conn.trans.right*conn.weight;

		} else if (conn.type == ConnectionType.Polar) {
			float angle = Mathf.PI/2f - conn.pos*Mathf.PI;
			p = conn.trans.TransformPoint(conn.trans.sizeDelta.x/2f * Mathf.Cos(angle),
			                              conn.trans.sizeDelta.y/2f * Mathf.Sin(angle),
			                              0);
			c = p + conn.trans.TransformDirection(Mathf.Cos(angle), Mathf.Sin(angle), 0)*conn.weight;
		}
	}
}
