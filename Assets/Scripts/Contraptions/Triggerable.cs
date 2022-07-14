using UnityEngine;

public abstract class Triggerable : MonoBehaviour, Resettable {

	public abstract void OnSwitchToggle(bool isEnabled);

	public void SaveState() {
	}

	public void ResetState() {
	}
}
