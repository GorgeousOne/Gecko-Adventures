using System.Collections;
using UnityEngine;

public class SpikeController : MonoBehaviour, Resettable {
	
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

	private bool _isExtended = true;
	private SpriteRenderer _renderer;

	private bool _savedWasExtended;

	private AudioSource _spikeExtendAudio;
	
	private void OnEnable() {
		_spikeExtendAudio = GetComponent<AudioSource>();

		if (!timedActivationEnabled && !trampleActivationEnabled) {
			Destroy(this);
		}
		_renderer = GetComponent<SpriteRenderer>();
		SetExtended(_isExtended && !trampleActivationEnabled);
	}
	
	private void Update() {
		// _spikeExtendAudio = GetComponent<AudioSource>();

		if (timedActivationEnabled && !trampleActivationEnabled) {
			if (_isExtended != CalcTimedExtendedState()) {
				SetExtended(!_isExtended);
			}
		}
	}
	
	public void SetExtended(bool state) {
		_isExtended = state;
		_renderer.sprite = _isExtended ? extended : retracted;
		damageCollider.enabled = _isExtended;
		// _spikeExtendAudio = GetComponent<AudioSource>();
		_spikeExtendAudio.enabled = _isExtended;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!_isExtended && trampleActivationEnabled && other.CompareTag("Player")) {
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
	
	public void SaveState() {
		_savedWasExtended = _isExtended;
	}

	public void ResetState() {
		_isExtended = _savedWasExtended;
	}
}