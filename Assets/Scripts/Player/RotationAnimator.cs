
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RotationAnimator : MonoBehaviour {
	
	[SerializeField] private TongueMovement2 tongue;
	[SerializeField] private PlayerMovement2 player;
	
	[SerializeField] private SpriteRenderer headRenderer;
	[SerializeField] private SpriteRenderer bodyRenderer;
	[SerializeField] private Animator bodyAnimator;
	[SerializeField] private List<Sprite> headRotations;
	[SerializeField] private List<Sprite> bodyRotations;

	private int _lastBodySpriteIndex = -1;
	private int _lastHeadSpriteIndex = -1;
	private bool _isHeadFacingRight = true;
	private bool _isBodyFacingRight = true;
	private bool _isSwinging;
	
	private Rigidbody2D _rigid;

	private void Start() {
		_rigid = GetComponent<Rigidbody2D>();
	}

	void Update() {
		float xMirroredAngle = MathUtil.WrapToPi(tongue.GetExtendAngle() + 90);
		
		_UpdateBodySprite(xMirroredAngle);
		if (tongue.IsExtending() || tongue.IsAttached()) {
			_UpdateHeadSprite(xMirroredAngle);
		} else {
			_ResetHeadSprite();
		}
	}
	
	private void _UpdateHeadSprite(float xMirroredAngle) {
		int newSpriteIndex = _GetHeadSpriteIndex(xMirroredAngle);
		bool isTongueFacingRight = xMirroredAngle > 0;

		if (_isHeadFacingRight != isTongueFacingRight) {
			_FlipHead();
		}
		if (newSpriteIndex != _lastHeadSpriteIndex) {
			_lastHeadSpriteIndex = newSpriteIndex;
			headRenderer.sprite = headRotations[_lastHeadSpriteIndex];
		}
	}

	private void _ResetHeadSprite() {
		headRenderer.sprite = headRotations[headRotations.Count / 2];

		if (_isHeadFacingRight != _isBodyFacingRight) {
			_FlipHead();
		}
	}

	private void _UpdateBodySprite(float xMirroredAngle) {
		_ToggleSwingAnimation(xMirroredAngle);

		if (_isSwinging) {
			int newSpriteIndex = _GetBodySpriteIndex(xMirroredAngle);
		
			if (newSpriteIndex != _lastBodySpriteIndex) {
				_lastBodySpriteIndex = newSpriteIndex;
				bodyRenderer.sprite = bodyRotations[_lastBodySpriteIndex];
			}
		}
		bool hasTurnedAround = Mathf.Sign(_rigid.velocity.x) != (_isBodyFacingRight ? 1 : -1);

		if (!MathUtil.IsZero(_rigid.velocity.x) && hasTurnedAround) {
			_FlipBody();
		}
	}

	private void _ToggleSwingAnimation(float xMirroredAngle) {
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
	
	private int _GetHeadSpriteIndex(float xMirroredAngle) {
		return (int) Mathf.Round((headRotations.Count - 1) * Mathf.Abs(xMirroredAngle) / 180);
	}

	private int _GetBodySpriteIndex(float xMirroredAngle) {
		return (int) Mathf.Round((bodyRotations.Count - 1) * Mathf.Abs(xMirroredAngle) / 180);
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
