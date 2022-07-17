
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComicImage : ComicElement {

	[SerializeField] private float autoStayTime = 3f;
	
	[Header("Image")]
	[SerializeField] [Min(0)] private float fadeInTime = 0.5f;
	[SerializeField] [Min(0)] private float fadeOutTime = 0.5f;
	[SerializeField] private float clickCooldownTime = 1f;

	private Image _image; 
	private bool _isActive;
	private float _coolDown;
	
	public override void Activate(Action deactivateCallback) {
		base.Activate(deactivateCallback);
		_isActive = true;
		_coolDown = clickCooldownTime;
		_image = GetComponent<Image>();
		_image.color = fadeInTime > 0 ? new Color(1, 1, 1, 0) : Color.white;

		if (doAutoContinue) {
			StartCoroutine(DeactivateTimed());
		}
	}

	private void Update() {
		Color color = _image.color;

		if (IsActive()) {
			if (color.a < 1) {
				color.a = MathF.Min(1, color.a + (1 / fadeInTime) * Time.deltaTime);
				_image.color = color;
			}
			if (_coolDown > 0) {
				_coolDown -= Time.deltaTime;
			}
		} else {
			if (fadeOutTime > 0 && color.a > 0) {
				color.a = MathF.Max(0, color.a - (1 / fadeOutTime) * Time.deltaTime);
				_image.color = color;
			} else {
				gameObject.SetActive(false);
				DeactivateCallback?.Invoke();	
			}
		}
	}
	
	protected override void Interact() {
		if (_isActive && _coolDown <= 0) {
			_isActive = false;
		}
	}

	protected override bool IsSelfActive() {
		return _isActive;
	}

	public override void Deactivate() {
		_isActive = false;
	}
	
	private IEnumerator DeactivateTimed() {
		yield return new WaitForSeconds(autoStayTime);
		Deactivate();
		OnInteract();
	}
}