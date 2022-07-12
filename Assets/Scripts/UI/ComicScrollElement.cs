
using UnityEngine;

public class ComicScrollElement : ComicElement {

	[SerializeField] private float unitsPerSecond = 1;
	[SerializeField] private int ppu = 16;
	
	private bool _isScrolledThrough;
	private float _ppiInv;

	private void Update() {
		if (_isScrolledThrough) {
			Debug.Log("stop");
			gameObject.SetActive(false);
		}
		
		Rect screenRect = ((RectTransform) transform).rect;
		float movement = Time.deltaTime * unitsPerSecond;

		float maxY = transform.position.y + 0.5f * screenRect.height / ppu;
		_isScrolledThrough = true;
		
		foreach (RectTransform child in transform) {
			
			child.transform.Translate(0, movement, 0);
			float childMinY = child.position.y - 0.5f * child.rect.height / ppu;

			if (childMinY < maxY) {
				_isScrolledThrough = false;
			}
		}
	}
	
	private void OnDrawGizmos() {
		RectTransform rectTransform = ((RectTransform) transform);
		float maxY = rectTransform.position.y + 0.5f * rectTransform.rect.height / ppu;
		
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(new Vector3(-10, maxY, 0), new Vector3(10, maxY, 0));
		Gizmos.color = new Color(1, .5f, 0);

		foreach (RectTransform child in transform) {
			float childY = child.position.y - 0.5f * child.rect.height / ppu;
			Gizmos.DrawLine(new Vector3(-10, childY, 0), new Vector3(10, childY, 0));
		}
	}
}
