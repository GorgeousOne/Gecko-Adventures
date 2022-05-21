using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour {
	
	[SerializeField] private List<Triggerable> connected;

	protected void Activate() {
		foreach (Triggerable triggerable in connected) {
			triggerable.OnTriggerActivate();
		}
	}
	
	protected void OnDrawGizmos() {
		Gizmos.color = new Color(.25f, 0, 0);
		foreach (Triggerable triggerable in connected) {
			Gizmos.DrawLine(transform.position, ((MonoBehaviour) triggerable).gameObject.transform.position);
		}
	}
}