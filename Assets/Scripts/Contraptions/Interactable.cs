using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour {
	
	private bool _isInPlayerRange;
	private PlayerControls _controls;

	protected abstract void OnInteract();
	
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Enable();
	}

	private void OnDisable() {
		_controls.Disable();
	}
	
	private void Update() {
		if (_controls.Player.Interact.WasPerformedThisFrame() && _isInPlayerRange) {
			OnInteract();
		}
	}
 
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isInPlayerRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isInPlayerRange = false;
		}
	}
}