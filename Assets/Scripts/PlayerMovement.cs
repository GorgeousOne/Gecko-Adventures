using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	[SerializeField] private LayerMask solidsLayerMask;
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
	
	private void Awake() {
		_controls = new PlayerControls();
	}

	private void Start() {
		_rigid = GetComponent<Rigidbody2D>();
		_capsule = GetComponent<CapsuleCollider2D>();
	}
	
	private void OnEnable() {
		_controls.Enable();
	}

	private void OnDisable() {
		_controls.Disable();
	}

	private void Update() {
		_groundedRemember -= Time.deltaTime;
		_jumpPressedRemember -= Time.deltaTime;
		
		if (IsGrounded()) {
			_groundedRemember = groundedRememberDuration;
		}
		if (_controls.Player.Jump.WasPerformedThisFrame()) {
			_jumpPressedRemember = jumpPressedRememberDuration;
		}

		// Debug.Log((_jumpPressedRemember > 0) + ", " + (_groundedRemember > 0));
		if (_jumpPressedRemember > 0 && _groundedRemember > 0) {
			_jumpPressedRemember = 0;
			_groundedRemember = 0;
			_rigid.velocity = new Vector2(_rigid.velocity.x, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpHeight));
		}
		float horizontalVelocity = _rigid.velocity.x;
		float horizontalInput = _controls.Player.Move.ReadValue<float>();
			
		//starts slowing down when if vertical player input stops
		if (Mathf.Abs(horizontalInput) < 0.01f) {
			if (_wasRunning) {
				_stopRunVelocity = horizontalVelocity;
				_stopRunTime = Time.time;
				_wasRunning = false;
			}
			float deceleratePercent = (Time.time - _stopRunTime) / decelerateDuration;
			horizontalVelocity = Mathf.SmoothStep(_stopRunVelocity, 0, deceleratePercent);
			_startRunTime = Time.time;

		//accelerates continuously when moving towards one direction
		} else {
			
			//damps horizontal velocity when switching direction
			if (Mathf.Abs(horizontalVelocity) > 0.01f && 
			    Mathf.Sign(horizontalInput) != Mathf.Sign(horizontalVelocity)) {
				_wasRunning = false;
			}
			if (!_wasRunning) {
				_startRunTime = Time.time;
				_wasRunning = true;
			}
			float acceleratePercent = (Time.time - _startRunTime) / accelerateDuration;
			horizontalVelocity = Math.Sign(horizontalInput) * Mathf.SmoothStep(0, maxHorizontalVelocity, acceleratePercent);
		}
		if (Mathf.Abs(horizontalVelocity) > 0.01f && 
		    Mathf.Sign(horizontalVelocity) != (_isFacingRight ? -1 : 1)) {
			Flip();
		}
		_rigid.velocity = new Vector2(horizontalVelocity, _rigid.velocity.y);
	}

	/**
	 * Creates a capsule close below the players capsule and checks if it intersects with any ground
	 */
	private bool IsGrounded() {
		Vector2 localScale = transform.localScale;
		Vector2 capsuleSize = _capsule.size;
		Vector2 capsuleOffset = _capsule.offset;
		
		capsuleSize.Scale(localScale);
		capsuleOffset.Scale(localScale);
		
		Vector2 capsuleOrigin = (Vector2) transform.position + capsuleOffset + new Vector2(0, -0.01f);
		return Physics2D.OverlapCapsule(capsuleOrigin, capsuleSize, _capsule.direction, 0, solidsLayerMask);
	}
	
	private void Flip() {
		// Switches the way the player is labelled as facing.
		_isFacingRight = !_isFacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}