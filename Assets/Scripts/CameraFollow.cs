using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField][Range(0, 1)] float smoothSpeed = 0.125f;

	public Transform target;

	private void LateUpdate() {
		Vector3 playerPos = new Vector3(target.position.x, target.position.y, -10);
		Vector3 lerpedPos = Vector3.Lerp(transform.position, playerPos, smoothSpeed);
		transform.position = lerpedPos;
	}
}
