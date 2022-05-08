using UnityEngine;

public class SpikeController : MonoBehaviour {
	[SerializeField] private Sprite retracted;
	[SerializeField] private Sprite extended;

	private bool _isExtended = true;
	private SpriteRenderer _renderer;
	private Collider2D _collider;
	
	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<Collider2D>();
		SetExtended(_isExtended);
	}
	
	public void SetExtended(bool state) {
		_isExtended = state;
		
		if (_isExtended) {
			_renderer.sprite = extended;
			_collider.enabled = true;
		} else {
			_renderer.sprite = retracted;
			_collider.enabled = false;
		}
	}
}
