using System;
using UnityEngine;

public class TongueMovement : MonoBehaviour {
	
	[SerializeField] private TongueControl tongue;
	[SerializeField] private LayerMask attachLayerMask;
	[SerializeField] private LayerMask collectLayerMask;

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

	private void Update() {
		//makes tongue tip touch attach point while being attached
		if (tongue.IsAttached()) {
			Vector2 attachPoint = tongue.GetAttachPoint();
			transform.right = GetAimDir(attachPoint);
			float distance = (attachPoint - (Vector2) transform.position).magnitude;
			tongue.SetExtendDistance(distance);
			
		//animates tongue extend on left click
		} else if (!tongue.IsExtending() && _controls.Player.TongueShoot.WasPerformedThisFrame()) {
			Vector2 mouseScreenPos = _controls.Player.MousePos.ReadValue<Vector2>();
			Vector2 mousePos = _cam.ScreenToWorldPoint(mouseScreenPos);
			transform.right = GetAimDir(mousePos);
			tongue.PlayExtend();
		}
	}

	private Vector2 GetAimDir(Vector2 aim) {
		return new Vector2(
			aim.x - transform.position.x,
			aim.y - transform.position.y);
	}

	public void OnTongueTrigger(Collider2D other) {
		if (!tongue.IsExtending() || IsBehindTongue(other.transform.position)) {
			return;
		}
		GameObject otherObject = other.gameObject;
		
		if (MaskContains(attachLayerMask, otherObject)) {
			tongue.AttachTo(other);
		} else if (MaskContains(collectLayerMask, otherObject)) {
			transform.right = GetAimDir(otherObject.transform.position);
			tongue.PickUp(otherObject);
		}
	}

	private bool MaskContains(LayerMask mask, GameObject other) {
		return (mask & 1 << other.layer) != 0;
	}

	private bool IsBehindTongue(Vector3 point) {
		return Vector2.Dot(transform.right, point - transform.position) < 0;
	}
}
