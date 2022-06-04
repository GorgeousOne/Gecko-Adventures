using UnityEngine;

public class PlatformTriggerableController : Triggerable {

	[SerializeField] private Vector2 targetOffset;
	[SerializeField] private float moveOffset;
	[SerializeField] private float moveTime = 2;
	[SerializeField] private float waitTime = 2;

	private Vector2 _startPos;
	private bool _isMovingForward = true;
	private float _moveStart;

	private bool _savedWasMovingForward;
	private float _savedMoveStart;
	
	private bool _isOpen;

	private void Start() {
		_startPos = transform.position;
		_moveStart = moveOffset;
		SaveState();
	}

	/**
	 * Interpolates platform position over time making it move between start and target triggered by a switch
	 */
	private void Update() {
		if (_isOpen) {
			if (PassedMovementTime()) {
				_isMovingForward = !_isMovingForward;
				_moveStart += moveTime + waitTime;
			}
			_UpdatePosition();
		}
	}
	
	private void _UpdatePosition() {
		Vector2 startPos = _startPos;
		Vector2 targetPos = _startPos;

		if (_isMovingForward) {
			targetPos += targetOffset;
		} else {
			startPos += targetOffset;
		}
		float elapsedTime = Mathf.Clamp(LevelTime.time - _moveStart, 0, moveTime);
		transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);
	}

	public override void OnSwitchToggle(bool isEnabled) {
		_isOpen = isEnabled;
		float elapsedTime = Mathf.Clamp(LevelTime.time - _moveStart, 0, moveTime);
		_moveStart = LevelTime.time - (1 - elapsedTime);
	}

	private bool PassedMovementTime() {
		return LevelTime.time - _moveStart > moveTime + waitTime;
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
	
	public override void SaveState() {
		_savedWasMovingForward = _isMovingForward;
		_savedMoveStart = _moveStart;
	}

	public override void ResetState() {
		_isMovingForward = _savedWasMovingForward;
		_moveStart = _savedMoveStart;
	}
}
