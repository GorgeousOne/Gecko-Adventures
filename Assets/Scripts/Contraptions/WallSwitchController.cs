using UnityEngine;

public class WallSwitchController : Switch {

	private SpriteRenderer _renderer;
	
	protected new void Start() {
		base.Start();
		_renderer = GetComponent<SpriteRenderer>();
	}

	protected override void OnInteract() {
		base.OnInteract();
		_renderer.flipX = IsEnabled;
	}
}