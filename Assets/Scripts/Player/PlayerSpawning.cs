using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerSpawning : Resettable {

	[SerializeField] private LevelCheckpoints levelCheckpoints;
	[SerializeField] private SpriteRenderer renderer;
	[SerializeField] private CameraFollow camera = null;
	public UnityEvent playerSpawnEvent;
	public UnityEvent playerDeathEvent;
	
	private Rigidbody2D _rigid;
	private bool _isDead;
	
	void Start() {
		if (FindObjectOfType<LevelTime>() == null) {
			Debug.LogError("No LevelTime script found. Please add exactly 1 LevelTime prefab to the scene.");
			Application.Quit();
			// UnityEditor.EditorApplication.isPlaying = false;
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
		if (other.gameObject.CompareTag("Void")) {
			Die();
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Deadly")) {
			Die();
		}
	}

	public void Die() {
		if (!IsDead()) {
			_isDead = true;
			if (camera) {
				camera.PauseFollow();
			}
			LevelTime.Pause();
			renderer.color = Color.red;
			StartCoroutine(RestartFromCheckpoint(1));
			playerDeathEvent.Invoke();
		}
	}

	public bool IsDead() {
		return _isDead;
	}

	private void Revive() {
		_isDead = false;
		if (camera) {
			camera.UnpauseFollow();
		}
		renderer.color = Color.white;
		// _rigid.bodyType = RigidbodyType2D.Dynamic;
		_rigid.velocity = Vector2.zero;
		LevelTime.UnPause();
		playerSpawnEvent.Invoke();
	}

	private IEnumerator RestartFromCheckpoint(float delay) {
		yield return new WaitForSeconds(delay);
		levelCheckpoints.ResetToLastCheckpoint();
		Revive();
	}
	
	public override void SaveState() {
	}

	public override void ResetState() {
		transform.position = levelCheckpoints.GetCurrentSpawnPoint();
	}
}
