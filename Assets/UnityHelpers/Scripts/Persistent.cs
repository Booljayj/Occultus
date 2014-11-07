using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Persistent : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	void OnDestroy() {
		Destroy(gameObject);
	}
}