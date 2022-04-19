using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3 : MonoBehaviour {
	[SerializeField] LayerMask mSolidsLayerMask;
	[SerializeField] float fJumpHeight = 10;
	[SerializeField] float fJumpPressedRememberDuration = 0.2f;
	[SerializeField] float fGroundedRememberDuration = 0.2f;
	[SerializeField] float fMaxHorizontalVelocity = 10;
	[SerializeField] float fAccelerateDuration = 0.2f;
	[SerializeField] float fDecelerateDuration = 0.2f;

	float fJumpPressedRemember;
	float fGroundedRemember;
	float fStartRunTime;
	float fStopRunTime;
	float fStopRunVelocity;
	private bool wasRunning = false;

	Rigidbody2D rigid;
	CapsuleCollider2D capsule;

	void Start() {
		rigid = GetComponent<Rigidbody2D>();
		capsule = GetComponent<CapsuleCollider2D>();
	}

	void Update() {
		// Vector2 v2GroundedBoxCheckPosition = (Vector2) transform.position + new Vector2(0, -0.01f);
		// Vector2 v2GroundedBoxCheckScale = (Vector2) transform.localScale + new Vector2(-0.02f, 0);
		// bool bGrounded2 = Physics2D.OverlapBox(v2GroundedBoxCheckPosition, v2GroundedBoxCheckScale, 0, mSolidsLayerMask);
		
		fGroundedRemember -= Time.deltaTime;
		
		if (IsGrounded()) {
			fGroundedRemember = fGroundedRememberDuration;
		}
		fJumpPressedRemember -= Time.deltaTime;
		
		if (Input.GetButtonDown("Jump")) {
			fJumpPressedRemember = fJumpPressedRememberDuration;
		}
		if (fJumpPressedRemember > 0 && fGroundedRemember > 0) {
			fJumpPressedRemember = 0;
			fGroundedRemember = 0;
			rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * fJumpHeight));
		}
		float fHorizontalVelocity = rigid.velocity.x;
		float fHorizontalInput = Input.GetAxisRaw("Horizontal");
			
		//starts slowing down when if vertical player input stops
		if (Mathf.Abs(fHorizontalInput) < 0.01f) {
			if (wasRunning) {
				fStopRunVelocity = fHorizontalVelocity;
				fStopRunTime = Time.time;
				wasRunning = false;
			}
			float fDeceleratePercent = (Time.time - fStopRunTime) / fDecelerateDuration;
			fHorizontalVelocity = Mathf.SmoothStep(fStopRunVelocity, 0, fDeceleratePercent);
			fStartRunTime = Time.time;

		//accelerates continuously when moving towards one direction
		} else {
			
			//damps horizontal velocity when switching direction
			if (Mathf.Abs(fHorizontalVelocity) > 0.01f && 
			    Mathf.Sign(fHorizontalInput) != Mathf.Sign(fHorizontalVelocity)) {
				wasRunning = false;
			}
			if (!wasRunning) {
				fStartRunTime = Time.time;
				wasRunning = true;
			}
			float fAcceleratePercent = (Time.time - fStartRunTime) / fAccelerateDuration;
			fHorizontalVelocity = Math.Sign(fHorizontalInput) * Mathf.SmoothStep(0, fMaxHorizontalVelocity, fAcceleratePercent);
		}
		rigid.velocity = new Vector2(fHorizontalVelocity, rigid.velocity.y);
	}

	bool IsGrounded() {
		Vector2 localScale = transform.localScale;
		Vector2 capsuleSize = capsule.size;
		Vector2 capsuleOffset = capsule.offset;
		capsuleSize.Scale(localScale);
		capsuleOffset.Scale(localScale);
		Vector2 capsuleOrigin = (Vector2) transform.position + capsuleOffset + new Vector2(0, -0.1f);
		return Physics2D.OverlapCapsule(capsuleOrigin, capsuleSize, capsule.direction, 0, mSolidsLayerMask);
	}
}