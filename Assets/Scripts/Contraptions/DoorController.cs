using UnityEngine;
using UnityEngine.Serialization;

public class DoorController : Triggerable {

	[SerializeField] private Vector2 openOffset;
	[FormerlySerializedAs("moveTime")] [SerializeField] private float openingTime = 1;
	[SerializeField] private float closingTime = 1;

	private Vector2 _startPos;
	private bool _isOpening;
	private float _moveStartTime;

	private bool _savedWasOpen;
	private float _savedMoveStart;

	private AudioSource[] _listOfDoorAudios;
	
	private void Start() {
		_startPos = transform.position;
		//set door to end of moving animation
		_moveStartTime = Time.deltaTime - (_isOpening ? openingTime : closingTime);
		SaveState();
		_listOfDoorAudios = GetComponents<AudioSource>();
		_listOfDoorAudios[0].enabled = false;
		_listOfDoorAudios[1].enabled = false;
	}

	// Update is called once per frame
	void Update() {
		float moveDuration = LevelTime.time - _moveStartTime;
		float openingProgress;

		if (_isOpening) {
			openingProgress = Mathf.Clamp01(moveDuration / openingTime);
			_listOfDoorAudios[0].enabled = true;
			_listOfDoorAudios[1].enabled = false;
		} else {
			openingProgress = 1 - Mathf.Clamp01(moveDuration / closingTime);
			_listOfDoorAudios[0].enabled = false;
			_listOfDoorAudios[1].enabled = true;
		}
		transform.position = Vector2.Lerp(_startPos, _startPos + openOffset, openingProgress);
	}

	public override void OnSwitchToggle(bool isEnabled) {
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
	}

	private void OnDrawGizmos() {
		Vector2 position = _startPos != Vector2.zero ? _startPos : transform.position;
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(position, position + openOffset);
	}

	public new void SaveState() {
		_savedWasOpen = _isOpening;
		_savedMoveStart = _moveStartTime;
	}

	public new void ResetState() {
		_isOpening = _savedWasOpen;
		_moveStartTime = _savedMoveStart;
	}
}
