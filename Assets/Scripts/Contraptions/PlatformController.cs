using UnityEngine;

public class PlatformController : MonoBehaviour {

	[SerializeField] private Vector2 targetOffset;
	[SerializeField] private float moveOffset;
	[SerializeField] private float moveTime = 2;
	[SerializeField] private float waitTime = 2;

	private Vector2 _startPos;
	private bool _isMovingForward = true;
	private float _moveStart;

	private void Start() {
		_startPos = transform.position;
		_moveStart = moveOffset;
		// StartCoroutine("SwitchDirectionOnTargetReach");
	}

	/**
	 * Interpolates platform position over time making it move between start and target
	 */
	private void Update() {
		if (PassedMovementTime()) {
			_isMovingForward = !_isMovingForward;
			_moveStart += moveTime + waitTime;
		}
		_UpdatePosition();
	}
	
	private void _UpdatePosition() {
		Vector2 startPos = _startPos;
		Vector2 targetPos = _startPos;

		if (_isMovingForward) {
			targetPos += targetOffset;
		} else {
			startPos += targetOffset;
		}
		float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
		transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);
	}

	private bool PassedMovementTime() {
		return Time.time - _moveStart > moveTime + waitTime;
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			collider.transform.parent = transform;
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			collider.transform.parent = null;
		}
	}

	private void OnDrawGizmos() {
		Vector2 position = _startPos != Vector2.zero ? _startPos : transform.position;
		Gizmos.DrawLine(position, position + targetOffset);
	}
}
