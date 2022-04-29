using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
	
	[SerializeField] protected UnityEvent interactAction;
	
	private bool _isInPlayerRange;
	private PlayerControls _controls;

	private void Awake() {
		_controls = new PlayerControls();
	}

	private void OnEnable() {
		_controls.Enable();
	}

	private void OnDisable() {
		_controls.Disable();
	}
	
	private void Update() {
		if (_controls.Player.Interact.WasPerformedThisFrame() && _isInPlayerRange) {
			interactAction.Invoke();
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