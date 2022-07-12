using UnityEngine;

public class ImageSoundMenuController : MonoBehaviour {

	[SerializeField] private GameObject crashSound;
	
	private AudioSource[] _menuAudiosList;

	private void Start() {
		_menuAudiosList = GetComponentsInChildren<AudioSource>(true);
		// background audio
		_menuAudiosList[0].enabled = true;
	}

	private void Update() {
		if (crashSound.activeSelf) {
			_menuAudiosList[0].enabled = false;
		}
	}
}
