using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class GrapherUtilities : MonoBehaviour {
	[MenuItem("Graph/Create Node")]
	static void CreateNode() {
		GameObject nodeObj = new GameObject("New Node", typeof(Canvas), typeof(Image));
		GameObject textObj = new GameObject("Text", typeof(Text));
		textObj.transform.parent = nodeObj.transform;

		RectTransform nodeRect = nodeObj.GetComponent<RectTransform>();
		RectTransform textRect = textObj.GetComponent<RectTransform>();

		nodeRect.pivot = new Vector2(.5f, .5f);
		nodeRect.sizeDelta = new Vector2(400f, 200f);
		nodeRect.localScale = new Vector3(.005f, .005f, .005f);

		textRect.anchorMax = Vector2.one;
		textRect.anchorMin = Vector2.zero;
		textRect.pivot = new Vector2(.5f, .5f);
		textRect.position = Vector3.zero;
		textRect.sizeDelta = Vector2.zero;

		Text text = textObj.GetComponent<Text>();
		text.text = "New Node";
		text.color = Color.black;
		text.fontSize = 48;
		text.alignment = TextAnchor.MiddleCenter;
	}

	[MenuItem("Graph/Create Connection")]
	static void CreateConnection() {
		GameObject connObj = new GameObject("Connection", typeof(Connector));
		LineRenderer line = connObj.GetComponent<LineRenderer>();
		line.SetWidth(.01f, .01f);

		Connector conn = connObj.GetComponent<Connector>();
		conn.start.color = Color.white;
		conn.start.weight = 1f;
		conn.end.color = Color.cyan;
		conn.end.weight = 1f;
	}
}