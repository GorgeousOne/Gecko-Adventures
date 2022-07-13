using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerSpawning : MonoBehaviour {

	[SerializeField] private LevelCheckpoints levelCheckpoints;
	[SerializeField] private CameraFollow mainCamera = null;
	[SerializeField] private Animator bodyAnimator;
	public UnityEvent playerSpawnEvent;
	public UnityEvent playerDeathEvent;
	
	private Rigidbody2D _rigid;
	private bool _isDead;

	private AudioSource[] _listOfPlayerAudios;
	private AudioSource _deathAudio;
	
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

		_listOfPlayerAudios = GetComponents<AudioSource>();
		_deathAudio = _listOfPlayerAudios[3];
		_deathAudio.enabled = false;
	}

	private void OnApplicationQuit() {
		PlayerPrefs.DeleteAll();
	}

	private void LoadLastPlayerPos() {
		Vector3 savedPosition = levelCheckpoints.GetCurrentSpawnPoint();
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
			if (mainCamera) {
				mainCamera.PauseFollow();
			}
			LevelTime.Pause();
			// bodyRenderer.color = Color.red;
			bodyAnimator.SetBool("IsDead", _isDead);
			StartCoroutine(RestartFromCheckpoint(1));
			playerDeathEvent.Invoke();
		}
	}

	public bool IsDead() {
		// enable/disable death sound
		_deathAudio.enabled = _isDead;
		return _isDead;
	}

	private void Revive() {
		_isDead = false;
		if (mainCamera) {
			mainCamera.UnpauseFollow();
		}
		transform.parent = null;
		transform.position = levelCheckpoints.GetCurrentSpawnPoint();
		// bodyRenderer.color = Color.white;
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
