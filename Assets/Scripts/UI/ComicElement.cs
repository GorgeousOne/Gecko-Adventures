
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that represents a comic element that can be clicked through with interact button.
/// Can have comic elements as children that have to be clicked through before this element disappears.
/// </summary>
public class ComicElement : MonoBehaviour {

	[Header("Auto Read")]
	[SerializeField] protected bool doAutoContinue;

	protected List<ComicElement> ChildComics;
	protected int ActiveChildIndex = -1;
	protected Action DeactivateCallback;
	
	protected void OnEnable() {
		ChildComics = new List<ComicElement>();

		foreach(Transform child in transform) {
			ComicElement childComic = child.gameObject.GetComponent<ComicElement>();

			if (childComic != null) {
				ChildComics.Add(childComic);
			}
		}
	}
	
	public void Activate() {
		Activate(null);
	}
	
	public virtual void Activate(Action deactivateCallback) {
		gameObject.SetActive(true);
		DeactivateCallback = deactivateCallback;
	}

	/// <summary>
	/// Handles interaction with this comic element when Continue button is clicked
	/// </summary>
	public void OnInteract() {
		if (IsSelfActive()) {
			Interact();

			if (!IsSelfActive()) {
				ActivateNextChild();
			}
		} else {
			if (ActiveChildIndex == -1) {
				ActivateNextChild();
				return;
			}
			if (ActiveChildIndex >= ChildComics.Count) {
				return;
			}
			ChildComics[ActiveChildIndex].OnInteract();

			if (!ChildComics[ActiveChildIndex].IsActive()) {
				DeactivateActiveChild();
			}
		}
	}

	private void DeactivateActiveChild() {
		ChildComics[ActiveChildIndex].Deactivate();
	}
	
	private void ActivateNextChild() {
		if (++ActiveChildIndex < ChildComics.Count) {
			ChildComics[ActiveChildIndex].Activate(ActivateNextChild);
		}
	}

	/// <summary>
	/// Returns true if self or a child comic element still need time for being displayed
	/// </summary>
	/// <returns></returns>
	public bool IsActive() {
		return IsSelfActive() || ActiveChildIndex < ChildComics.Count;
	}
	
	protected virtual bool IsSelfActive() {
		return false;
	}

	/// <summary>
	/// Deactivates element which may include a fade out.
	/// Invokes deactivate callback at end of fade out.
	/// </summary>
	public virtual void Deactivate() {
		gameObject.SetActive(false);
		DeactivateCallback?.Invoke();
	}

	protected virtual void Interact() {}
}