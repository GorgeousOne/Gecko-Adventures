using UnityEngine;

public class MaskCloseup : MonoBehaviour {

	[SerializeField] private PickupHandler pickupHandler;
	[SerializeField] private Animator transition;

	private PlayerControls _controls;
	private bool _isVisible;

	private AudioSource _notificationAudio;
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Enable();

		pickupHandler.maskCollectEvent.AddListener(OnMaskCollect);
		_notificationAudio = GetComponent<AudioSource>();
		_notificationAudio.enabled = false;
	}

	private void OnDisable() {
		_controls.Disable();
	}

	public void OnMaskCollect() {
		transition.SetTrigger("FadeIn");
		_isVisible = true;
		_notificationAudio.enabled = true;
	}

	private void Update() {
		if (_isVisible && _controls.Player.Interact.WasPerformedThisFrame()) {
			_isVisible = false;
			transition.SetTrigger("FadeOut");
		}
	}
}
