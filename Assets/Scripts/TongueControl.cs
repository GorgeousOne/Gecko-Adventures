using System;
using UnityEngine;
using UnityEngine.Events;

public class TongueControl : MonoBehaviour {

	[SerializeField] private TriggerEvent triggerAction;
	[SerializeField] private TriggerEvent attachAction;
	[SerializeField] private UnityEvent detachAction;
	[SerializeField] private float extendDuration;
	
	private SpriteRenderer _renderer;
	private Animator _animator;
	private Animation _extendAnimation;
	private Vector2 _defaultPos;
	
	private Collider2D _attachment;
	private float _extendStart;
	private float _extendDistance;
	
	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();
		_extendAnimation = GetComponent<Animation>();
		_defaultPos = transform.localPosition;
		CalculateExtendAnimation();
	}

	/**
	 * Adaptes tongue extend animation to tongue length and extend duration on start
	 */
	private void CalculateExtendAnimation() {
		Keyframe[] frames = {
			new(0, 0),
			new(.5f * extendDuration, GetMaxLength()),
			new(extendDuration, 0)
		};
		AnimationCurve curve = new AnimationCurve(frames);
		_extendAnimation.clip.ClearCurves();
		_extendAnimation.clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
		_animator.SetFloat("extendSpeed", extendDuration);
	}
	
	public float GetMaxLength() {
		//returns the width of the unscaled tongue sprite I think
		return _renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit;
	}

	public float GetLength() {
		return _extendDistance;
	}

	public bool IsExtending() {
		return Time.time - _extendStart < extendDuration;
	}

	public Vector2 GetAttachPoint() {
		return _attachment.bounds.center;
	}
	
	public bool IsAttached() {
		return _attachment != null;
	}

	public void SetExtendDistance(float distance) {
		_extendDistance = distance;
		transform.localPosition = _defaultPos + new Vector2(_extendDistance, 0);
	}

	public void PlayExtend() {
		_animator.Play("Extend");
		_extendStart = Time.time;
	}

	public void Attach(Collider2D other) {
		if (IsAttached()) {
			return;
		}
		_attachment = other;
		_animator.Play("Idle");
		_animator.enabled = false;
		attachAction.Invoke(other);
	}

	public void Detach() {
		_attachment = null;
		_animator.enabled = true;
		detachAction.Invoke();
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		triggerAction.Invoke(other);
	}
}

[Serializable]
public class TriggerEvent : UnityEvent<Collider2D> {}