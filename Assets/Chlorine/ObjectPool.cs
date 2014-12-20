using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
	public List<GameObject> prefabs;

	Stack<GameObject> pool;
	[SerializeField] int poolSize = 1;
	
	void Start() {
		//create a pool with the maximum size given.
		pool = new Stack<GameObject>(poolSize);

		//add existing children to the pool
		if (transform.childCount > poolSize) {
			Debug.LogError("Object Pool has too many existing objects to add to the pool.\n" +
			               "Consider increasing the pool size or removing some objects", this);
		}

		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);
			pool.Push(child.gameObject);
		}
	}

	/// <summary>
	/// Spawn a GameObject at the specified position and rotation, parented to the parent. Pulls from the pool if an object is available.
	/// </summary>
	/// <param name="parent">Parent Transform</param>
	/// <param name="position">Local Position.</param>
	/// <param name="rotation">Local Rotation.</param>
	public GameObject Spawn() {
		if (pool != null && pool.Count > 0) {
			//there are objects in the pool already, so use the one on top, making sure to activate it.
			pool.Peek().SetActive(true);
			return pool.Pop();

		} else {
			//the pool is empty, so create a new object. Object will start active.
			if (prefabs.Count > 0) {
				//if there are prefabs to choose from in the prefab list, create a random one
				GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
				if (prefab) {
					return (GameObject)Instantiate(prefab);
				} else {
					//the chosen prefab was null, here's an escape.
					Debug.LogWarning("Object pool has a null prefab entry", this);
					return new GameObject("New Object");
				}

			} else {
				//if the prefab list is empty, create an empty object.
				return new GameObject("New Object");
			}
		}
	}

	/// <summary>
	/// Recyles the GameObject into the pool. If there is no space left in the pool, the object is destroyed normally.
	/// </summary>
	/// <param name="obj">The Object to Recycle</param>
	public void Recycle(GameObject obj) {
		if (pool == null || pool.Count >= poolSize) {
			//if the pool is full, we have no choice but to destroy the object.
			if (Application.isEditor) DestroyImmediate(obj);
			else Destroy(obj);

		} else {
			//set up the object for storage
			obj.transform.parent = transform;
			obj.SetActive(false);

			//there is space in the pool, so shove it in there.
			pool.Push(obj);
		}
	}

	/// <summary>
	/// Clears the pool, destroying all GameObjects.
	/// </summary>
	void Clear() {
		foreach (GameObject obj in pool) {
			DestroyImmediate(obj);
		}
		pool.Clear();
	}
}
