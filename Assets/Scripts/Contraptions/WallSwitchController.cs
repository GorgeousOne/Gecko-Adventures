using UnityEngine;

public class WallSwitchController : InteractableSwitch {

	private SpriteRenderer _renderer;
	
	protected void Start() {
		_renderer = GetComponent<SpriteRenderer>();
	}
	
	protected override void OnInteract() {
		base.OnInteract();
		_renderer.flipX = IsEnabled;
	}

	public void ResetState() {
		base.ResetState();
		_renderer.flipX = IsEnabled;
	}
}