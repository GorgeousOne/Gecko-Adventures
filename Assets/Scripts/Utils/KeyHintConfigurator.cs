
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Key Hint Configurator", menuName = "Scriptable Objects/Key Hint Configurator", order = 1)]
public class KeyHintConfigurator : ScriptableObject {
	
	[System.Serializable]
	public struct DeviceSet
	{
		public string deviceRawPath;
		public DeviceDisplaySettings deviceDisplaySettings;
	}

	[SerializeField] private List<DeviceSet> listDeviceSets;

	public DeviceDisplaySettings GetDeviceSettings(PlayerInput playerInput) {
		string currentDeviceRawPath = playerInput.devices[0].ToString();

		foreach (DeviceSet set in listDeviceSets) {

			if (set.deviceRawPath == currentDeviceRawPath) {
				return set.deviceDisplaySettings;
			}
		}
		return listDeviceSets[0].deviceDisplaySettings;
	}
}
