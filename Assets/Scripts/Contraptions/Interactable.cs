using UnityEngine;

public abstract class Interactable : Resettable {

	[SerializeField] private Sprite keyHintSprite;
	[SerializeField] private Vector2 keyHintOffset = new (0, 1);
	
	private bool _isInPlayerRange;
	private PlayerControls _controls;
	private GameObject _keyHint;
	
	protected abstract void OnInteract();
	
	protected void OnEnable() {
		_controls = new PlayerControls();
		_controls.Enable();
	}

	protected void OnDisable() {
		_controls.Disable();
	}

	protected void Start() {
		if (keyHintSprite == null) {
			keyHintSprite = Resources.Load<Sprite>("Sprites/key-hint-e");
		}
		CreateKeyHint();			
	}

	protected void Update() {
		if (_controls.Player.Interact.WasPerformedThisFrame() && _isInPlayerRange) {
			OnInteract();
		}
	}
 
	protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isInPlayerRange = true;
			SetHintVisible(_isInPlayerRange);
		}
	}

	protected void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isInPlayerRange = false;
			SetHintVisible(_isInPlayerRange);
		}
	}

	protected void SetHintVisible(bool state) {
		_keyHint.SetActive(state);
	}
	
	protected void CreateKeyHint() {
		_keyHint = new GameObject("KeyHint");
		SpriteRenderer renderer = _keyHint.AddComponent<SpriteRenderer>();
		renderer.sprite = keyHintSprite;
		renderer.sortingLayerName = "Fore";
		_keyHint.SetActive(false);
		_keyHint.transform.parent = transform;
		_keyHint.transform.localPosition = keyHintOffset;
	}
}