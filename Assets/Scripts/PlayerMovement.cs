using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	[Header("General")]
	[SerializeField] private Transform playerTransform;
	[SerializeField] private LayerMask solidsLayerMask;
	[SerializeField] private TongueControl tongue;
	[SerializeField] private Animator spriteAnimator;

	[Header("Walk")]
	[SerializeField] private float maxWalkSpeed = 10;
	[SerializeField] private float accelerateTime = 0.2f;
	
	[Header("Jump")]
	[SerializeField] private float jumpHeight = 3.25f;
	[SerializeField] private float jumpPressedRememberDuration = 0.2f;
	[SerializeField] private float groundedRememberDuration = 0.1f;
	
	[Header("Other")]
	[SerializeField] private float swingForce = 20;
	[SerializeField] private float ropingSpeed = 10;
	[SerializeField] private float crouchHeight = .9f;
	[SerializeField] private float maxCrouchSpeed = 4f;
	
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

	private float _lastMovementInput;
	private bool _jumpInputPerformed;
	private bool _isCrouching;
	private bool _wantsCrouch;
	private float _defaultHeight;
	private float _defaultCapsuleOffY;
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.TongueExtend.performed += _ => _isExtendingTongue = true;
		_controls.Player.TongueExtend.canceled += _ => _isExtendingTongue = false;
		_controls.Player.TongueRetract.performed += _ => _isRetractingTongue = true;
		_controls.Player.TongueRetract.canceled += _ => _isRetractingTongue = false;
		_controls.Player.Crouch.performed += _ => _wantsCrouch = true;
		_controls.Player.Crouch.canceled += _ => _wantsCrouch = false;
		
		_controls.Enable();
	}

	private void OnDisable() {
		_controls.Disable();
	}
	
	private void Start() {
		_rigid = GetComponent<Rigidbody2D>();
		_capsule = GetComponent<CapsuleCollider2D>();
		_defaultHeight = _capsule.size.y;
		_defaultCapsuleOffY = _capsule.offset.y;
	}
	
	private void Update() {
		_lastMovementInput = _controls.Player.Move.ReadValue<float>();
		
		if (_controls.Player.Jump.WasPerformedThisFrame()) {
			_jumpInputPerformed = true;
		}
	}

	private void FixedUpdate() {
		bool isGrounded = CheckGrounding();
		CheckCrouching();
		CheckJumping();
		CheckTongueLengthChange();
		CheckHorizontalMovement(isGrounded, tongue.IsAttached() && !isGrounded);
		_lastMovementInput = 0;
		_jumpInputPerformed = false;
		
	}

	private bool CheckGrounding() {
		_groundedRemember -= Time.deltaTime;
		_jumpPressedRemember -= Time.deltaTime;
		bool isGrounded = IsGrounded();
		
		if (isGrounded) {
			_groundedRemember = groundedRememberDuration;
		}
		return isGrounded;
	}
	
	/**
	 * Creates a capsule close below the players capsule and checks if it intersects with any ground
	 */
	private bool IsGrounded() {
		Vector2 capsuleOffset = _capsule.offset;
		Vector2 capsuleSize = _capsule.size;
		
		//shrinks capsule width to avoid wall jumps
		capsuleSize.x -= 0.1f;
		
		Vector2 capsuleOrigin = (Vector2) transform.position + capsuleOffset + new Vector2(0, -0.01f);
		return Physics2D.OverlapCapsule(capsuleOrigin, capsuleSize, _capsule.direction, 0, solidsLayerMask);
	}
	
	private void CheckJumping() {
		if (_jumpInputPerformed) {
			//detaches tongue when jumping
			if (tongue.IsAttached()) {
				tongue.Detach();
				return;
			}
			_jumpPressedRemember = jumpPressedRememberDuration;
		}
		//performes jump (with small threshold before landing and after starting to fall)
		if(_jumpPressedRemember > 0 && _groundedRemember > 0) {
			ApplyJumpVelocity();
		}
	}

	private void CheckTongueLengthChange() {
		if (tongue.IsAttached()) {
			float newTongueLength = _tongueConnection.distance;
			
			if (_isExtendingTongue) {
				newTongueLength += ropingSpeed * Time.fixedDeltaTime;
			} else if (_isRetractingTongue) {
				newTongueLength -= ropingSpeed * Time.fixedDeltaTime;
			}
			_tongueConnection.distance = Mathf.Clamp(newTongueLength, 1, tongue.GetMaxLength());
		}
	}
	
	private void CheckHorizontalMovement(bool isGrounded, bool isHanging) {
		float horizontalInput = _lastMovementInput;

		_rigid.velocity = isHanging ?
				CalcSwingVelocity(_rigid.velocity, horizontalInput):
				CalcWalkVelocity(_rigid.velocity, horizontalInput);

		bool hasTurnedAround = Mathf.Sign(_rigid.velocity.x) != (_isFacingRight ? -1 : 1);

		if (!isZero(_rigid.velocity.x) && hasTurnedAround) {
			Flip();
		}
		spriteAnimator.SetFloat("Speed", isGrounded ? Mathf.Abs(_rigid.velocity.x) : 0f);
	}

	private Vector2 CalcWalkVelocity(Vector2 velocity, float horizontalInput) {
		Vector2 newVelocity = new Vector2(0, velocity.y);
		
		if (isZero(horizontalInput)) {
			newVelocity.x = GetDecelerated(velocity.x);
		} else {
			bool isTuningAround = Mathf.Sign(horizontalInput) != Mathf.Sign(velocity.x);
			
			if (!isZero(velocity.x) && isTuningAround) {
				newVelocity.x = 0;
			}else {
				newVelocity.x = GetAccelerated(velocity.x, Mathf.Sign(horizontalInput));
			}
		}
		return newVelocity;
	}
	
	private Vector2 CalcSwingVelocity(Vector2 velocity, float horizontalInput) {
		if (isZero(horizontalInput) || transform.position.y > tongue.GetAttachPoint().y) {
			return velocity;
		}
		Vector2 impulse = GetSwingRightVector2() * Mathf.Sign(horizontalInput) * swingForce * Time.fixedDeltaTime;
		return _rigid.velocity + impulse;
	}
	
	//returns like... the tangent direction for the swinging circle
	public Vector2 GetSwingRightVector2() {
		if (tongue.IsAttached()) {
			Vector2 attachDirection = tongue.GetAttachPoint() - (Vector2) transform.position;
			return new Vector2(attachDirection.y, -attachDirection.x).normalized;
		}
		return Vector2.zero;
	}
	
	private bool isZero(float f, float margin = 0.01f) {
		return Mathf.Abs(f) < margin;
	}
	
	//approximates velocity needed to reach given jump height
	/*
	 * Velocity is calculated every update, so perfectly calculating it probably won't work.
	 * Assuming that the velocity is calculated each update with: vNew = (v - g) / (1 + drag)
	 * and the formula for the start velocity needed in perpendicular throw to reach a certain height is: v = sqrt(yMax * 2 * g)
	 * perhaps they can be combined like vJump = sqrt(yMax * 2 * g * (1 + 0.5 * drag))
	 */
	private void ApplyJumpVelocity() {
		_jumpPressedRemember = 0;
		_groundedRemember = 0;
		float gravity = _rigid.gravityScale * -Physics2D.gravity.y;
		float dragFactor = (1 + 0.5f * _rigid.drag);
		float velocity = Mathf.Sqrt(jumpHeight * 2 * gravity * dragFactor);
		_rigid.velocity = new Vector2(_rigid.velocity.x, velocity);
	}
	
	private float GetAccelerated(float currentSpeed, float direction) {
		float movementSpeed = _isCrouching ? maxCrouchSpeed : maxWalkSpeed;
		float acceleration = Time.fixedDeltaTime / accelerateTime * movementSpeed;
		float newSpeed = currentSpeed + direction * acceleration;
		return Math.Clamp(newSpeed, -movementSpeed, movementSpeed);
	}

	private float GetDecelerated(float currentSpeed) {
		float movementSpeed = _isCrouching ? maxCrouchSpeed : maxWalkSpeed;
		float deceleration = (Time.fixedDeltaTime / accelerateTime * movementSpeed);

		if (Mathf.Abs(currentSpeed) < deceleration) {
			return 0;
		}
		float newSpeed = currentSpeed - Mathf.Sign(currentSpeed) * deceleration;
		return Math.Clamp(newSpeed, -movementSpeed, movementSpeed);
	}

	private void CheckCrouching() {
		if (_wantsCrouch) {
			if (!_isCrouching) {
				Crouch();
			}
		} else if (_isCrouching) {
			if (CanStandUp()) {
				StandUp();
			}
		}
	}
	private void Crouch() {
		_isCrouching = true;
		spriteAnimator.SetBool("IsCrouching", _isCrouching);
		_capsule.size = new Vector2(_capsule.size.x, crouchHeight);
		_capsule.offset = new Vector2(_capsule.offset.x, _defaultCapsuleOffY - (_defaultHeight - crouchHeight) / 2);
		_rigid.velocity = new Vector2(
			Mathf.Clamp(_rigid.velocity.x, -maxCrouchSpeed, maxCrouchSpeed),
			_rigid.velocity.y);
	}

	private bool CanStandUp() {
		Vector2 capsuleOffset = _capsule.offset;
		Vector2 capsuleSize = _capsule.size;
		
		//shrinks capsule width to avoid wall intersections
		capsuleSize.x -= 0.1f;
		
		Vector2 capsuleOrigin = (Vector2) transform.position + capsuleOffset + new Vector2(0, 0.5f);
		return !Physics2D.OverlapCapsule(capsuleOrigin, capsuleSize, _capsule.direction, 0, solidsLayerMask);
	}
	
	private void StandUp() {
		_isCrouching = false;
		spriteAnimator.SetBool("IsCrouching", _isCrouching);
		_capsule.size = new Vector2(_capsule.size.x, _defaultHeight);
		_capsule.offset = new Vector2(_capsule.offset.x, _defaultCapsuleOffY);
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