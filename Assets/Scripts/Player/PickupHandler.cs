using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PickupHandler : MonoBehaviour {

	[SerializeField] private float maxLightIntensity = 2f;
	[SerializeField] private float maxLightTime = 30f;
	[SerializeField] private float spawnLightTime = 15f;
	[SerializeField] private float lightSourceTime = 15f;
	
	private bool _isDarkLevel;
	private PlayerSpawning _playerSpawning;
	private Light2D _playerLight;
	private float _remainingLightTime;
	
	private void OnEnable() {
		_isDarkLevel = FindGlobalLight().intensity < .2f;
		
		if (_isDarkLevel) {
			_playerLight = GetComponent<Light2D>();
			_playerLight.enabled = true;
			_playerSpawning = GetComponent<PlayerSpawning>();
			_playerSpawning.playerSpawnEvent.AddListener(OnPlayerSpawn);
			_remainingLightTime = spawnLightTime;
		}
	}

	private void OnDisable() {
		_playerSpawning.playerSpawnEvent.RemoveListener(OnPlayerSpawn);
	}

	private void Update() {
		if (!_isDarkLevel) {
			return;
		}
		if (_remainingLightTime <= 0) {
			if (!_playerSpawning.IsDead()) {
				_playerSpawning.Die();
			}
			return;
		}
		_remainingLightTime = Mathf.Max(0, _remainingLightTime - Time.deltaTime);
		_playerLight.intensity = MathUtil.SquareIn(_remainingLightTime / (maxLightTime * 0.3f)) * maxLightIntensity;
	}

	public void ProcessPickup(GameObject pickup) {
		if (_isDarkLevel && pickup.CompareTag("Light Source")) {
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
