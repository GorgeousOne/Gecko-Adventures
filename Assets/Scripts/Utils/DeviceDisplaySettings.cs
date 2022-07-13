using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Device Display Settings", menuName = "Scriptable Objects/Device Display Settings", order = 1)]
public class DeviceDisplaySettings : ScriptableObject {

	public Sprite moveIcon;
	public Sprite jumpIcon;
	public Sprite crouchIcon;
	public Sprite interactIcon;
	public Sprite tongueShootIcon;
	public Sprite tongueLengthChangeIcon;

	public Sprite GetBindingSprite(string action) {
		switch (action) {
			case "move":
				return moveIcon;
			case "jump":
				return jumpIcon;
			case "crouch":
				return crouchIcon;
			case "interact":
				return interactIcon;
			case "tongue-shoot":
				return tongueShootIcon;
			case "tongue-length-change":
				return tongueLengthChangeIcon;
			default:
				throw new Exception("Unknown action: \"" + action + "\"");
		}
	}
}