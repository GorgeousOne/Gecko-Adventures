using UnityEngine;

public class PlatformController : Triggerable {

	[SerializeField] private Vector2 targetOffset;
	[SerializeField] private float moveOffset;
	[SerializeField] private float moveTime = 2;
	[SerializeField] private float waitTime = 2;
	[SerializeField] private bool isEnabled = true;

	private Vector2 _startPos;
	private bool _isMovingForward = true;
	private float _moveStart;

	private Vector2 _savedPos;
	private bool _savedWasMovingForward;
	private float _savedMoveStart;
	private bool _saveWasEnabled;
	
	private void Start() {
		_startPos = transform.position;
		_moveStart = moveOffset;
		SaveState();
	}

	/**
	 * Interpolates platform position over time making it move between start and target
	 */
	private void Update() {
		if (isEnabled) {
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

	private float _GetMoveProgress() {
		Vector2 startPos = _startPos;
		Vector2 targetPos = _startPos;

		if (_isMovingForward) {
			targetPos += targetOffset;
		} else {
			startPos += targetOffset;
		}
		float moveDistance = ((Vector2)(transform.position) - startPos).magnitude;
		float totalDistance = (targetPos - startPos).magnitude;
		return moveDistance / totalDistance;
	}

	public override void OnSwitchToggle(bool isEnabled) {
		this.isEnabled = isEnabled;

		if (isEnabled) {
			float moveProgress = _GetMoveProgress();
			_moveStart = LevelTime.time - moveProgress * moveTime;
		}
	}

	private bool PassedMovementTime() {
		return LevelTime.time - _moveStart > moveTime + waitTime;
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("Player")) {
			collider.transform.parent.parent = transform;
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.CompareTag("Player")) {
			collider.transform.parent.parent = null;
		}
	}

	private void OnDrawGizmos() {
		Vector2 position = _startPos != Vector2.zero ? _startPos : transform.position;
		Gizmos.DrawLine(position, position + targetOffset);
	}
	
	public override void SaveState() {
		_savedPos = transform.position;
		_savedWasMovingForward = _isMovingForward;
		_savedMoveStart = _moveStart;
		_saveWasEnabled = isEnabled;
	}

	public override void ResetState() {
		transform.position = _savedPos;
		_isMovingForward = _savedWasMovingForward;
		_moveStart = _savedMoveStart;
		isEnabled = _saveWasEnabled;
	}
}
