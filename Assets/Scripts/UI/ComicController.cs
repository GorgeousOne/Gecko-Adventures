using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComicController : MonoBehaviour {
	
	[SerializeField] private GameObject continueText;
	[SerializeField] private Image background;
	[SerializeField] private SortedDictionary<GameObject, float> Imgs;
	
	private PlayerControls _controls;
	private List<GameObject> _images;
	private int _currentImageIndex;
	private float _clickCooldown;
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.Interact.performed += _ => ShowNextImage();
		_controls.Enable();

		Time.timeScale = 0;
		
		LoadImages();
		_currentImageIndex = 0;
		_clickCooldown = 1;
		_images[_currentImageIndex].SetActive(true);
	}

	private void LoadImages() {
		_images = new List<GameObject>();
		Transform imageHolder = transform.Find("Images");

		foreach (Transform image in imageHolder) {
			_images.Add(image.gameObject);
		}
		if (!_images.Any()) {
			Debug.LogWarning($"No images found in comic {gameObject.name}.");
		}
	}

	private void OnDisable() {
		_controls.Disable();
	}

	private void Update() {
		// Color bgColor = background.color;

		// if (bgColor.a <= 0.9f) {
			// bgColor.a += 0.05f;
			// background.color = bgColor;
		// }
		if (_clickCooldown > 0) {
			continueText.SetActive(false);
			_clickCooldown -= Time.deltaTime;
		} else {
			continueText.SetActive(true);
		}
	}

	private void ShowNextImage() {
		if (_clickCooldown > 0) {
			return;
		}
		_images[_currentImageIndex].SetActive(false);
		_currentImageIndex += 1;
		

		if (_currentImageIndex < _images.Count) {
			_images[_currentImageIndex].SetActive(true);
			_clickCooldown = 1;
			return;
		}
		Time.timeScale = 1;
		gameObject.SetActive(false);
	}
}