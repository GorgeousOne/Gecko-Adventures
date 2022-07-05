
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

	private new void OnEnable() {
		base.OnEnable();
		_image = GetComponent<Image>();
	}

	public override void Activate() {
		Debug.Log("activate " + gameObject.name);
		gameObject.SetActive(true);
		_image.color = fadeInTime > 0 ? new Color(1, 1, 1, 0) : Color.white;
		_isActive = true;
		_coolDown = clickCooldownTime;
	}
	
	public override void Deactivate() {
		gameObject.SetActive(false);
	}

	private void Update() {
		Color color = _image.color;

		if (color.a < 1) {
			color.a = MathF.Min(1, color.a + (1 / fadeInTime) * Time.deltaTime);
			_image.color = color;
		}
		if (_isActive && _coolDown > 0) {
			_coolDown -= Time.deltaTime;
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
}