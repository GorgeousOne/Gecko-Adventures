using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Rendering.Universal;

public class TongueMovement : MonoBehaviour {

	[SerializeField] private float extendTime = 0.2f;
	[SerializeField] private float stayExtendedTime = 0.2f;
	[SerializeField] private Transform pivot;
	[SerializeField] private LayerMask attachLayerMask;
	[SerializeField] private LayerMask collectLayerMask;
	[SerializeField] private TriggerEvent attachAction;
	[SerializeField] private UnityEvent detachAction;
	// added for lighting the gecko after eating a firefly
	[SerializeField] private GameObject player;
	[SerializeField] private float lightTimer = 2.0f;
	// ***************************************************

	private PlayerControls _controls;
	private CapsuleCollider2D _collider;
	private Camera _cam;
	
	private SpriteRenderer _renderer;
	private Vector2 _defaultPos;
	
	private Collider2D _attachment;
	private GameObject _pickup;

	private bool _canExtend = true;
	private float _extendStart;
	private float _extendDistance;
	private float _length;

	private bool _tongueShootPressed;
	private bool _isControllerUsed;

	// added for lighting the gecko after eating a firefly
	private int fireflyCounter = 0;
	private float waitTime = 0;
	private float lightOffSet = 0;
	// ***************************************************

	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Enable();
		_collider = GetComponent<CapsuleCollider2D>();
		// InputUser.onChange +=
		// 	(user, change, device) =>
		// 	{
		// 		if (change == InputUserChange.ControlSchemeChanged)
		// 			Debug.Log($"User {user} switched control scheme");
		// 	};
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
		_extendStart = -100;
	}

	private void Update() {
		if (IsAttached()) {
			AlignTongueToPoint(GetAttachPoint());
		} else if (IsExtending()) {
			UpdateExtendLength();
		} else if (_pickup) {

			// added for lighting the gecko after eating a firefly
			fireflyCounter += 1;
			Destroy(_pickup);

			if (player.GetComponent<Light2D>() == null) {
				player.AddComponent<Light2D>();
				player.GetComponent<Light2D>().intensity = 2;
				player.GetComponent<Light2D>().pointLightOuterRadius = 2.0f;
			}

			// waitTime += Time.deltaTime;

			// if (fireflyCounter == 1) {
			// 	Destroy(player.GetComponent<Light2D>(), lightTimer);
			// }

			// if (fireflyCounter > 1 && waitTime < lightTimer) {
			// 	float timeLeft = lightTimer - waitTime;
			// 	Destroy(player.GetComponent<Light2D>(), timeLeft + lightTimer);
			// }

			Destroy(player.GetComponent<Light2D>(), lightTimer);
			// *********************************************************************
			
		}
		if(WasTongueShootPerformed()) {
			_tongueShootPressed = true;
		}
	}

	private void UpdateExtendLength() {
		float newLength = Mathf.SmoothStep(0, _length, GetExtendProgress());
		SetExtendDistance(newLength);
		_collider.size = new Vector2(newLength, _collider.size.y);
		_collider.offset = new Vector2(-0.5f * newLength, 0);
	}
	
	private void AlignTongueToPoint(Vector2 attachPoint) {
		pivot.right = GetAimDir(attachPoint);
		float distance = (attachPoint - (Vector2) pivot.position).magnitude;
		SetExtendDistance(distance);
	}
	
	private bool WasTongueShootPerformed() {
		return _controls.Player.TongueShoot.WasPerformedThisFrame() || 
		       _controls.Player.TongueShootGamepad.WasPerformedThisFrame() &&
		       !MathUtil.IsZero(_controls.Player.TongueShootGamepad.ReadValue<Vector2>().magnitude);
	}
	
	private void FixedUpdate() {
		//animates tongue extend on left click
		if (_tongueShootPressed && _canExtend && !IsExtending()) {
			Vector2 gamepadAim = _controls.Player.TongueShootGamepad.ReadValue<Vector2>();

			if (!MathUtil.IsZero(gamepadAim.x) || !MathUtil.IsZero(gamepadAim.y)) {
				pivot.right = gamepadAim.normalized;
			} else {
				Vector2 mouseScreenPos = _controls.Player.TongueAim.ReadValue<Vector2>();
				Vector2 mousePos = _cam.ScreenToWorldPoint(mouseScreenPos);
				pivot.right = GetAimDir(mousePos);
			}
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
	
	//returns the width of the unscaled tongue sprite I think
	public float GetMaxLength() {
		return _length;
	}

	public float GetExtendDistance() {
		return _extendDistance;
	}
	
	public bool IsExtending() {
		return Time.time - _extendStart < 2 * extendTime + stayExtendedTime;
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
		float dt = (Time.time - _extendStart);
		float progress = dt / extendTime;

		if (dt < extendTime) {
			return dt / extendTime;
		} else if (dt > extendTime + stayExtendedTime) {
			return (2 * extendTime + stayExtendedTime - dt) / extendTime;
		}
		return 1;
		// float halfTime = extendTime + 0.5f * stayExtendedTime;
		// float dt = (Time.time - _extendStart);
		// float shiftedDt = Math.Abs((dt - (extendTime + 0.25f * stayExtendedTime)) / extendTime - halfTime) - stayExtendedTime / (4 * extendTime);
		// return 1 - Mathf.Clamp01(shiftedDt);
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
		// if (!IsExtending() || IsBehindTongue(other.transform.position)) {
		if (!IsExtending()) {
			return;
		}
		GameObject otherObject = other.gameObject;
		
		if (MaskContains(attachLayerMask, otherObject)) {
			AttachTo(other);
		} else if (MaskContains(collectLayerMask, otherObject)) {
			AlignTongueToPoint(otherObject.transform.position);
			PickUp(otherObject);
		}
	}
	
	private bool MaskContains(LayerMask mask, GameObject other) {
		return (mask & 1 << other.layer) != 0;
	}
}

[Serializable]
public class TriggerEvent : UnityEvent<Collider2D> {}