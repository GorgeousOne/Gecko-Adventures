using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSwitch : Interactable, IResettable {

	[SerializeField] private Sprite disabledSprite;
	[SerializeField] private Sprite enabledSprite;
	[SerializeField] private float deepPitch = 0.8f;
	[SerializeField] private List<Triggerable> connected;
	
	protected bool IsEnabled;
	protected bool _savedWasEnabled;
	private SpriteRenderer _renderer;
	private AudioSource _sound;
	
	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		_sound = GetComponent<AudioSource>();
	}

	protected override void OnInteract() {
		Toggle();
	}

	protected void Toggle() {
		IsEnabled = !IsEnabled;
		_renderer.sprite = IsEnabled ? enabledSprite : disabledSprite;

		foreach (Triggerable toggleable in connected) {
			toggleable.OnReceiveToggleSignal(IsEnabled);
		}

		_sound.pitch = IsEnabled ? 1f : deepPitch;
		_sound.Play();
		
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
		_renderer.sprite = IsEnabled ? enabledSprite : disabledSprite;
	}
}