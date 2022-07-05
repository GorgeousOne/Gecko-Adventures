using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComicController : MonoBehaviour {
	
	private List<ComicElement> _comicsElements;
	private int _activeComicIndex;
	private PlayerControls _controls;

	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.Interact.performed += _ => InteractOnComic();
		_controls.Enable();
		LoadElements();
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
			gameObject.SetActive(false);
		}
	}

	private void OnDisable() {
		_controls.Disable();
	}
	
	private void InteractOnComic() {
		_comicsElements[_activeComicIndex].OnInteract();
		
		if (_comicsElements[_activeComicIndex].IsActive()) {
			return;
		}
		if (++_activeComicIndex < _comicsElements.Count) {
			_comicsElements[_activeComicIndex - 1].Deactivate();
			_comicsElements[_activeComicIndex].Activate();
			return;
		}
		gameObject.SetActive(false);
	}
}