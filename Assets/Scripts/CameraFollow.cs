using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

// [ExecuteAlways]
public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform target;
	[SerializeField] [Min(0)] private float snapTime = 1f;
	
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
		_snapStartTime = -snapTime;
	}

	private void LateUpdate() {
		if (target == null || _isFollowingPaused) {
			return;
		}
		Vector3 newGoalPos = GetSnapPoint();
		
		//interpolate between current position and guide target point
		if (_IsSnapping()) {
			float snapProgress = (LevelTime.time - _snapStartTime) / snapTime;
			Vector3 finalPos = Vector3.Lerp(_snapStartPos, newGoalPos, MathUtil.SquareIn(snapProgress));
			transform.position = finalPos;
		//move camera along target
		} else {
			transform.position = newGoalPos;
		}
	}

	private Vector2 GetSnapPoint() {
		Vector3 snapPos = _guide != null ? _guide.GetTargetPoint() : transform.position;
		Vector3 targetPos = target.position;
		return new Vector2(
				followX ? targetPos.x : snapPos.x,
				followY ? targetPos.y : snapPos.y);
	}
	
	public void PauseFollow() {
		_isFollowingPaused = true;
	}

	public void UnpauseFollow() {
		// delays camera following so camera guides touched during respawn have time to trigger events
		StartCoroutine(DelayUnpause());
	}

	private IEnumerator DelayUnpause() {
		yield return new WaitForSeconds(.1f);
		_isFollowingPaused = false;
	}

	private bool _IsSnapping() {
		return _guide != null && LevelTime.time - _snapStartTime < snapTime;
	}
	
	public void OnCameraGuideEnter(CameraGuide guide) {
		followX = guide.followX;
		followY = guide.followY;
		_guide = guide;
		
		//instantly move to guide target on level starts
		if (LevelTime.time > snapTime) {
			_snapStartPos = transform.position;
			_snapStartTime = LevelTime.time;
		} else {
			transform.position = GetSnapPoint();
		}
	}
}
