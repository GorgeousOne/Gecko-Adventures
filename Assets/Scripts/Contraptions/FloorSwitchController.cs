using System.Collections.Generic;
using UnityEngine;

public class FloorSwitchController : MonoBehaviour {

	[SerializeField] private SpriteRenderer rendering;
	[SerializeField] private Vector2 pressedOffset;
	[SerializeField] private List<Triggerable> connected;

	// [SerializeField] private float moveTime;
	
	private bool _isEnabled;
	private Vector2 _defaultPos;

	private void Start() {
		_defaultPos = rendering.transform.position;
	}

	// public void Update() {
		// Vector2 startPos = _startPos;
		// Vector2 targetPos = _startPos;
		// if (_isEnabled) {
			// targetPos += pressedOffset;
		// }
		// else {
			// startPos += pressedOffset;
		// }
		// float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
		// transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);
		// toggleAction.Invoke(_isEnabled);
	// }

	private void Toggle() {
		if (_isEnabled) {
			rendering.transform.position = _defaultPos + pressedOffset;
		} else {
			rendering.transform.position = _defaultPos;
		}
		foreach (Triggerable triggerable in connected) {
			triggerable.OnSwitchToggle(_isEnabled);
		}
	}
	
	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			collider.transform.parent = transform;
			_isEnabled = true;
			Toggle();
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			collider.transform.parent = null;
			_isEnabled = false;
			Toggle();
		}
	}
	
	private void OnDrawGizmos() {
		Gizmos.color = _isEnabled ? new Color(.75f, 0, 0) : new Color(.25f, 0, 0);
		foreach (Triggerable triggerable in connected) {
			Gizmos.DrawLine(transform.position, triggerable.transform.position);
		}
	}
}