using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable {

	[SerializeField] private List<Triggerable> connected;

	protected bool IsEnabled;
	 
	protected override void OnInteract() {
		IsEnabled = !IsEnabled;

		foreach (Triggerable triggerable in connected) {
			triggerable.OnSwitchToggle(IsEnabled);
		}
	}
	
	protected void OnDrawGizmos() {
		Gizmos.color = IsEnabled ? new Color(.75f, 0, 0) : new Color(.25f, 0, 0);
		foreach (Triggerable triggerable in connected) {
			Gizmos.DrawLine(transform.position, triggerable.gameObject.transform.position);
		}
	}
}