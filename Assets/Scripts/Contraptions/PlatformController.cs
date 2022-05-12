using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

	[SerializeField] private Vector2 offset = new(0, 5);
	[SerializeField] private float moveTime = 2;
	[SerializeField] private float delayTime = 2;

	private Vector2 _startPos;
	private bool _isReturning = true;

	void Start() {
		_startPos = transform.position;
		StartCoroutine(nameof(ToggleReturn));
	}

	void Toggle() {
		if (_isReturning) {
			transform.position = _startPos;
		} else {
			transform.position = _startPos + offset;
		}
	}

	IEnumerator ToggleReturn() {
		yield return new WaitForSeconds(moveTime + delayTime);
		_isReturning = !_isReturning;
		StartCoroutine("ToggleReturn");
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.CompareTag("Player")) {
			collider.transform.parent = transform;
			Toggle();
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.CompareTag("Player")) {
			collider.transform.parent = null;
			Toggle();
		}
	}

	private void OnDrawGizmos() {
		Vector2 position = _startPos != Vector2.zero ? _startPos : transform.position;
		Gizmos.DrawLine(position, position + offset);
	}
}
