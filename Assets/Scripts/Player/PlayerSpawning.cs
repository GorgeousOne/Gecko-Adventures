using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawning : MonoBehaviour {

	[SerializeField] private SpriteRenderer playerRenderer;
	[SerializeField] private LevelCheckpoints levelCheckpoints;
	
	private Rigidbody2D _rigid;
	
	void Start() {
		_rigid = GetComponent<Rigidbody2D>();
		_loadLastPlayerPos();
	}

	private void OnApplicationQuit() {
		PlayerPrefs.DeleteAll();
	}

	private void _loadLastPlayerPos() {
		Vector3 savedPosition = transform.position;
		string sceneName = SceneManager.GetActiveScene().name;

		if (PlayerPrefs.HasKey("Player_X_" + sceneName)) {
			savedPosition.x = PlayerPrefs.GetFloat("Player_X_" + sceneName);
			savedPosition.y = PlayerPrefs.GetFloat("Player_Y_" + sceneName);
			savedPosition.z = PlayerPrefs.GetFloat("Player_Z_" + sceneName);
		}
		transform.position = savedPosition;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Deadly")) {
			Die();
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Deadly")) {
			Die();
		}
	}

	private void Die() {
		playerRenderer.color = Color.red;
		_rigid.bodyType = RigidbodyType2D.Static;
		StartCoroutine(RestartFromCheckpoint(1));
	}

	private void Revive() {
		playerRenderer.color = Color.white;
		_rigid.bodyType = RigidbodyType2D.Dynamic;
	}

	private IEnumerator RestartFromCheckpoint(float delay) {
		yield return new WaitForSeconds(delay);
		transform.position = levelCheckpoints.GetCurrentSpawnPoint();
		Revive();
	}
	
	private IEnumerator RestartLevel(float delay) {
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
