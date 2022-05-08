using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour {

	[SerializeField] private SpriteRenderer playerRenderer;
	private Rigidbody2D _rigid;
	
	void Start() {
		_rigid = GetComponent<Rigidbody2D>();
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
		StartCoroutine(RestartLevel(1));
	}

	private IEnumerator RestartLevel(float delay) {
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
