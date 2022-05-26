using UnityEngine;

public class CameraGuide : MonoBehaviour {

	[SerializeField] public bool followX = true;
	[SerializeField] public bool followY;
	private CameraFollow _camFollow;

	private void Start() {
		_camFollow = Camera.main.GetComponent<CameraFollow>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		_camFollow.OnCameraGuideEnter(this);
	}

	// public Vector3 GetGuidedPosition(Vector3 camPos) {
		
	// }
	
	private void OnDrawGizmos() {
		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		
		Gizmos.color = Color.white;
		Gizmos.DrawCube(collider.transform.position + (Vector3) collider.offset, collider.size);
	}
}