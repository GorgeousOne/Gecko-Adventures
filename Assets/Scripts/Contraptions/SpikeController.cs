using System;
using System.Collections;
using UnityEngine;

public class SpikeController : Resettable {
	
	[Header("Sprites")]
	[SerializeField] private Sprite retracted;
	[SerializeField] private Sprite extended;

	[Header("Timing")] 
	[SerializeField] private bool timedActivationEnabled = false;
	[SerializeField] [Min(0)] private float extendOffset;
	[SerializeField] [Min(.5f)] private float extendTime = 2;
	[SerializeField] [Min(.5f)] private float retractTime = 2;
	
	private bool _isExtended = true;
	private SpriteRenderer _renderer;
	private Collider2D _collider;

	private bool _savedWasExtended;
	
	private void OnEnable() {
		_renderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<Collider2D>();
		SetExtended(_isExtended);
	}
	
	private void Update() {
		if (timedActivationEnabled) {
			bool timedExtendedState = CalcTimedExtendedState();
			if (_isExtended != timedExtendedState) {
				SetExtended(timedExtendedState);
			}
		}
	}
	
	public void SetExtended(bool state) {
		_isExtended = state;
		_renderer.sprite = _isExtended ? extended : retracted;
		_collider.enabled = _isExtended;
	}

	private bool CalcTimedExtendedState() {
		return MathUtil.FloorMod(LevelTime.time - extendOffset, extendTime + retractTime) < extendTime;
	}

	public override void SaveState() {
	}

	public override void ResetState() {
	}
}
