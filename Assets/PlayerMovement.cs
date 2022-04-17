using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool isJumping = false;

	void Update() {
		horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump")) {
			isJumping = true;
		}
	}

	void FixedUpdate() {
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, isJumping);
		isJumping = false;
	}
}
