using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleKeyHint : MonoBehaviour, IKeyHint {
	
	[SerializeField] private string keyAction;
	[SerializeField] private Sprite hintBackground;
	[SerializeField] private Vector2 padding = new Vector2(.25f, .28125f);
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

		SpriteRenderer keyBind = keyHint.AddComponent<SpriteRenderer>();
		keyBind.sprite = keySprite;
		keyBind.color = Color.black;
		keyBind.sortingLayerName = "Fore";
		keyBind.sortingOrder = 2;
		keyBind.transform.localPosition = keyHintOffset;

		float ppu = keySprite.pixelsPerUnit;
		Vector4 spriteBorder = keySprite.border / ppu;

		keyBind.transform.localPosition += new Vector3(
				.5f * (-spriteBorder.x + spriteBorder.z),
				.5f * (-spriteBorder.y + spriteBorder.w), 0);

		//update bubble to fit key hint
		Vector2 keySize = keyBind.size;
		keySize -= new Vector2(
			spriteBorder.x + spriteBorder.z,
			spriteBorder.y + spriteBorder.w);
		
		Vector2 bubbleSize = bubble.size;
		Vector2 newBubbleSize = new Vector2(
			Mathf.Max(bubbleSize.x,keySize.x + 2 * padding.x),
			Mathf.Max(bubbleSize.y, keySize.y + 2 * padding.y));

		//make pixel count even
		newBubbleSize.x += newBubbleSize.x % (2f / ppu);
		newBubbleSize.y += newBubbleSize.y % (2f / ppu);
		bubble.size = newBubbleSize;
	}
}
