using UnityEngine;
using System.Collections;

public class UIExpand : MonoBehaviour {
	public Vector2 minSize;
	public Vector2 maxSize;

	public bool expand;

	void OnValidate() {
		RectTransform rect = GetComponent<RectTransform>();
		if (expand) {
			rect.sizeDelta = maxSize;
		} else {
			rect.sizeDelta = minSize;
		}
	}
}
