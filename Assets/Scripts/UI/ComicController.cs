using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ComicController : MonoBehaviour {

	[SerializeField] private UnityEvent comicEndEvent;
	
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
			_activeComicIndex = -1;
			ActivateNextComic();
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
		DeactivateCurrentComic();
	}

	private void DeactivateCurrentComic() {
		_comicsElements[_activeComicIndex].Deactivate();
	}

	private void ActivateNextComic() {
		++_activeComicIndex;
		bool hasNextComic = _activeComicIndex + 1 < _comicsElements.Count;
		_comicsElements[_activeComicIndex].Activate(hasNextComic ? ActivateNextComic : EndComic);
	}

	private void EndComic() {
		comicEndEvent.Invoke();
		gameObject.SetActive(false);
	}
}