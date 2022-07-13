using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceController : MonoBehaviour {

	[SerializeField] private KeyHintConfigurator keyHints;
	
	private PlayerInput _playerInput;
	private string _currentControlScheme;
	
	private void OnEnable() {
		_playerInput = GetComponent<PlayerInput>();
		OnControlsChanged();
	}

	//INPUT SYSTEM AUTOMATIC CALLBACKS --------------

	//This is automatically called from PlayerInput, when the input device has changed
	//(IE: Keyboard -> Xbox Controller)
	public void OnControlsChanged() {
		if(_playerInput.currentControlScheme != _currentControlScheme) {
			_currentControlScheme = _playerInput.currentControlScheme;
			UpdateKeyHintVisuals(keyHints.GetDeviceSettings(_playerInput));
		}
	}

	private void UpdateKeyHintVisuals(DeviceDisplaySettings settings) {
		foreach (IKeyHint hint in FindObjectsOfType<SimpleKeyHint>(true)) {
			hint.UpdateKeyHint(settings);
		}
		foreach (IKeyHint hint in FindObjectsOfType<Interactable>(true)) {
			hint.UpdateKeyHint(settings);
		}
	}
}