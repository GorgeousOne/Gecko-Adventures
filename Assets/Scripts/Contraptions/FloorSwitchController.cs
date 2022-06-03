using UnityEngine;

public class FloorSwitchController : Switch {

	[SerializeField] private SpriteRenderer rendering;
	[SerializeField] private Vector2 pressedOffset;

	// [SerializeField] private float moveTime;
	
	private Vector2 _defaultPos;

	private void Start() {
		_defaultPos = rendering.transform.position;
	}

	protected new void Toggle() {
		base.Toggle();
		
		if (IsEnabled) {
			rendering.transform.position = _defaultPos + pressedOffset;
		} else {
			rendering.transform.position = _defaultPos;
		}
	}
	
	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			if (!IsEnabled) {
				Toggle();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			if (IsEnabled) {
				Toggle();
			}
		}
	}
}