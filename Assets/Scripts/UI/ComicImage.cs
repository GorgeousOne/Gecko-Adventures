
using System;
using UnityEngine;
using UnityEngine.UI;

public class ComicImage : ComicElement {

	[SerializeField] [Min(0)] private float fadeInTime = 0.5f;
	// [SerializeField] [Min(0)] private float fadeOutTime = 0.5f;
	[SerializeField] private float clickCooldownTime = 1f;

	private Image _image; 
	private bool _isActive;
	private float _coolDown;

	private void OnEnable() {
		_image = GetComponent<Image>();
	}

	public override void Activate() {
		gameObject.SetActive(true);
		_image.color = fadeInTime > 0 ? new Color(0, 0, 0, 0) : Color.white;
		_isActive = true;
		_coolDown = clickCooldownTime;
	}
	
	private void Update() {
		Color color = _image.color;

		if (color.a < 1) {
			color.a = MathF.Min(1, color.a + (1 / fadeInTime) * Time.deltaTime);
		}
		if (_isActive && _coolDown > 0) {
			_coolDown -= Time.deltaTime;
		}
	}

	protected override void Interact() {
		if (_isActive && _coolDown <= 0) {
			_isActive = false;
			gameObject.SetActive(false);
		}
	}

	protected override bool IsSelfActive() {
		return _isActive;
	}
}