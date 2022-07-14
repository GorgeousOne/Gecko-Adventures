using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour, Resettable {
	
	[SerializeField] private List<Triggerable> connected;

	protected void Activate() {
		foreach (Triggerable triggerable in connected) {
			triggerable.OnSwitchToggle(true);
		}
	}
	
	protected void OnDrawGizmos() {
		Gizmos.color = new Color(.25f, 0, 0);
		foreach (Triggerable triggerable in connected) {
			Gizmos.DrawLine(transform.position, triggerable.gameObject.transform.position);
		}
	}

	public void SaveState() {
		throw new System.NotImplementedException();
	}

	public void ResetState() {
		throw new System.NotImplementedException();
	}
}