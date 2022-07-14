
using UnityEngine;
using UnityEngine.UI;

public class TextKeyHint : MonoBehaviour, IKeyHint {

	[SerializeField] private Image image;
	[SerializeField] private string keyAction;
	
	public void UpdateKeyHint(DeviceDisplaySettings settings) {
		Sprite keySprite = settings.GetBindingSprite(keyAction);
		image.sprite = keySprite;
		RectTransform rectTransform = transform as RectTransform;
		Vector2 size = rectTransform.sizeDelta;

		size.x = size.y * keySprite.bounds.size.x / keySprite.bounds.size.y;
		rectTransform.sizeDelta = size;
	}
}
