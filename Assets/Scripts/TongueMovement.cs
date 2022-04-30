using UnityEngine;

public class TongueMovement : MonoBehaviour {
	
	[SerializeField] private TongueControl tongue;
	
	private PlayerControls _controls;
	// private Collider2D _
	private void Awake() {
		_controls = new PlayerControls();
	}
	
	private void OnEnable() {
		_controls.Enable();
	}

	private void OnDisable() {
		_controls.Disable();
	}

	private void Update() {
		//makes tongue tip touch attach point while being attached
		if (tongue.IstAttached()) {
			Vector2 attachPoint = tongue.GetAttachPoint();
			transform.right = GetAimDir(attachPoint);
			float distance = (attachPoint - (Vector2) transform.position).magnitude;
			tongue.SetExtendDistance(distance);
			
		//makes tongue follow cursor if not extending
		} else if (!tongue.IsExtending()) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(_controls.Player.MousePos.ReadValue<Vector2>());
			transform.right = GetAimDir(mousePos);
			
			//animates tongue extend on left click
			if (_controls.Player.TongueShoot.WasPerformedThisFrame()) {
				tongue.PlayExtend();
			}
		}
	}

	private Vector2 GetAimDir(Vector2 aim) {
		return new Vector2(
			aim.x - transform.position.x,
			aim.y - transform.position.y);
	}

	public void OnTongueTrigger(Collider2D other) {
		if (tongue.IsExtending()) {
			tongue.Attach(other);
		}
	}
}
