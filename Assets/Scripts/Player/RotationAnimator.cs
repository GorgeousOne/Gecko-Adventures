
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RotationAnimator : MonoBehaviour {
	
	[SerializeField] private TongueMovement2 tongue;
	[SerializeField] private PlayerMovement2 player;
	
	[SerializeField] private SpriteRenderer headRenderer;
	[SerializeField] private SpriteRenderer bodyRenderer;
	[SerializeField] private SpriteMask headHideMask;
	[SerializeField] private Animator bodyAnimator;
	[SerializeField] private List<Sprite> headRotations;
	[SerializeField] private List<Sprite> bodyRotations;

	private int _lastBodySpriteIndex = -1;
	private int _lastHeadSpriteIndex = -1;
	private bool _isHeadFacingRight = true;
	private bool _isBodyFacingRight = true;
	private bool _isSwinging;
	
	private bool _isUsingTongue;
	
	private Rigidbody2D _rigid;

	private void Start() {
		_rigid = GetComponent<Rigidbody2D>();
	}

	void Update() {
		float tongueAngle = tongue.GetExtendAngle();
		_UpdateBodySprite(tongueAngle);
		
		if (tongue.IsExtending() || tongue.IsAttached()) {
			_UpdateHeadSprite(tongueAngle);
		} else {
			_ResetHeadSprite();
		}
	}
	
	private void _UpdateHeadSprite(float tongueAngle) {
		headRenderer.enabled = true;
		headHideMask.enabled = !tongue.IsAttached();

		float xMirroredTongueAngle = MathUtil.WrapToPi(tongueAngle + 90);
		int newSpriteIndex = _GetHeadSpriteIndex(xMirroredTongueAngle);
		bool isTongueFacingRight = xMirroredTongueAngle > 0;

		if (_isHeadFacingRight != isTongueFacingRight) {
			_FlipHead();
		}
		if (newSpriteIndex != _lastHeadSpriteIndex) {
			_lastHeadSpriteIndex = newSpriteIndex;
			headRenderer.sprite = headRotations[_lastHeadSpriteIndex];
		}
	}
	
	/// <summary>
	/// Resets the head sprite to closed mouth
	/// </summary>
	private void _ResetHeadSprite() {
		headRenderer.enabled = false;
		headHideMask.enabled = false;
	}

	/// <summary>
	/// Selects correct rotation sprite for body when swinging. Otherwise enables sprite animator.
	/// </summary>
	/// <param name="tongueAngle"></param>
	private void _UpdateBodySprite(float tongueAngle) {
		bool hasTurnedAround = Mathf.Sign(_rigid.velocity.x) != (_isBodyFacingRight ? 1 : -1);
		bool isOverSwinging = tongue.IsAttached() && tongueAngle < 0;
		
		if (!MathUtil.IsZero(_rigid.velocity.x) && hasTurnedAround && !isOverSwinging) {
			_FlipBody();
		}
		_ToggleSwingAnimation();
		
		if (_isSwinging) {
			int newSpriteIndex = _GetBodySpriteIndex(tongueAngle);
		
			if (newSpriteIndex != _lastBodySpriteIndex) {
				_lastBodySpriteIndex = newSpriteIndex;
				bodyRenderer.sprite = bodyRotations[_lastBodySpriteIndex];
			}
		}
	}
	
	/// <summary>
	/// Disables body sprite animator when swinging
	/// </summary>
	private void _ToggleSwingAnimation() {
		if (bodyAnimator.enabled) {
			if (tongue.IsAttached() && !player.CheckGrounding()) {
				bodyAnimator.enabled = false;
				_isSwinging = true;
			}
		} else if (!tongue.IsAttached() || player.CheckGrounding()) {
			bodyAnimator.enabled = true;
			_isSwinging = false;
		}
	}
	
	/// <summary>
	/// Determines rotated head sprite based on the tongue angle.
	/// </summary>
	/// <param name="xMirroredAngle">tongue angle between -180 and 180 (0 is facing down, +-180 is facing up, 90 is facing right)</param>
	/// <returns></returns>
	private int _GetHeadSpriteIndex(float xMirroredAngle) {
		return (int) Mathf.Round((headRotations.Count - 1) * Mathf.Abs(xMirroredAngle) / 180f);
	}
	
	/// <summary>
	/// Determines rotated body sprite based on swinging angle
	/// </summary>
	/// <param name="tongueAngle"> angle of tongue (0 facing right, +-180 facing left, 90 facing up)</param>
	/// <returns></returns>
	private int _GetBodySpriteIndex(float tongueAngle) {
		//returns maximum rotated sprite index if player is above y coordinate of attached object
		if (tongueAngle < 0) {
			return bodyRotations.Count - 1;
		}
		if (!_isBodyFacingRight) {
			tongueAngle = 180 - tongueAngle;
		}
		return (int) Mathf.Round((bodyRotations.Count - 1) * Mathf.Abs(tongueAngle) / 180f);
	}
	
	private void _FlipHead() {
		headRenderer.transform.localScale = _InvertX(headRenderer.transform.localScale);
		_isHeadFacingRight = !_isHeadFacingRight;
	}
	
	private void _FlipBody() {
		bodyRenderer.transform.localScale = _InvertX(bodyRenderer.transform.localScale);
		_isBodyFacingRight = !_isBodyFacingRight;
	}

	private Vector3 _InvertX(Vector3 v) {
		return new Vector3(-v.x, v.y, v.z);
	}
}
