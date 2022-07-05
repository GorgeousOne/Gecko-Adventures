
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that represents a comic element that can be clicked through with interact button.
/// Can have comic elements as children that have to be clicked through before this element disappears.
/// </summary>
public abstract class ComicElement : MonoBehaviour {

	protected List<ComicElement> ChildComics;
	protected int ActiveChildIndex;
	
	protected void Awake() {
		ChildComics = new List<ComicElement>();
		
		foreach(Transform child in transform) {
			ComicElement childComic = child.gameObject.GetComponent<ComicElement>();

			if (childComic != null) {
				ChildComics.Add(childComic);
			}
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	public void OnInteract() {
		if (IsSelfActive()) {
			Interact();

			if (!IsSelfActive()) {
				ActiveChildIndex = -1;
				ActivateNextChild();
			}
		} else {
			if (ActiveChildIndex >= ChildComics.Count) {
				return;
			}
			ChildComics[ActiveChildIndex].OnInteract();

			if (!ChildComics[ActiveChildIndex].IsActive()) {
				ActivateNextChild();
			}
		}
	}

	private void ActivateNextChild() {
		if (++ActiveChildIndex < ChildComics.Count) {
			ChildComics[ActiveChildIndex].Activate();
		}		
	}

	public bool IsActive() {
		return IsSelfActive() || ActiveChildIndex < ChildComics.Count;
	}

	public abstract void Activate();
	protected abstract void Interact();
	protected abstract bool IsSelfActive();
}