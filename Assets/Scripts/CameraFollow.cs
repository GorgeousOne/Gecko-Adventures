using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField][Range(0, 1)] private float smoothSpeed = 1f;
	[SerializeField] private int ppu = 16;
	
	public Transform target;

	private void LateUpdate() {
		Vector3 lerpPos = Vector3.Lerp(transform.position, target.position, smoothSpeed);


		Vector3 pixelPos = (1f / ppu) * new Vector3(
			Mathf.Round(lerpPos.x * ppu),
			Mathf.Round(lerpPos.y * ppu),
			-10 * ppu);
		transform.position = pixelPos;
	}
}
