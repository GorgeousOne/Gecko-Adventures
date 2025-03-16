using System.Collections.Generic;
using UnityEngine;

public abstract class Switch : MonoBehaviour {

	[SerializeField] private List<Triggerable> connected;

	protected bool IsEnabled;
	
	protected void Toggle() {
		IsEnabled = !IsEnabled;

		foreach (Triggerable toggleable in connected) {
			toggleable.OnReceiveToggleSignal(IsEnabled);
		}
	}
	
	protected void OnDrawGizmos() {
		Gizmos.color = IsEnabled ? new Color(.75f, 0, 0) : new Color(.25f, 0, 0);
		foreach (Triggerable toggleable in connected) {
			Gizmos.DrawLine(transform.position, toggleable.gameObject.transform.position);
		}
	}
}