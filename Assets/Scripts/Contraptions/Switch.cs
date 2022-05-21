using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable {

	[SerializeField] private List<Switchable> connected;

	protected bool IsEnabled;
	 
	protected override void OnInteract() {
		IsEnabled = !IsEnabled;

		foreach (Switchable switchable in connected) {
			switchable.OnSwitchToggle(IsEnabled);
		}
	}
	
	protected void OnDrawGizmos() {
		Gizmos.color = IsEnabled ? new Color(.75f, 0, 0) : new Color(.25f, 0, 0);
		foreach (Switchable switchable in connected) {
			Gizmos.DrawLine(transform.position, ((MonoBehaviour) switchable).gameObject.transform.position);
		}
	}
}