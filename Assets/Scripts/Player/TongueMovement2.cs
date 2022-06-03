using UnityEngine;
using UnityEngine.Events;

public class TongueMovement2 : MonoBehaviour {

	[SerializeField] private float extendTime = 0.4f;
	[SerializeField] private Transform pivot;
	[SerializeField] private LayerMask attachLayerMask;
	[SerializeField] private LayerMask collectLayerMask;
	[SerializeField] private TriggerEvent attachAction;
	[SerializeField] private UnityEvent detachAction;

	private PlayerControls _controls;
	private Camera _cam;
	
	private SpriteRenderer _renderer;
	private Vector2 _defaultPos;
	
	private Collider2D _attachment;
	private GameObject _pickup;
	// private bool _canAttach = true;

	private bool _canExtend = true;
	private float _extendStart;
	private float _extendDistance;
	private float _length;

	private bool _tongueShootPressed;
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Enable();
	}

	private void OnDisable() {
		_controls.Disable();
	}
	
	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		_defaultPos = transform.localPosition;
		_length = _renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit;
		_extendStart = -extendTime;
		
		_cam = Camera.main;
	}

	private void Update() {
		if (IsAttached()) {
			Vector2 attachPoint = GetAttachPoint();
			pivot.right = GetAimDir(attachPoint);
			float distance = (attachPoint - (Vector2) pivot.position).magnitude;
			SetExtendDistance(distance);
		} else if (IsExtending()) {
			float extendProgress = GetExtendProgress();

			if (extendProgress < .5f) {
				SetExtendDistance(Mathf.SmoothStep(0, _length, 2 * extendProgress));
			} else {
				SetExtendDistance(Mathf.SmoothStep(_length, 0, 2 * (extendProgress - .5f)));
			}
		} else if (_pickup) {
			Destroy(_pickup);
		}
		if (_controls.Player.TongueShoot.WasPerformedThisFrame()) {
			_tongueShootPressed = true;
		}
	}

	private void FixedUpdate() {
		//animates tongue extend on left click
		if (_tongueShootPressed && _canExtend && !IsExtending()) {
			Vector2 mouseScreenPos = _controls.Player.MousePos.ReadValue<Vector2>();
			Vector2 mousePos = _cam.ScreenToWorldPoint(mouseScreenPos);
			pivot.right = GetAimDir(mousePos);
			PlayExtend();
		}
		_tongueShootPressed = false;
	}

	public void SetExtendingEnabled(bool state) {
		_canExtend = state;
	}
	
	private Vector2 GetAimDir(Vector2 aim) {
		return new Vector2(
			aim.x - pivot.position.x,
			aim.y - pivot.position.y);
	}
	
	// public void SetAttachable(bool canAttach) {
	// 	_canAttach = canAttach;
	//
	// 	if (!_canAttach && IsAttached()) {
	// 		Detach();
	// 	}
	// }

	//returns the width of the unscaled tongue sprite I think
	public float GetMaxLength() {
		return _length;
	}

	public float GetExtendDistance() {
		return _extendDistance;
	}
	
	public bool IsExtending() {
		return Time.time - _extendStart < extendTime;
	}

	public Vector3 GetAttachPoint() {
		return _attachment.bounds.center;
	}
	
	public float GetExtendAngle() {
		if (!IsAttached() && !IsExtending()) {
			return 0f;
		}
		return MathUtil.WrapToPi(pivot.rotation.eulerAngles.z);
	}
	
	public bool IsAttached() {
		return _attachment;
	}

	public void SetExtendDistance(float distance) {
		_extendDistance = distance;
		transform.localPosition = _defaultPos + new Vector2(_extendDistance, 0);
	}

	public void PlayExtend() {
		_extendStart = Time.time;
	}

	public void AttachTo(Collider2D other) {
		if (IsAttached()) {
			return;
		}
		_extendStart = Time.time - extendTime;
		_attachment = other;
		attachAction.Invoke(other);
	}

	public void Detach() {
		_attachment = null;
		SetExtendDistance(0);
		detachAction.Invoke();
	}

	public float GetExtendProgress() {
		return (Time.time - _extendStart) / extendTime;
	}

	public void SetExtendProgress(float percent) {
		_extendStart = Time.time - extendTime * Mathf.Clamp01(percent);
	}
	
	public void PickUp(GameObject pickup) {
		if (_pickup) {
			return;
		}
		float extendProgress = GetExtendProgress();

		if (extendProgress < .5f) {
			SetExtendProgress(1 - extendProgress);
		}
		_pickup = pickup;
		_pickup.transform.parent = transform;
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (!IsExtending() || IsBehindTongue(other.transform.position)) {
			return;
		}
		GameObject otherObject = other.gameObject;
		
		if (MaskContains(attachLayerMask, otherObject)) {
			AttachTo(other);
		} else if (MaskContains(collectLayerMask, otherObject)) {
			transform.right = GetAimDir(otherObject.transform.position);
			PickUp(otherObject);
		}
	}
	
	private bool MaskContains(LayerMask mask, GameObject other) {
		return (mask & 1 << other.layer) != 0;
	}
	
	private bool IsBehindTongue(Vector3 point) {
		return Vector2.Dot(transform.right, point - transform.position) < 0;
	}
}

// [Serializable]
// public class TriggerEvent : UnityEvent<Collider2D> {}