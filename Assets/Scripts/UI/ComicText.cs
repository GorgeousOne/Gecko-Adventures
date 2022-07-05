
using TMPro;
using UnityEngine;

/// <summary>
/// A comic element that types a text on the screen char by char over time
/// </summary>

public class ComicText : ComicElement {

	//Jan Ehlers formula :D after .4 seconds of not being able to interact the user gets impatient
	[SerializeField] private float clickCooldownTime = .4f;
	[SerializeField] private float timePerChar = 0.02f;

	private TMP_Text uiText;
	private string textToWrite;
	private int charIndex;
	private float timer;
	
	private bool _isActive;
	private float _coolDown;

	private new void OnEnable() {
		base.OnEnable();
		uiText = GetComponent<TMP_Text>();
		textToWrite = uiText.text;
	}

	public override void Activate() {
		gameObject.SetActive(true);
		charIndex = 0;
		_isActive = true;
		_coolDown = clickCooldownTime;
	}
	
	public override void Deactivate() {
		gameObject.SetActive(false);
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
		if (IsTextComplete()) {
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
}