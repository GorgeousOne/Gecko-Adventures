using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class PickupHandler : MonoBehaviour {

	[Header("Timing")]
	[SerializeField] private float maxLightTime = 30f;
	[SerializeField] private float spawnLightTime = 15f;
	[SerializeField] private float lightSourceTime = 15f;

	[Header("Light Properties")] 
	[SerializeField] private float fullLightIntensity = 2f;
	[SerializeField] private float fullLightRadius = 10f;
	
	[SerializeField] private float halfLightIntensity = 1.5f;
	[SerializeField] private float halfLightRadius = 5f;
	[SerializeField] private float halfLightIntensityLerpTime = 1f;
	
	[SerializeField] private float minLightRadius = 3f;
	[SerializeField] private float lightDropTime = 5f;

	public UnityEvent maskCollectEvent;
	
	private bool _isDarkLevel;
	private PlayerSpawning _playerSpawning;
	private Light2D _playerLight;
	private float _remainingLightTime;
	
	private void OnEnable() {
		_isDarkLevel = FindGlobalLight().intensity < .2f;
		
		_playerLight = GetComponent<Light2D>();
		_playerLight.enabled = true;
		_playerLight.intensity = 0;
		
		if (_isDarkLevel) {
			_playerSpawning = GetComponent<PlayerSpawning>();
			_playerSpawning.playerSpawnEvent.AddListener(OnPlayerSpawn);
			_remainingLightTime = spawnLightTime;
		}
	}

	private void Update() {
		if (_remainingLightTime <= 0) {
			if (_isDarkLevel && !_playerSpawning.IsDead()) {
				_playerSpawning.Die();
			}
			return;
		}
		_remainingLightTime = Mathf.Max(0, _remainingLightTime - Time.deltaTime);
		UpdateLightSettings();
	}

	private void UpdateLightSettings() {
		float halfTime = .5f * maxLightTime;
		
		if (_remainingLightTime > halfTime) {
			_playerLight.pointLightOuterRadius = fullLightRadius;
			_playerLight.intensity = fullLightIntensity;
			
		} else {
			if (_remainingLightTime > lightDropTime) {
				float lerpPercent = Mathf.InverseLerp(halfTime - halfLightIntensityLerpTime, halfTime, _remainingLightTime);
				_playerLight.pointLightOuterRadius = MathUtil.Map(_remainingLightTime, lightDropTime, halfTime, halfLightRadius, fullLightRadius);
				_playerLight.intensity = Mathf.SmoothStep(halfLightIntensity, fullLightIntensity, lerpPercent);
				
			} else {
				float lightDropPercent = _remainingLightTime / lightDropTime;
				_playerLight.pointLightOuterRadius = Mathf.Lerp(minLightRadius, halfLightRadius, lightDropPercent);
				_playerLight.intensity = lightDropPercent * halfLightIntensity;
			}
		}
	}
	
	public void ProcessPickup(GameObject pickup) {
		if (pickup.CompareTag("Mask")) {
			maskCollectEvent.Invoke();
		} else if (pickup.CompareTag("Light Source")) {
			_remainingLightTime = Mathf.Min(_remainingLightTime + lightSourceTime, maxLightTime);
		}
	}

	private void OnPlayerSpawn() {
		if (_isDarkLevel) {
			_remainingLightTime = spawnLightTime;
		}
	}
	
	private static Light2D FindGlobalLight() {
		foreach (Light2D light2D in FindObjectsOfType<Light2D>()) {
			if (light2D.lightType == Light2D.LightType.Global) {
				return light2D;
			}
		}
		throw new NullReferenceException("Couldn't find global light in scene to determine if this is a dark level :(");
	}
}
