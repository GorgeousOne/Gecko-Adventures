using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableSwitch : Interactable {

	[SerializeField] private List<Triggerable> connected;

	protected bool IsEnabled;
	protected bool _savedWasEnabled;
	
	protected override void OnInteract() {
		Toggle();
	}

	protected void Toggle() {
		IsEnabled = !IsEnabled;

		foreach (Triggerable toggleable in connected) {
			toggleable.OnSwitchToggle(IsEnabled);
		}
	}
	
	protected void OnDrawGizmos() {
		Gizmos.color = IsEnabled ? new Color(.75f, 0, 0) : new Color(.25f, 0, 0);

		foreach (Triggerable toggleable in connected) {
			if (toggleable) {
				Gizmos.DrawLine(transform.position, toggleable.gameObject.transform.position);
			}
		}
	}

	public new void SaveState() {
		_savedWasEnabled = IsEnabled;
	}

	public new void ResetState() {
		IsEnabled = _savedWasEnabled;
	}
}