
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ComicScrollElement : ComicElement {

	[Header("Text")]
	[Tooltip("World units per second")]
	[SerializeField] [Min(0.1f)] private float scrollSpeed = 1;
	[SerializeField] private int ppu = 16;
	[SerializeField] private float clickCooldownTime = .5f;

	
	private bool _isScrolledThrough;
	private float _coolDown;

	public override void Activate(Action deactivateCallback) {
		base.Activate(deactivateCallback);
		_coolDown = clickCooldownTime;
	}
	
	private void Update() {
		if (_coolDown > 0) {
			_coolDown -= Time.deltaTime;
		}
		
		Rect screenRect = ((RectTransform) transform).rect;
		float maxY = transform.position.y + 0.5f * screenRect.height / ppu;
		float movement = Time.deltaTime * scrollSpeed;
		_isScrolledThrough = true;
		
		foreach (RectTransform child in transform) {
			
			child.transform.Translate(0, movement, 0);
			float childMinY = child.position.y - 0.5f * child.rect.height / ppu;

			if (childMinY < maxY) {
				_isScrolledThrough = false;
			}
		}
		if (_isScrolledThrough && doAutoContinue) {
			Deactivate();
		}
	}
	
	protected override void Interact() {
		if (_coolDown <= 0) {
			_isScrolledThrough = true;
		}
	}
	
	protected override bool IsSelfActive() {
		return _isScrolledThrough;
	}
	
	private void OnDrawGizmos() {
		RectTransform rectTransform = ((RectTransform) transform);
		float maxY = rectTransform.position.y + 0.5f * rectTransform.rect.height / ppu;
		
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(new Vector3(-10, maxY, 0), new Vector3(10, maxY, 0));
		Gizmos.color = new Color(1, .5f, 0);

		foreach (RectTransform child in transform) {
			float childY = child.position.y - 0.5f * child.rect.height / ppu;
			Gizmos.DrawLine(new Vector3(-10, childY, 0), new Vector3(10, childY, 0));
		}
	}
}
