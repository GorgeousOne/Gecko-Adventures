using System;
using Unity.VisualScripting;
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
	private GameObject _pickup;
	
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
		} else if (_pickup) {
			Destroy(_pickup);
		}
	}

	//returns the width of the unscaled tongue sprite I think
	public float GetMaxLength() {
		return _length;
	}

	public float GetLength() {
		return _extendDistance;
	}

	public bool IsExtending() {
		return Time.time - _extendStart < extendTime;
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
		_extendStart = Time.time;
	}

	public void AttachTo(Collider2D other) {
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

	public void PickUp(GameObject pickup) {
		if (_pickup) {
			return;
		}
		_extendStart = Time.time - extendTime * GetRetractDistancePercent(pickup.transform.position);
		pickup.transform.localPosition = Vector3.zero;
		pickup.transform.parent = transform;
		_pickup = pickup;
	}

	private float GetRetractDistancePercent(Vector3 point) {
		float distance = Mathf.Clamp((transform.position - point).magnitude, 0, _length);
		return 0.5f + 0.5f * distance / _length;
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		triggerAction.Invoke(other);
	}
}

[Serializable]
public class TriggerEvent : UnityEvent<Collider2D> {}