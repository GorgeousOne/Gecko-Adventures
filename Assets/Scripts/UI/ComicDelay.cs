
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// idk what this does, probably adding a timed pause in an active comic
/// </summary>
public class ComicDelay : ComicElement {

	[SerializeField] [Min(0.1f)] private float waitTime = 3f;

	private float _coolDown;
	
	public override void Activate(Action deactivateCallback) {
		base.Activate(deactivateCallback);
		StartCoroutine(DeactivateTimed());
	}

	protected override bool IsSelfActive() {
		return true;
	}
	
	private IEnumerator DeactivateTimed() {
		yield return new WaitForSeconds(waitTime);
		Deactivate();
	}
}