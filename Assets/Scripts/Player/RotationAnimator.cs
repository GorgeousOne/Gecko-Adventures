
using System;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnimator : MonoBehaviour {
	
	[SerializeField] private TongueMovement2 tongue;
	[SerializeField] private SpriteRenderer headRenderer;
	[SerializeField] private SpriteRenderer bodyRenderer;
	[SerializeField] private List<Sprite> headRotations;
	[SerializeField] private List<Sprite> bodyRotations;

	private int _lastBodySpriteIndex = -1;
	private int _lastHeadSpriteIndex = -1;
	
	// Update is called once per frame
	void Update() {
		float tongueAngle = tongue.GetExtendAngle();

		// if (tongue.IsAttached()) {
			// _UpdateBodySprite(tongueAngle);
		// }
		if (tongue.IsExtending() || tongue.IsAttached()) {
			_UpdateHeadSprite(tongueAngle);
		} else {
			_UpdateHeadSprite(0f);
		}
	}

	private void _UpdateHeadSprite(float tongueAngle) {
		int newSpriteIndex = _GetHeadSprite(tongueAngle);

		if (newSpriteIndex != _lastHeadSpriteIndex) {
			_lastHeadSpriteIndex = newSpriteIndex;
			
			Debug.Log(newSpriteIndex);
			headRenderer.sprite = headRotations[_lastHeadSpriteIndex];
		}
	}

	private void _UpdateBodySprite(float tongueAngle) {
		int newSpriteIndex = _GetBodySprite(tongueAngle);

		if (newSpriteIndex != _lastBodySpriteIndex) {
			_lastBodySpriteIndex = newSpriteIndex;
			bodyRenderer.sprite = bodyRotations[_lastBodySpriteIndex];
		}
	}
	
	private int _GetHeadSprite(float tongueAngle) {
		float mirrorXAngle = Math.Abs(MathUtil.WrapToPi(tongueAngle + 90));
		return (int) Mathf.Round((headRotations.Count - 1) * mirrorXAngle / 180);
	}

	private int _GetBodySprite(float tongueAngle) {
		float mirrorXAngle = Math.Abs(MathUtil.WrapToPi(tongueAngle + 90));
		return (int) Mathf.Round((bodyRotations.Count - 1) * mirrorXAngle);
	}
}
