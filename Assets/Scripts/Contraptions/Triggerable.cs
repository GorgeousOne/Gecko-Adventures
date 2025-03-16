using UnityEngine;

public abstract class Triggerable : MonoBehaviour, IResettable {

	[SerializeField] protected float triggerDelay;
	protected bool isTriggered;
	protected float triggerStart;
	private bool _savedWasTriggered;
	private float _savedLastTriggerStart;
	
	protected void FixedUpdate() {
		if (isTriggered && triggerStart < Time.time) {
			isTriggered = false;
			OnToggle(true);
		}
	}

	public void OnReceiveToggleSignal(bool isEnabled) {
		if (isEnabled) {
			isTriggered = true;
			triggerStart = Time.time + triggerDelay;
		}
		else {
			isTriggered = false;
			OnToggle(false);
		}
	}

	protected abstract void OnToggle(bool isEnabled);

	public void SaveState() {
		_savedWasTriggered = isTriggered;
		_savedLastTriggerStart = triggerStart;
	}

	public void ResetState() {
		isTriggered = _savedWasTriggered;
		triggerStart = _savedLastTriggerStart;
	}
}
