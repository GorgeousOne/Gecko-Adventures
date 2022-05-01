using System;
using UnityEngine;
using UnityEngine.Events;

public class TongueControl : MonoBehaviour {

	[SerializeField] private TriggerEvent triggerAction;
	[SerializeField] private TriggerEvent attachAction;
	[SerializeField] private UnityEvent detachAction;
	[SerializeField] private float extendTime;
	
	private SpriteRenderer _renderer;
	private Vector2 _defaultPos;
	
	private Collider2D _attachment;
	private float _extendStart;
	private float _extendDistance;
	private float _length;
	
	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		_defaultPos = transform.localPosition;
		_length = _renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit;
		_extendStart = -extendTime;
	}

	private void Update() {
		if (IsExtending()) {
			float elapsedPercent = (Time.time - _extendStart) / extendTime;

			if (elapsedPercent < .5f) {
				SetExtendDistance(Mathf.SmoothStep(0, _length, 2 * elapsedPercent));
			} else {
				SetExtendDistance(Mathf.SmoothStep(_length, 0, 2 * (elapsedPercent - .5f)));
			}
		}
	}

	public float GetMaxLength() {
		//returns the width of the unscaled tongue sprite I think
		return _length;
	}

	public float GetLength() {
		return _extendDistance;
	}

	public bool IsExtending() {
		return Time.time - _extendStart <= extendTime;
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
		// _animator.Play("Extend");
		_extendStart = Time.time;
	}

	public void Attach(Collider2D other) {
		if (IsAttached()) {
			return;
		}
		_extendStart = Time.time - extendTime;
		_attachment = other;
		attachAction.Invoke(other);
	}

	public void Detach() {
		_attachment = null;
		SetExtendDistance(0);
		detachAction.Invoke();
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		triggerAction.Invoke(other);
	}
}

[Serializable]
public class TriggerEvent : UnityEvent<Collider2D> {}