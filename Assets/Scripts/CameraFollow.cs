using UnityEngine;

// [ExecuteAlways]
public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform target;
	[SerializeField] private Vector2 targetOffset;
	[SerializeField][Range(0, 1)] private float smoothSpeed = 1f;
	[SerializeField] private int ppu = 16;

	private void LateUpdate() {
		Vector3 lerpPos = Vector3.Lerp(transform.position, target.position + (Vector3) targetOffset, smoothSpeed);
		Vector3 pixelPos = new Vector3(
			(1f / ppu) * Mathf.Round(lerpPos.x * ppu),
			transform.position.y,// Mathf.Round(lerpPos.y * ppu),
			-10);
		transform.position = pixelPos;
	}
}
