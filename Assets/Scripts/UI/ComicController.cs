using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComicController : MonoBehaviour {
	
	// [SerializeField] private GameObject continueText;
	// [SerializeField] private SortedDictionary<GameObject, float> Imgs;
	
	private List<ComicElement> _comicsElements;
	private int _activeComicIndex;
	private PlayerControls _controls;

	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.Interact.performed += _ => InteractOnComic();
		_controls.Enable();

		// Time.timeScale = 0;
		
		LoadElements();
		// _clickCooldown = 1;
		// ChildComic[_currentImageIndex].SetActive(true);
	}
	
	private void LoadElements() {
		_comicsElements = new List<ComicElement>();
		
		foreach(Transform child in transform) {
			ComicElement childComic = child.gameObject.GetComponent<ComicElement>();

			if (childComic != null) {
				_comicsElements.Add(childComic);
			}
		}
		if (_comicsElements.Any()) {
			_activeComicIndex = 0;
			_comicsElements[_activeComicIndex].Activate();
		} else {
			Debug.LogWarning($"No elements found in comic \"{gameObject.name}\".");

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
		// if (_clickCooldown > 0) {
			// continueText.SetActive(false);
			// _clickCooldown -= Time.deltaTime;
		// } else {
			// continueText.SetActive(true);
		// }
	}

	private void InteractOnComic() {
		Debug.Log("interact " + _comicsElements[_activeComicIndex].IsActive());
		// _activeComicIndex
		// if (_clickCooldown > 0) {
		// return;
		// }
		// _comicsElements[_activeComicIndex].SetActive(false);
		_comicsElements[_activeComicIndex].OnInteract();

		if (_comicsElements[_activeComicIndex].IsActive()) {
			return;
		}
		if (++_activeComicIndex < _comicsElements.Count) {
			_comicsElements[_activeComicIndex].Activate();
			// _clickCooldown = 1;
			return;
		}
		// Time.timeScale = 1;
		gameObject.SetActive(false);
	}
}