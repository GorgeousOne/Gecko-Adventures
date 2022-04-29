using UnityEngine;

public class DoorController : MonoBehaviour {

	[SerializeField] private Vector3 closedPosition;
	[SerializeField] private Vector3 openPosition;
	[SerializeField] private float movingSpeed;

	bool _isOpen;

	// Update is called once per frame
	void Update() {
		Vector3 currentPos = transform.position;
		Vector3 targetPos = _isOpen ? openPosition : closedPosition;
		transform.position = Vector3.Lerp(currentPos, targetPos, movingSpeed);
	}

	public void OnSwitchToggle(bool isEnabled) {
		_isOpen = isEnabled;
	}

	private void OnDrawGizmos() {
		Gizmos.DrawIcon(closedPosition, "sv_icon_dot15_pix16_gizmo.png", false);
		Gizmos.DrawIcon(openPosition, "sv_icon_dot13_pix16_gizmo.png", false);
	}
}
