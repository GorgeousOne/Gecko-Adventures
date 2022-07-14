using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceController : MonoBehaviour {

	[SerializeField] private KeyHintConfigurator keyHints;

	private PlayerInput _playerInput;
	private string _currentControlScheme;
	
	private void OnEnable() {
		_playerInput = GetComponent<PlayerInput>();
		OnDeviceRegained();
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

	public void OnDeviceRegained() {
		StartCoroutine(WaitForDeviceToBeRegained());
	}

	IEnumerator WaitForDeviceToBeRegained() {
		yield return new WaitForSeconds(0.1f);
		UpdateKeyHintVisuals(keyHints.GetDeviceSettings(_playerInput));
	}
	
	private void UpdateKeyHintVisuals(DeviceDisplaySettings settings) {
		foreach (IKeyHint hint in FindObjectsOfType<MonoBehaviour>(true).OfType<IKeyHint>()) {
			hint.UpdateKeyHint(settings);
		}
	}
}