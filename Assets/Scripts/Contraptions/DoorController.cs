using UnityEngine;

public class DoorController : Switchable {

	[SerializeField] private Vector2 openOffset;
	[SerializeField] private float moveTime;

	private Vector2 _startPos;
	private bool _isOpen;

	private float _moveStart;
	
	private void Start() {
		_startPos = transform.position;
	}

	// Update is called once per frame
	void Update() {
		Vector2 startPos = _startPos;
		Vector2 targetPos = _startPos;

		if (_isOpen) {
			targetPos += openOffset;
		}
		else {
			startPos += openOffset;
		}
		float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
		transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);
	}

	public override void OnSwitchToggle(bool isEnabled) {
		_isOpen = isEnabled;
		float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
		_moveStart = Time.time - (1 - elapsedTime);
	}

	private void OnDrawGizmos() {
		Vector2 position = _startPos != Vector2.zero ? _startPos : transform.position;
		Gizmos.DrawIcon(position, "sv_icon_dot13_pix16_gizmo.png", true);
		Gizmos.DrawIcon(position + openOffset, "sv_icon_dot15_pix16_gizmo.png", true);
	}
}
