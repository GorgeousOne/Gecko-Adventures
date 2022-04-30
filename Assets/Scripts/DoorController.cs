using System;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour {

	[SerializeField] private Vector2 openOffset;
	[SerializeField] private float movingSpeed;

	private Vector2 _startPos;
	private bool _isOpen;
	
	private void Start() {
		_startPos = transform.position;
	}

	// Update is called once per frame
	void Update() {
		Vector2 targetPos = _startPos;

		if (_isOpen) {
			targetPos += openOffset;
		}
		transform.position = Vector2.Lerp(transform.position, targetPos, movingSpeed);
	}

	public void OnSwitchToggle(bool isEnabled) {
		_isOpen = isEnabled;
	}

	private void OnDrawGizmos() {
		Vector2 position = _startPos != Vector2.zero ? _startPos : transform.position;
		Gizmos.DrawIcon(position, "sv_icon_dot13_pix16_gizmo.png", true);
		Gizmos.DrawIcon(position + openOffset, "sv_icon_dot15_pix16_gizmo.png", true);
	}
}
