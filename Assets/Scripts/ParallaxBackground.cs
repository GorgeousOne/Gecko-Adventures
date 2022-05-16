using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

	[SerializeField] [Range(0, 1)] private float parallaxEffect = 1;
	[SerializeField] [Min(1)] private int ppu = 16;
	
	private Vector3 _startPos;
	private float _width;
	private Camera _cam;
	
	private void Start() {
		_cam = Camera.main;
		_width = GetComponent<SpriteRenderer>().bounds.size.x;
		_startPos = transform.position;
	}

	private void LateUpdate() {
		float camDist = _cam.transform.position.x * (1 - parallaxEffect);
		float dist = _cam.transform.position.x * parallaxEffect;
		Vector3 position = transform.position;

		if (camDist > _startPos.x + _width) {
			_startPos.x += _width;
		} else if (camDist < _startPos.x - _width) {
			_startPos.x -= _width;
		}
		position.x = _startPos.x + dist;
		transform.position = position;
	}
}