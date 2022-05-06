using System;
using UnityEngine;

public class ArrowController : MonoBehaviour {
	
	private void OnTriggerExit2D(Collider2D other) {
		GetComponent<Collider2D>().isTrigger = false;
		Debug.Log("invincible off");
	}

	private void OnCollisionEnter2D(Collision2D other) {
		GetComponent<Rigidbody2D>().gravityScale = 3;
	}
}
