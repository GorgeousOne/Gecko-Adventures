using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SpikeController : Triggerable, IResettable {
	
	[SerializeField] private bool isExtended = true;
	
	[Header("Sprites")]
	[SerializeField] private Collider2D damageCollider;
	[SerializeField] private Sprite retracted;
	[SerializeField] private Sprite extended;

	[Header("Timing")] 
	[SerializeField] private bool timedActivationEnabled;
	[SerializeField] [Min(0)] private float extendOffset;
	[SerializeField] [Min(.5f)] private float extendTime = 2;
	[SerializeField] [Min(.5f)] private float retractTime = 2;

	[Header("Trample Triggering")]
	[SerializeField] private bool trampleActivationEnabled;
	[SerializeField] private float trampleTriggerOffset = 1f;
	[SerializeField] [Min(.1f)] private float trampleTriggerExtendTime = 3;

	private SpriteRenderer _renderer;

	private bool _savedWasExtended;

	private AudioSource _spikeExtendAudio;
	
	private void OnEnable() {
		_spikeExtendAudio = GetComponent<AudioSource>();

		if (isExtended && !timedActivationEnabled && !trampleActivationEnabled) {
			Destroy(this);
		}
		_renderer = GetComponent<SpriteRenderer>();
		SetExtended(isExtended && !trampleActivationEnabled);
	}
	
	private void Update() {
		// _spikeExtendAudio = GetComponent<AudioSource>();

		if (timedActivationEnabled && !trampleActivationEnabled) {
			if (isExtended != CalcTimedExtendedState()) {
				SetExtended(!isExtended);
			}
		}
	}
	
	public void SetExtended(bool state) {
		isExtended = state;
		_renderer.sprite = isExtended ? extended : retracted;
		damageCollider.enabled = isExtended;
		// _spikeExtendAudio = GetComponent<AudioSource>();
		_spikeExtendAudio.enabled = isExtended;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!isExtended && trampleActivationEnabled && other.CompareTag("Player")) {
			StartCoroutine(TrampleExtend());
		}
	}

	private IEnumerator TrampleExtend() {
		yield return new WaitForSeconds(trampleTriggerOffset);
		SetExtended(true);
		_spikeExtendAudio.enabled = true;
		yield return new WaitForSeconds(trampleTriggerExtendTime);
		SetExtended(false);
		_spikeExtendAudio.enabled = false;
	}

	private bool CalcTimedExtendedState() {
		return MathUtil.FloorMod(LevelTime.time - extendOffset, extendTime + retractTime) < extendTime;
	}

	protected override void OnToggle(bool isEnabled) {
		SetExtended(true);
	}

	public new void SaveState() {
		_savedWasExtended = isExtended;
	}

	public new void ResetState() {
		SetExtended(_savedWasExtended);
	}
}