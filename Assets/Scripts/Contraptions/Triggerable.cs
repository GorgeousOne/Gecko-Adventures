using UnityEngine;

public abstract class Triggerable : MonoBehaviour, Resettable {

	public abstract void OnSwitchToggle(bool isEnabled);

	public void SaveState() {
		throw new System.NotImplementedException();
	}

	public void ResetState() {
		throw new System.NotImplementedException();
	}
}
