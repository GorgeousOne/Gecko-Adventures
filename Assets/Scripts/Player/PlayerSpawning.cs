using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawning : Resettable {

	[SerializeField] private LevelCheckpoints levelCheckpoints;
	
	private Rigidbody2D _rigid;
	
	void Start() {
		if (FindObjectOfType<LevelTime>() == null) {
			Debug.LogError("No LevelTime script found. Please add exactly 1 LevelTime prefab to the scene.");
			Application.Quit();
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (levelCheckpoints == null) {
			Debug.LogWarning("No respawn points set for PlayerSpawning script. Please add a reference a LevelCheckpoints object.");
		}
		_rigid = GetComponent<Rigidbody2D>();
		_loadLastPlayerPos();
	}

	private void OnApplicationQuit() {
		PlayerPrefs.DeleteAll();
	}

	private void _loadLastPlayerPos() {
		Vector3 savedPosition = levelCheckpoints.GetCurrentSpawnPoint();
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
		if (!IsDead()) {
			_rigid.bodyType = RigidbodyType2D.Static;
			StartCoroutine(RestartFromCheckpoint(1));
		}
	}

	public bool IsDead() {
		return _rigid.bodyType != RigidbodyType2D.Dynamic;
	}

	private void Revive() {
		_rigid.bodyType = RigidbodyType2D.Dynamic;
		_rigid.velocity = Vector2.zero;
	}

	private IEnumerator RestartFromCheckpoint(float delay) {
		yield return new WaitForSeconds(delay);
		levelCheckpoints.ResetToLastCheckpoint();
		Revive();
	}
	
	// private IEnumerator RestartLevel(float delay) {
		// yield return new WaitForSeconds(delay);
		// SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	// }

	public override void SaveState() {
	}

	public override void ResetState() {
		transform.position = levelCheckpoints.GetCurrentSpawnPoint();
	}
}
