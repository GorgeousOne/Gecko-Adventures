using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	[Header("General")]
	[SerializeField] private LayerMask solidsLayerMask;
	[SerializeField] private CapsuleCollider2D capsule;
	[SerializeField] private TongueMovement tongue;
	[SerializeField] private Animator bodyAnimator;
	
	[Header("Walk")]
	[SerializeField] private float maxWalkSpeed = 10;
	/// <summary>
	/// time to get from 0 to maxWalkSpeed 
	/// </summary>
	[SerializeField] private float accelerateTime = 0.2f;
	
	[Header("Jump")]
	// amount of units the player can jump high
	[SerializeField] private float jumpHeight = 3.25f;
	/// <summary>
	/// time buffer in which player still jumps after not touching ground anymore
	/// </summary>
	[SerializeField] private float jumpPressedRememberDuration = 0.2f;
	/// <summary>
	/// time buffer in which player still jump before even touching ground
	/// </summary>
	[SerializeField] private float groundedRememberDuration = 0.2f;
	
	[Header("Crouch")]
	[SerializeField] private float maxCrouchSpeed = 4f;
	[SerializeField] private float crouchHeight = .9f;
	[SerializeField] private Transform ceilingCheck;
	[SerializeField] private Transform crushCheck;
	
	[Header("Swing")]
	[SerializeField] private float swingForce = 17;
	[SerializeField] private float ropingSpeed = 10;

	private bool _isEnabled = true;
	private PlayerSpawning _playerSpawning;
	private float _jumpPressedRemember;
	private float _groundedRemember;
	private float _startRunTime;
	private float _stopRunTime;
	private float _stopRunVelocity;
	private bool _wasRunning;
	private bool _isFacingRight;

	private Rigidbody2D _rigid;
	// private CapsuleCollider2D capsule;
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

	private AudioSource[] _walkingAudios;
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.TongueExtend.performed += _ => _isExtendingTongue = true;
		_controls.Player.TongueExtend.canceled += _ => _isExtendingTongue = false;
		_controls.Player.TongueRetract.performed += _ => _isRetractingTongue = true;
		_controls.Player.TongueRetract.canceled += _ => _isRetractingTongue = false;
		_controls.Player.Crouch.performed += _ => _wantsCrouch = true;
		_controls.Player.Crouch.canceled += _ => _wantsCrouch = false;
		_controls.Enable();

		_playerSpawning = GetComponent<PlayerSpawning>();
		_playerSpawning.playerDeathEvent.AddListener(tongue.Detach);

		_walkingAudios = GetComponents<AudioSource>();
		_walkingAudios[0].enabled = false;
		_walkingAudios[1].enabled = false;
		_walkingAudios[2].enabled = false;
	}

	private void OnDisable() {
		_controls.Disable();
		_playerSpawning.playerDeathEvent.RemoveListener(tongue.Detach);
	}
	
	private void Start() {
		_rigid = GetComponent<Rigidbody2D>();
		_defaultHeight = capsule.size.y;
		_defaultCapsuleOffY = capsule.offset.y;
	}
	
	/// <summary>
	/// Reads control inputs
	/// </summary>
	private void Update() {
		if (_playerSpawning.IsDead() || !_isEnabled) {
			return;
		}
		_lastMovementInput = _controls.Player.Move.ReadValue<float>();
		
		if (_controls.Player.Jump.WasPerformedThisFrame()) {
			_jumpInputPerformed = true;
		}

		if (tongue.IsAttached()) {
			// disable jumping sound
			_walkingAudios[2].enabled = false;
		}

		// enable sound for walking & disable sound otherwise
		_walkingAudios[0].enabled = _lastMovementInput != 0 && !tongue.IsAttached() && !_isCrouching;
		// enable sound for crouching & disable sound otherwise
		_walkingAudios[1].enabled = _lastMovementInput != 0 && !tongue.IsAttached() && _isCrouching;
	}

	private void FixedUpdate() {
		if (IsBeingCrushed()) {
			_playerSpawning.Die();
		}
		bool isGrounded = CheckGrounding();
		
		if (!_playerSpawning.IsDead()) {
			CheckCrouching();
			CheckJumping();
			CheckTongueLengthChange();
		}
		CheckHorizontalMovement(tongue.IsAttached() && !isGrounded);
		_lastMovementInput = 0;
		_jumpInputPerformed = false;
		
		bodyAnimator.SetFloat("VelY", _rigid.velocity.y);
		bodyAnimator.SetBool("Crouching", _isCrouching);
		bodyAnimator.SetFloat("Speed", isGrounded ? Mathf.Abs(_rigid.velocity.x) : 0f);
	}

	public void SetMovingEnabled(bool state) {
		_isEnabled = state;
		tongue.SetExtendingEnabled(state);
	}

	public bool CheckGrounding() {
		_groundedRemember -= Time.fixedDeltaTime;
		_jumpPressedRemember -= Time.fixedDeltaTime;
		bool isGrounded = IsGrounded();
		
		if (isGrounded) {
			_groundedRemember = groundedRememberDuration;
			// disable jumping sound
			_walkingAudios[2].enabled = false;
		}
		return isGrounded;
	}
	
	/**
	 * Creates a capsule close below the players capsule and checks if it intersects with any ground
	 */
	private bool IsGrounded() {
		Vector2 capsuleSize = capsule.size;
		
		//shrinks capsule width to avoid wall jumps
		capsuleSize.x -= 0.1f;
		float groundCheckHeight = transform.parent == null ? .1f : .3f;
		
		Vector2 capsuleOrigin = (Vector2) capsule.transform.position + capsule.offset - new Vector2(0, groundCheckHeight);
		Physics2D.queriesHitTriggers = false;
		bool isGrounded = Physics2D.OverlapCapsule(capsuleOrigin, capsuleSize, capsule.direction, 0, solidsLayerMask);
		Physics2D.queriesHitTriggers = true;
		return isGrounded;
	}
	
	private void CheckJumping() {
		if (_jumpInputPerformed) {
			//only detaches tongue when jumping
			if (tongue.IsAttached()) {
				tongue.Detach();
				// enable jumping sound after detaching
				_walkingAudios[2].enabled = true;
				return;
			}
			_jumpPressedRemember = jumpPressedRememberDuration;
		}
		//performes jump (with small threshold before landing and after starting to fall)
		if(_jumpPressedRemember > 0 && _groundedRemember > 0) {
			ApplyJumpVelocity();
			// enable jumping sound
			_walkingAudios[2].enabled = true;
		}		
	}

	/// <summary>
	/// Extends swinging tongue on right click and extends it on left click
	/// </summary>
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
	
	private void CheckHorizontalMovement(bool isHanging) {
		float horizontalInput = _lastMovementInput;

		_rigid.velocity = isHanging ?
				CalcSwingVelocity(_rigid.velocity, horizontalInput) :
				CalcWalkVelocity(_rigid.velocity, horizontalInput);
	}

	/// <summary>
	/// Accelerates and decelerates player based on if horizontal movement input was performed
	/// </summary>
	/// <param name="velocity">current velocity of player</param>
	/// <param name="horizontalInput">-1 or 1 if A or D pressed</param>
	/// <returns></returns>
	private Vector2 CalcWalkVelocity(Vector2 velocity, float horizontalInput) {
		Vector2 newVelocity = new Vector2(0, velocity.y);
		
		//slows player down if no movement input
		if (MathUtil.IsZero(horizontalInput)) {
			newVelocity.x = GetDecelerated(velocity.x);
		} else {
			bool isTurningAround = Mathf.Sign(horizontalInput) != Mathf.Sign(velocity.x);
			
			//slows down player if not standing still already
			if (!MathUtil.IsZero(velocity.x) && isTurningAround) {
				newVelocity.x = 0;
			}else {
				newVelocity.x = GetAccelerated(velocity.x, Mathf.Sign(horizontalInput));
			}
		}
		return newVelocity;
	}
	
	/// <summary>
	/// Calculates the accelerated velocity of player when swinging 
	/// </summary>
	/// <param name="velocity"></param>
	/// <param name="horizontalInput"></param>
	/// <returns></returns>
	private Vector2 CalcSwingVelocity(Vector2 velocity, float horizontalInput) {
		//does not accelerate if no input or if highest swing point reached
		if (MathUtil.IsZero(horizontalInput) || transform.position.y > tongue.GetAttachPoint().y) {
			return velocity;
		}
		//adds an impulse to velocity based on the swinging angle and input direction
		Vector2 impulse = GetSwingRightVector2() * (Mathf.Sign(horizontalInput) * swingForce * Time.fixedDeltaTime);
		return velocity + impulse;
	}
	
	/// <summary>
	/// returns like... the tangent direction of the current point on the swinging circle
	/// </summary>
	/// <returns></returns>
	public Vector2 GetSwingRightVector2() {
		if (tongue.IsAttached()) {
			Vector2 attachDirection = tongue.GetAttachPoint() - transform.position;
			return new Vector2(attachDirection.y, -attachDirection.x).normalized;
		}
		return Vector2.zero;
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
	
	/// <summary>
	/// Accelerates the current speed linearly
	/// </summary>
	/// <param name="currentSpeed"></param>
	/// <param name="direction">+1 or -1 for left or right</param>
	/// <returns></returns>
	private float GetAccelerated(float currentSpeed, float direction) {
		float maxMovementSpeed = _isCrouching ? maxCrouchSpeed : maxWalkSpeed;
		float acceleration = Time.fixedDeltaTime / accelerateTime * maxMovementSpeed;
		float newSpeed = currentSpeed + direction * acceleration;
		return Math.Clamp(newSpeed, -maxMovementSpeed, maxMovementSpeed);
	}

	private float GetDecelerated(float currentSpeed) {
		float maxMovementSpeed = _isCrouching ? maxCrouchSpeed : maxWalkSpeed;
		float deceleration = Time.fixedDeltaTime / accelerateTime * maxMovementSpeed;

		if (Mathf.Abs(currentSpeed) < deceleration) {
			return 0;
		}
		float newSpeed = currentSpeed - Mathf.Sign(currentSpeed) * deceleration;
		return Math.Clamp(newSpeed, -maxMovementSpeed, maxMovementSpeed);
	}

	/// <summary>
	/// Makes player crouch on key press. Makes player stand up on key release if not trapped below something
	/// </summary>
	private void CheckCrouching() {
		bool canStandUp = CanStandUp();
		
		if (!_isCrouching && (!canStandUp || _wantsCrouch)) {
			Crouch();
		}
		if (!_wantsCrouch && _isCrouching && canStandUp) {
			StandUp();
		}
	}
	
	/// <summary>
	/// Resizes capsule to fit crouching height and slows player down
	/// </summary>
	private void Crouch() {
		_isCrouching = true;
		tongue.SetExtendingEnabled(false);
		
		capsule.size = new Vector2(capsule.size.x, crouchHeight);
		capsule.offset = new Vector2(capsule.offset.x, _defaultCapsuleOffY - (_defaultHeight - crouchHeight) / 2);
		
		_rigid.velocity = new Vector2(
			Mathf.Clamp(_rigid.velocity.x, -maxCrouchSpeed, maxCrouchSpeed),
			_rigid.velocity.y);
	}

	/// <summary>
	/// Checks intersection of player capsule with solid objects 0.5 units above player
	/// </summary>
	/// <returns>true if nothing is blocking the player from standing up, otherwise false</returns>
	private bool CanStandUp() {
		Physics2D.queriesHitTriggers = false;
		bool canStandUp = !Physics2D.OverlapPoint(ceilingCheck.position, solidsLayerMask);
		Physics2D.queriesHitTriggers = true;
		return canStandUp;
	}

	private bool IsBeingCrushed() {
		//disable physics2d intersection checks with triggers
		Physics2D.queriesHitTriggers = false;
		bool isBeingCrushed = Physics2D.OverlapPoint(crushCheck.position, solidsLayerMask);
		Physics2D.queriesHitTriggers = true;
		return isBeingCrushed;
	}
	
	/// <summary>
	/// Resized capsule back to default size.
	/// </summary>
	private void StandUp() {
		_isCrouching = false;
		tongue.SetExtendingEnabled(true);
		capsule.size = new Vector2(capsule.size.x, _defaultHeight);
		capsule.offset = new Vector2(capsule.offset.x, _defaultCapsuleOffY);
	}
	
	/// <summary>
	/// Installs a joint onto player body that insures that maximum tongue length is not exceeded
	/// </summary>
	/// <param name="other"></param>
	public void OnTongueAttach(Collider2D other) {
		Rigidbody2D attachedObject = other.attachedRigidbody;
		_tongueConnection = this.AddComponent<DistanceJoint2D>();
		_tongueConnection.enableCollision = true;
		_tongueConnection.connectedBody = attachedObject;
		_tongueConnection.autoConfigureDistance = false;
		_tongueConnection.distance = tongue.GetMaxLength();
		_tongueConnection.maxDistanceOnly = true;
		_tongueConnection.anchor = tongue.gameObject.transform.parent.localPosition;
	}
	
	/// <summary>
	/// Removes tongue joint on tongue detach
	/// </summary>
	public void OnTongueDetach() {
		Destroy(_tongueConnection);
	}
}