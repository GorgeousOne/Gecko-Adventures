
using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// A comic element that types a text on the screen char by char over time
/// </summary>

public class ComicText : ComicElement {

	[SerializeField] private float autoTextStayTime = 3;
	
	[Header("Text")]
	[SerializeField] private float clickCooldownTime = .2f;
	[SerializeField] private float timePerChar = 0.02f;

	private TMP_Text uiText;
	private string textToWrite;
	private int charIndex;
	private float timer;
	
	private bool _isActive;
	private float _coolDown;

	protected new void OnEnable() {
		base.OnEnable();
		uiText = GetComponent<TMP_Text>();
		textToWrite = uiText.text;
		uiText.text = null;
	}

	public override void Activate(Action deactivateCallback) {
		base.Activate(deactivateCallback);
		charIndex = 0;
		_isActive = true;
		_coolDown = clickCooldownTime;

		if (doAutoContinue) {
			StartCoroutine(DeactivateTimed());
		}
	}
	
	private void Update() { 
		if (_coolDown > 0) {
			_coolDown -= Time.deltaTime;
		}
		if (IsTextComplete()) {
			return;
		}
		timer += Time.deltaTime;
			
		while (!IsTextComplete() && timer >= 0f) {
			timer -= timePerChar;
			++charIndex;
			string text = textToWrite[..charIndex] + "<color=#00000000>" + textToWrite[charIndex..] + "</color>";
			uiText.text = text;
		}
	}

	protected override void Interact() {
		if (IsTextComplete() && _coolDown <= 0) {
			_isActive = false;
			return;
		}
		if (_coolDown <= 0) {
			WriteAll();
			_coolDown = clickCooldownTime;
		}
	}

	protected override bool IsSelfActive() {
		return _isActive;
	}

	public bool IsTextComplete() {
		return charIndex >= textToWrite.Length;
	}
	
	public void WriteAll() {
		uiText.text = textToWrite;
		charIndex = textToWrite.Length;
	}

	private IEnumerator DeactivateTimed() {
		yield return new WaitUntil(IsTextComplete);
		yield return new WaitForSeconds(autoTextStayTime);
		Deactivate();
	}
}