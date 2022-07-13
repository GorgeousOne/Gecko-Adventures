using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceController : MonoBehaviour {

	[SerializeField] private KeyHintConfigurator keyHints;
	
	private PlayerInput _playerInput;
	private string _currentControlScheme;
	
	private void OnEnable() {
		_playerInput = GetComponent<PlayerInput>();
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

	//This is automatically called from PlayerInput, when the input device has been disconnected and can not be identified
	//IE: Device unplugged or has run out of batteries

	// public void OnDeviceLost() {
	// 	// playerVisualsBehaviour.SetDisconnectedDeviceVisuals();
	// }
	//
	// public void OnDeviceRegained() {
	// 	StartCoroutine(WaitForDeviceToBeRegained());
	// }
	//
	// IEnumerator WaitForDeviceToBeRegained()
	// {
	// 	yield return new WaitForSeconds(0.1f);
	// 	// playerVisualsBehaviour.UpdatePlayerVisuals();
	// }

	private void UpdateKeyHintVisuals(DeviceDisplaySettings settings) {
		foreach (Resettable resettable in FindObjectsOfType<Resettable>(true)) {
			
		}
	}
}