using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleKeyHint : MonoBehaviour, IKeyHint {

	
	[SerializeField] private string keyAction;
	[SerializeField] private Sprite hintBackground;
	[SerializeField] private float padding = .25f;
	[SerializeField] private Vector2 keyHintOffset = new (0, 1);

	private GameObject _keyHint;
	private bool _isPlayInRange;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isPlayInRange = true;
			SetHintVisible(_isPlayInRange);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isPlayInRange = false;
			SetHintVisible(_isPlayInRange);
		}
	}
	
	private void SetHintVisible(bool state) {
		_keyHint.SetActive(state);
	}

	public void UpdateKeyHint(DeviceDisplaySettings settings) {
		CreateKeyHint(settings);
		SetHintVisible(_isPlayInRange);
	}
	
	private void CreateKeyHint(DeviceDisplaySettings settings) {
		if (_keyHint != null) {
			Destroy(_keyHint);
		}
		_keyHint = new GameObject("KeyHint");
		_keyHint.SetActive(false);
		_keyHint.transform.parent = transform;
		_keyHint.transform.localPosition = keyHintOffset;
	
		SpriteRenderer binding = _keyHint.AddComponent<SpriteRenderer>();
		Sprite keySprite = settings.GetBindingSprite(keyAction);
		binding.sprite = keySprite;
		binding.sortingLayerName = "Fore";
		binding.sortingOrder = 2;

		SpriteRenderer bubble = _keyHint.AddComponent<SpriteRenderer>();
		bubble.sprite = hintBackground;
		bubble.sortingLayerName = "Fore";
		bubble.sortingOrder = 1;
		bubble.drawMode = SpriteDrawMode.Sliced;

		float pixelScale = 1f / keySprite.pixelsPerUnit;
		Vector2 keySize = keySprite.rect.size;
		Vector2 bubbleSize = new Vector2(
			keySize.x * pixelScale + padding,
			keySize.y * pixelScale + padding);
		_keyHint.transform.localScale = bubbleSize;
	}
}
