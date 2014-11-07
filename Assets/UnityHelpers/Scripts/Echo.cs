using UnityEngine;

[ExecuteInEditMode]
public class Echo : MonoBehaviour {
	#region Creation/Destruction
	void Reset() {
		Debug.Log(string.Format("[{0}] Reset()", name));
	}

	void Awake() {
		Debug.Log(string.Format("[{0}] Awake()", name));
	}

	void Start () {
		Debug.Log(string.Format("[{0}] Start()", name));
	}

	void OnDestroy() {
		Debug.Log(string.Format("[{0}] OnDestroy()", name));
	}
	#endregion

	#region Runtime
	void OnEnable() {
		Debug.Log(string.Format("[{0}] OnEnable()", name));
	}

	void Update () {
		Debug.Log(string.Format("[{0}] Update()", name));
	}

	void OnDisable() {
		Debug.Log(string.Format("[{0}] OnDisable()", name));
	}
	#endregion

	void OnLevelWasLoaded(int i) {
		Debug.Log(string.Format("[{0}] OnLevelWasLoaded()", name));
	}
}
