using UnityEngine;

public class WallSwitchController : Switch {

	private SpriteRenderer _renderer;
	
	private void Start() {
		base.Start();
		_renderer = GetComponent<SpriteRenderer>();
	}

	protected override void OnInteract() {
		base.OnInteract();
		_renderer.flipX = IsEnabled;
	}
}