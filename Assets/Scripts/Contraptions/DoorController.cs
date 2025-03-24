using UnityEngine;
using UnityEngine.Serialization;

public class DoorController : Triggerable, IResettable {

	[SerializeField] private Vector2 openOffset;
	[SerializeField] private float openingTime = 1;
	[SerializeField] private float closingTime = 1;
	[SerializeField] private float deepPitch = 0.9f;
	
	private Vector2 _startPos;
	private bool _isOpening;
	private float _moveStartTime;

	private bool _savedWasOpen;	
	private float _savedMoveStart;

	private AudioSource _sound;
	
	private void Start() {
		_startPos = transform.position;
		//set door to end of moving animation
		_moveStartTime = Time.deltaTime - (_isOpening ? openingTime : closingTime);
		SaveState();
		_sound = GetComponent<AudioSource>();
	}

	void Update() {
		float moveDuration = LevelTime.time - _moveStartTime;
		float openingProgress;

		if (_isOpening) {
			openingProgress = Mathf.Clamp01(moveDuration / openingTime);
			if (openingProgress >= 1) {
				_sound.Stop();
			}
		} else {
			openingProgress = 1 - Mathf.Clamp01(moveDuration / closingTime);
			if (openingProgress <= 0) {
				_sound.Stop();
			}
		}
		transform.position = Vector2.Lerp(_startPos, _startPos + openOffset, openingProgress);
	}

	protected override void OnToggle(bool isEnabled) {
		if (isEnabled == _isOpening) {
			return;
		}
		float openingProgress = Mathf.Clamp01(((Vector2) transform.position - _startPos).magnitude / openOffset.magnitude);
		
		if (_isOpening) {
			_moveStartTime = LevelTime.time - closingTime * (1 - openingProgress);
		} else {
			_moveStartTime = LevelTime.time - openingTime * openingProgress;
		}
		_isOpening = isEnabled;
		_sound.pitch = _isOpening ? 1f : deepPitch;
		_sound.Play();
	}

	private void OnDrawGizmos() {
		Vector2 position = _startPos != Vector2.zero ? _startPos : transform.position;
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(position, position + openOffset);
	}

	
	public new void SaveState() {
		base.SaveState();
		_savedWasOpen = _isOpening;
		_savedMoveStart = _moveStartTime;
	}
	
	public new void ResetState() {
		base.ResetState();
		_isOpening = _savedWasOpen;
		_moveStartTime = _savedMoveStart;
	}
}
