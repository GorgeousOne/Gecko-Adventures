using System.Collections.Generic;
using UnityEngine;

// [ExecuteAlways]
public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform target;
	[SerializeField] private Vector2 targetOffset;
	[SerializeField][Range(0, 1)] private float smoothSpeed = 1f;
	[SerializeField] private int ppu = 16;
	
	[SerializeField] private bool followX = true;
	[SerializeField] private bool followY;
	[SerializeField] private GameObject cameraGuides;
	
	// private List<CameraGuide> _guides;
	
	void Awake() {
		// _guides = new List<CameraGuide>();
		
		foreach(Transform guideTransform in cameraGuides.transform) {
			CameraGuide guide = guideTransform.gameObject.GetComponent<CameraGuide>();

			if (null != guide) {
				guide.enterEvent.AddListener(OnCameraGuideEnter);
				// _guides.Add(guide);
			}
		}
	}
	private void LateUpdate() {
		Vector3 lerpPos = Vector3.Lerp(transform.position, target.position + (Vector3) targetOffset, smoothSpeed);
		Vector3 newPos = transform.position;

		if (followX) {
			newPos.x = lerpPos.x;
		}
		if (followY) {
			newPos.y = lerpPos.y;
		}
		
		transform.position = GetPixelPoint(newPos, ppu);
	}

	public void OnCameraGuideEnter(CameraGuide guide) {
		followX = guide.followX;
		followY = guide.followY;
		Vector2 guideTarget = guide.GetTargetPoint();
		transform.position = new Vector3(guideTarget.x, guideTarget.y, transform.position.z);
	}

	private Vector3 GetPixelPoint(Vector3 point, int ppu) {
		Vector3 pixelPoint = Vector3Int.RoundToInt(ppu * point);
		return 1f / ppu * pixelPoint;
	}
}
