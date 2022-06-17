using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

// [ExecuteAlways]
public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform target;
	[SerializeField] private Vector2 targetOffset;
	// [SerializeField][Range(0, 1)] private float smoothSpeed = 1f;
	[SerializeField] [Min(0)] private float snapTime = 1f;
	[SerializeField] private int ppu = 16;
	
	[SerializeField] private bool followX = true;
	[SerializeField] private bool followY;
	[SerializeField] private GameObject cameraGuides;

	private CameraGuide _guide;
	private float _snapStartTime;
	private Vector3 _snapStartPos;
	private bool _isFollowingPaused;

	void OnEnable() {
		if (target == null) {
			Debug.LogWarning("No target player is set for the CameraFollow script.");
		}
		if (cameraGuides == null) {
			Debug.LogWarning("No camera guides are set for the CameraFollow script.");
			return;
		}
		foreach(Transform guideTransform in cameraGuides.transform) {
			CameraGuide guide = guideTransform.gameObject.GetComponent<CameraGuide>();

			if (null != guide) {
				guide.enterEvent.AddListener(OnCameraGuideEnter);
			}
		}
	}

	private void LateUpdate() {
		// Vector3 lerpPos = Vector3.Lerp(transform.position, target.position + (Vector3) targetOffset, smoothSpeed);
		if (target == null || _isFollowingPaused) {
			return;
		}
		Vector3 targetPos = target.position;
		Vector3 currentPos = transform.position;
		Vector3 newGoalPos = new Vector3(0, 0, currentPos.z);
		
		if (_IsSnapping()) {
			Vector3 snapTarget = _guide.GetTargetPoint();
			newGoalPos.x = followX ? targetPos.x : snapTarget.x;
			newGoalPos.y = followY ? targetPos.y : snapTarget.y;
			
			float snapProgress = (Time.time - _snapStartTime) / snapTime;
			Vector3 finalPos = Vector3.Lerp(_snapStartPos, newGoalPos, MathUtil.SquareIn(snapProgress));
			// Vector3 finalPos = Vector3.Slerp(_snapStartPos, newGoalPos, snapProgress);
			// transform.position = GetPixelPoint(finalPos, ppu);
			transform.position = finalPos;

		} else {
			newGoalPos.x = followX ? targetPos.x : currentPos.x;
			newGoalPos.y = followY ? targetPos.y : currentPos.y;
			transform.position = newGoalPos;
		}
	}

	public void PauseFollow() {
		_isFollowingPaused = true;
	}

	public void UnpauseFollow() {
		_isFollowingPaused = false;
	}

	private bool _IsSnapping() {
		return _guide != null && Time.time - _snapStartTime < snapTime;
	}
	
	public void OnCameraGuideEnter(CameraGuide guide) {
		followX = guide.followX;
		followY = guide.followY;
		_guide = guide;
		_snapStartPos = transform.position;
		_snapStartTime = Time.time;
	}

	// private Vector3 GetPixelPoint(Vector3 point, int ppu) {
		// Vector3 pixelPoint = Vector3Int.RoundToInt(ppu * point);
		// return 1f / ppu * pixelPoint;
	// }
}
