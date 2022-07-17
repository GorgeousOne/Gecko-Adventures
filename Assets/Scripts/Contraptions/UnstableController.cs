using UnityEngine;
using UnityEngine.Events;

public class UnstableController : MonoBehaviour, Resettable {

	public UnityEvent BreakEvent;
	
	private bool _isBroken;
	private bool _wasBroken;

	private Collider2D _collider2D;
	private SpriteRenderer _renderer;
	
	private void Awake() {
		_collider2D = GetComponent<Collider2D>();
		_renderer = GetComponent<SpriteRenderer>();
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Player")) {
			Break();
		}
	}

	public void Break() {
		GetComponentInChildren<ParticleSystem>().Play();
		_isBroken = true;
		_collider2D.enabled = false;
		_renderer.enabled = false;
		BreakEvent.Invoke();
	}
	
	public void SaveState() {
		_wasBroken = _isBroken;
	}

	public void ResetState() {
		_isBroken = _wasBroken;
		_collider2D.enabled = !_wasBroken;
		_renderer.enabled = !_wasBroken;
	}
}
