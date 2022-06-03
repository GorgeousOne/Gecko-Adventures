using UnityEngine;

public abstract class Triggerable : Resettable {

	public abstract void OnSwitchToggle(bool isEnabled);
}
