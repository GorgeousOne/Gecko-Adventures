using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleKeyHint : MonoBehaviour, IKeyHint {
	
	[SerializeField] private string keyAction;
	[SerializeField] private Sprite hintBackground;
	[SerializeField] private float padding = .3125f;
	[SerializeField] private Vector2 bubbleOffset = new (0, 1);
	[SerializeField] private Vector2 keyHintOffset = new (0, .03125f);

	private GameObject _keyHintBubble;
	private bool _isPlayerInRange;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isPlayerInRange = true;
			SetHintVisible(_isPlayerInRange);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_isPlayerInRange = false;
			SetHintVisible(_isPlayerInRange);
		}
	}
	
	public void SetHintVisible(bool state) {
		_keyHintBubble.SetActive(state);
	}

	public void UpdateKeyHint(DeviceDisplaySettings settings) {
		CreateKeyHint(settings);
		SetHintVisible(_isPlayerInRange);
	}
	
	private void CreateKeyHint(DeviceDisplaySettings settings) {
		if (_keyHintBubble != null) {
			Destroy(_keyHintBubble);
		}
		_keyHintBubble = new GameObject("KeyHintBubble");
		_keyHintBubble.SetActive(false);
		_keyHintBubble.transform.parent = transform;
		_keyHintBubble.transform.localPosition = bubbleOffset;

		SpriteRenderer bubble = _keyHintBubble.AddComponent<SpriteRenderer>();
		bubble.sprite = hintBackground;
		bubble.sortingLayerName = "Fore";
		bubble.sortingOrder = 1;
		bubble.drawMode = SpriteDrawMode.Sliced;
		
		GameObject keyHint = new GameObject("KeyHint");
		keyHint.transform.parent = _keyHintBubble.transform;

		Sprite keySprite = settings.GetBindingSprite(keyAction);

		SpriteRenderer binding = keyHint.AddComponent<SpriteRenderer>();
		binding.sprite = keySprite;
		binding.color = Color.black;
		binding.sortingLayerName = "Fore";
		binding.sortingOrder = 2;
		binding.transform.localPosition = keyHintOffset;
		
		//update bubble to fit key hint
		Vector2 keySize = binding.size;
		Vector2 bubbleSize = bubble.size;
		
		Vector2 newBubbleSize = new Vector2(
			Mathf.Max(bubbleSize.x,keySize.x + padding),
			Mathf.Max(bubbleSize.y, keySize.y + padding));
		bubble.size = newBubbleSize;
	}
}
