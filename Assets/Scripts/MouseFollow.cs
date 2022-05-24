using UnityEngine;

public class MouseFollow : MonoBehaviour {
	
	private PlayerControls _controls;
	private Camera _cam;
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Enable();
	}

	private void OnDisable() {
		_controls.Disable();
	}

	private void Start() {
		_cam = Camera.main;
	}

	private void LateUpdate() {
		Vector2 mouseScreenPos = _controls.Player.MousePos.ReadValue<Vector2>();
		Vector2 mousePos = _cam.ScreenToWorldPoint(mouseScreenPos);
		transform.position = mousePos;
	}
}
