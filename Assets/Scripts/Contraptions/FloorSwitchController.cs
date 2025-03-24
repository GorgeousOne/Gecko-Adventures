using UnityEngine;

public class FloorSwitchController : Switch {

	[SerializeField] private SpriteRenderer rendering;
	[SerializeField] private Vector2 pressedOffset;
	[SerializeField] private float deepPitch = 0.9f;
	
	private Vector2 _defaultPos;
	private AudioSource _sound;
	private void Start() {
		_defaultPos = rendering.transform.position;
		_sound = GetComponent<AudioSource>();
	}

	protected new void Toggle() {
		base.Toggle();
		
		if (IsEnabled) {
			rendering.transform.position = _defaultPos + pressedOffset;
		} else {
			rendering.transform.position = _defaultPos;
		}
		_sound.pitch = IsEnabled ? 1f : deepPitch;
		_sound.Play();
	}
	
	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.CompareTag("Player")) {
			if (!IsEnabled) {
				Toggle();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.CompareTag("Player")) {
			if (IsEnabled) {
				Toggle();
			}
		}
	}
}