using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerSpawning : MonoBehaviour {

	[SerializeField] private LevelCheckpoints levelCheckpoints;
	[SerializeField] private SpriteRenderer renderer;
	[SerializeField] private CameraFollow camera = null;
	[SerializeField] private Animator bodyAnimator;
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
		LoadLastPlayerPos();
	}

	private void OnApplicationQuit() {
		PlayerPrefs.DeleteAll();
	}

	private void LoadLastPlayerPos() {
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
			// renderer.color = Color.red;
			bodyAnimator.SetBool("IsDead", _isDead);
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
		transform.parent = null;
		transform.position = levelCheckpoints.GetCurrentSpawnPoint();
		// renderer.color = Color.white;
		bodyAnimator.SetBool("IsDead", _isDead);
		_rigid.velocity = Vector2.zero;
		LevelTime.UnPause();
		playerSpawnEvent.Invoke();
	}

	private IEnumerator RestartFromCheckpoint(float delay) {
		yield return new WaitForSeconds(delay);
		levelCheckpoints.ResetToLastCheckpoint();
		Revive();
	}
}
