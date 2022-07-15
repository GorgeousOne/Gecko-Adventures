using UnityEngine;

public abstract class Interactable : MonoBehaviour {

	// [SerializeField] private Sprite hintBackground;
	// [SerializeField] private float padding = .3125f;

	// [SerializeField] private Vector2 bubbleOffset = new (0, 1);
	// [SerializeField] private Vector2 keyHintOffset = new (0, .03125f);
	
	private bool _isPlayerInRange;
	private PlayerControls _controls;
	private BubleKeyHint _keyHint;
	
	// private GameObject _keyHintBubble;
	
	protected abstract void OnInteract();
		
	protected void OnEnable() {
		// _keyHint = GetComponent<BubleKeyHint>();
		// _keyHint.SetHintVisible(_isPlayerInRange);
		_controls = new PlayerControls();
		_controls.Enable();
	}

	protected void OnDisable() {
		_controls.Disable();
	}

	protected void Update() {
		if (_controls.Player.Interact.WasPerformedThisFrame() && _isPlayerInRange) {
			OnInteract();
		}
	}
 
	protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isPlayerInRange = true;
			// _keyHint.SetHintVisible(_isPlayerInRange);
			// SetHintVisible(_isPlayerInRange);
		}
	}

	protected void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isPlayerInRange = false;
			// _keyHint.SetHintVisible(_isPlayerInRange);
		}
	}
}