﻿using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private Transform playerTransform;
	[SerializeField] private LayerMask solidsLayerMask;
	[SerializeField] private TongueControl tongue;
	[SerializeField] private Animator spriteAnimator;

	[SerializeField] private float ropingSpeed = 10;
	[SerializeField] private float jumpHeight = 3.25f;
	[SerializeField] private float jumpPressedRememberDuration = 0.2f;
	[SerializeField] private float groundedRememberDuration = 0.2f;
	[SerializeField] private float maxHorizontalVelocity = 10;
	[SerializeField] private float inertiaTime = 0.2f;
	
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

	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.TongueExtend.performed += _ => _isExtendingTongue = true;
		_controls.Player.TongueExtend.canceled += _ => _isExtendingTongue = false;
		_controls.Player.TongueRetract.performed += _ => _isRetractingTongue = true;
		_controls.Player.TongueRetract.canceled += _ => _isRetractingTongue = false;
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
		bool isGrounded = CheckGrounding();
		CheckJumping();
		CheckTongueLengthChange();
		CheckHorizontalMovement(isGrounded);
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
		
		Vector2 capsuleOrigin = (Vector2) playerTransform.position + capsuleOffset + new Vector2(0, -0.01f);
		return Physics2D.OverlapCapsule(capsuleOrigin, capsuleSize, _capsule.direction, 0, solidsLayerMask);
	}
	
	private void CheckJumping() {
		if (_controls.Player.Jump.WasPerformedThisFrame()) {
			//detaches tongue when jumping
			if (tongue.IsAttached()) {
				tongue.Detach();
				ApplyJumpVelocity();
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
				newTongueLength += ropingSpeed * Time.deltaTime;
			} else if (_isRetractingTongue) {
				newTongueLength -= ropingSpeed * Time.deltaTime;
			}
			_tongueConnection.distance = Mathf.Clamp(newTongueLength, 1, tongue.GetMaxLength());
		}
	}
	
	private void CheckHorizontalMovement(bool isGrounded) {
		float horizontalVelocity = _rigid.velocity.x;
		float horizontalInput = _controls.Player.Move.ReadValue<float>();
		
		//starts slowing down when if vertical player input stops
		if (Mathf.Abs(horizontalInput) < 0.01f) {
			//only slows down if not dangling from tongue
			if (!tongue.IsAttached() || isGrounded) {
				horizontalVelocity = GetDecelerated(horizontalVelocity);
			}
			//accelerates continuously when moving towards one direction
		} else {
			//damps horizontal velocity when switching direction
			if (Mathf.Abs(horizontalVelocity) > 0.01f && Mathf.Sign(horizontalInput) != Mathf.Sign(horizontalVelocity)) {
				horizontalVelocity = 0;
			}
			else {
				horizontalVelocity = GetAccelerated(horizontalVelocity, Mathf.Sign(horizontalInput));
			}
		}
		if (Mathf.Abs(horizontalVelocity) > 0.01f && Mathf.Sign(horizontalVelocity) != (_isFacingRight ? -1 : 1)) {
			Flip();
		}
		_rigid.velocity = new Vector2(horizontalVelocity, _rigid.velocity.y);
		spriteAnimator.SetFloat("Speed", isGrounded ? Mathf.Abs(horizontalVelocity) : 0f);
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
	
	//uses shifted and stretched quadratic function to smooth velocity during acceleration
	private float GetAccelerated(float currentSpeed, float direction) {
		float acceleration = Time.deltaTime / inertiaTime * maxHorizontalVelocity;
		float newSpeed = currentSpeed + direction * acceleration;
		return Math.Clamp(newSpeed, -maxHorizontalVelocity, maxHorizontalVelocity);
	}
	
	private float GetDecelerated(float currentSpeed) {
		float deceleration = (Time.deltaTime / inertiaTime * maxHorizontalVelocity);

		if (Mathf.Abs(currentSpeed) < deceleration) {
			return 0;
		} 
		float newSpeed = currentSpeed - Mathf.Sign(currentSpeed) * deceleration;
		return Math.Clamp(newSpeed, -maxHorizontalVelocity, maxHorizontalVelocity);
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