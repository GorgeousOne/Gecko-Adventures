using UnityEngine;
using Random = UnityEngine.Random;

public class FireflySpawner : MonoBehaviour, IResettable {
	
	[SerializeField] private GameObject prefab;
	private Vector3 _savedPos;

	private void Start() {
		foreach(Transform child in transform) {
			Destroy(child.gameObject);
		}
		Spawn();
	}

	private void Spawn() {
		if (prefab != null) {
			GameObject spawned = Instantiate(prefab, transform);
			spawned.GetComponent<Animator>().SetFloat("Offset", Random.Range(0f, 1f));
		}
		else {
			Debug.LogWarning("Spawner has no prefab assigned!", this);
		}
	}

	public void SaveState() { }

	public void ResetState() {
		if (transform.childCount == 0) {
			Debug.Log("my children!");
			Spawn();
		}
	}
}