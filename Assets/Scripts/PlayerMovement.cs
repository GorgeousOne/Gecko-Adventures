using System;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private Transform playerTransform;
	[SerializeField] private LayerMask solidsLayerMask;
	[SerializeField] private TongueControl tongue;

	[SerializeField] private float ropingSpeed = 0.03f;
	[SerializeField] private float jumpHeight = 10;
	[SerializeField] private float jumpPressedRememberDuration = 0.2f;
	[SerializeField] private float groundedRememberDuration = 0.2f;
	[SerializeField] private float maxHorizontalVelocity = 10;
	[SerializeField] private float accelerateDuration = 0.2f;
	[SerializeField] private float decelerateDuration = 0.2f;
	
	private float _jumpPressedRemember;
	private float _groundedRemember;
	private float _startRunTime;
	private float _stopRunTime;
	private float _stopRunVelocity;
	private bool _wasRunning;
	private bool _isFacingRight;

	private Rigidbody2D _rigid;
	private CapsuleCollider2D _capsule;
	private PlayerControls _controls;
	private DistanceJoint2D _tongueConnection;

	private bool _isExtendingTongue;
	private bool _isRetractingTongue;

	private void Awake() {
		_controls = new PlayerControls();
		_controls.Player.TongueExtend.performed += _ => _isExtendingTongue = true;
		_controls.Player.TongueExtend.canceled += _ => _isExtendingTongue = false;
		_controls.Player.TongueRetract.performed += _ => _isRetractingTongue = true;
		_controls.Player.TongueRetract.canceled += _ => _isRetractingTongue = false;
	}

	private void OnEnable() {
		_controls.Enable();
	}

	private void OnDisable() {
		_controls.Disable();
	}
	
	private void Start() {
		_rigid = GetComponent<Rigidbody2D>();
		_capsule = GetComponent<CapsuleCollider2D>();
	}
	
	private void Update() {
		_groundedRemember -= Time.deltaTime;
		_jumpPressedRemember -= Time.deltaTime;
		bool isGrounded = IsGrounded();
		
		if (isGrounded) {
			_groundedRemember = groundedRememberDuration;
		}
		if (_controls.Player.Jump.WasPerformedThisFrame()) {
			//detaches tongue when jumping
			if (tongue.IstAttached()) {
				tongue.Detach();
				Jump();
			} else {
				_jumpPressedRemember = jumpPressedRememberDuration;
			}
		}
		//retract and extend tongue
		if (tongue.IstAttached()) {
			float newTongueLength = _tongueConnection.distance;
			
			if (_isExtendingTongue) {
				newTongueLength += ropingSpeed;
			} else if (_isRetractingTongue) {
				newTongueLength -= ropingSpeed;
			}
			_tongueConnection.distance = Mathf.Clamp(newTongueLength, 1, tongue.GetMaxLength());
		}
		//performes jump (with small threshold before landing and after starting to fall)
		if (_jumpPressedRemember > 0 && _groundedRemember > 0) {
			_jumpPressedRemember = 0;
			_groundedRemember = 0;
			Jump();
		}
		float horizontalVelocity = _rigid.velocity.x;
		float horizontalInput = _controls.Player.Move.ReadValue<float>();
		bool isMoving = Mathf.Abs(horizontalVelocity) > 0.01f;
		
		//starts slowing down when if vertical player input stops
		if (Mathf.Abs(horizontalInput) < 0.01f) {
			//only slows down if not dangling from tongue
			if (!tongue.IstAttached() || isGrounded) {
				horizontalVelocity = GetDecelerated(horizontalVelocity);
			}
		//accelerates continuously when moving towards one direction
		} else {
			//damps horizontal velocity when switching direction
			if (isMoving && Mathf.Sign(horizontalInput) != Mathf.Sign(horizontalVelocity)) {
				horizontalVelocity = 0;
			}
			horizontalVelocity = GetAccelerated(horizontalVelocity, Mathf.Sign(horizontalInput));
		}
		if (isMoving && Mathf.Sign(horizontalVelocity) != (_isFacingRight ? -1 : 1)) {
			Flip();
		}
		if (float.IsNaN(horizontalVelocity)) {
			horizontalVelocity = 0;
		}
		_rigid.velocity = new Vector2(horizontalVelocity, _rigid.velocity.y);
	}

	private void Jump() {
		_rigid.velocity = new Vector2(_rigid.velocity.x, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpHeight));
	}
	//uses shifted and stretched quadratic function to smooth velocity during acceleration
	private float GetAccelerated(float currentSpeed, float direction) {
		float tmax = accelerateDuration;
		float tmax2 = tmax * tmax;
		float vmax = maxHorizontalVelocity;
		float y = Mathf.Abs(currentSpeed);
		
		float t = tmax - Mathf.Sqrt(-tmax2 * (y - vmax) / vmax);
		float tNext = Mathf.Clamp01(t + Time.deltaTime);

		float yNext = -vmax / tmax2 * Mathf.Pow(tNext - tmax, 2) + vmax;
		return yNext * direction;
	}
	
	private float GetDecelerated(float currentSpeed) {
		float tmax = decelerateDuration;
		float tmax2 = tmax * tmax;
		float vmax = maxHorizontalVelocity;
		float y = Mathf.Abs(currentSpeed);
		
		float t = tmax - Mathf.Sqrt(tmax2 * y / vmax);
		float tNext = Mathf.Clamp01(t + Time.deltaTime);
		float yNext = vmax / tmax2 * Mathf.Pow(tNext - tmax, 2);
		
		return yNext * Mathf.Sign(currentSpeed);
	}

	/**
	 * Creates a capsule close below the players capsule and checks if it intersects with any ground
	 */
	private bool IsGrounded() {
		Vector2 localScale = playerTransform.localScale;
		Vector2 capsuleSize = _capsule.size;
		Vector2 capsuleOffset = _capsule.offset;
		
		capsuleSize.Scale(localScale);
		capsuleOffset.Scale(localScale);
		
		Vector2 capsuleOrigin = (Vector2) playerTransform.position + capsuleOffset + new Vector2(0, -0.01f);
		return Physics2D.OverlapCapsule(capsuleOrigin, capsuleSize, _capsule.direction, 0, solidsLayerMask);
	}
	
	private void Flip() {
		_isFacingRight = !_isFacingRight;

		// Multiplies the player's x local scale by -1.
		Vector3 scale = playerTransform.localScale;
		scale.x *= -1;
		playerTransform.localScale = scale;
	}

	public void OnTongueAttach(Collider2D other) {
		Rigidbody2D anchor = other.attachedRigidbody;
		_tongueConnection = this.AddComponent<DistanceJoint2D>();
		_tongueConnection.enableCollision = true;
		_tongueConnection.connectedBody = anchor;
		_tongueConnection.autoConfigureDistance = false;
		_tongueConnection.distance = tongue.GetMaxLength();
		_tongueConnection.maxDistanceOnly = true;
	}

	public void OnTongueDetach() {
		Destroy(_tongueConnection);
	}
}