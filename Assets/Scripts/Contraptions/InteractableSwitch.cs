using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSwitch : Interactable, Resettable {

	[SerializeField] private List<Triggerable> connected;

	protected bool IsEnabled;
	protected bool _savedWasEnabled;
	private SpriteRenderer _renderer;

	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();

	}

	protected override void OnInteract() {
		Toggle();
	}

	protected void Toggle() {
		IsEnabled = !IsEnabled;
		_renderer.flipX = IsEnabled;

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

	public void SaveState() {
		_savedWasEnabled = IsEnabled;
	}

	public void ResetState() {
		IsEnabled = _savedWasEnabled;
		_renderer.flipX = IsEnabled;
	}
}