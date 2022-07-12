
using System;
using System.Collections;
using UnityEngine;

public class ComicDelay : ComicElement {

	[SerializeField] [Min(0.1f)] private float waitTime = 3f;

	private float _coolDown;
	
	public override void Activate(Action deactivateCallback) {
		base.Activate(deactivateCallback);
		Debug.Log("woooh mommy");
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