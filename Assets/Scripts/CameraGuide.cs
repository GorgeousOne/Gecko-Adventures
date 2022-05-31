using System;
using UnityEngine;
using UnityEngine.Events;

public class CameraGuide : MonoBehaviour {

	[SerializeField] private Vector2 targetOffset;
	[SerializeField] public bool followX = true;
	[SerializeField] public bool followY;

	public GuideEnterEvent enterEvent;
	private BoxCollider2D _collider;
	
	private void Start() {
		_collider = GetComponent<BoxCollider2D>();
	}

	public Vector2 GetTargetPoint() {
		return _collider.bounds.center + (Vector3) targetOffset;
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			enterEvent.Invoke(this);
		}
	}
	
	private void OnDrawGizmos() {
		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(collider.transform.position + (Vector3) collider.offset, collider.size);
		Gizmos.DrawIcon(collider.bounds.center + (Vector3) targetOffset, "sv_icon_dot8_pix16_gizmo", true);
	}
}

[Serializable]
public class GuideEnterEvent : UnityEvent<CameraGuide> {}