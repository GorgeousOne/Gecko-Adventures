using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileTrapController : Triggerable, Resettable {
	
	[Header("Shooting")]
	[SerializeField] private GameObject projectileType;
	[SerializeField] private float projectileSpeed = 30;

	[Header("Timing")] 
	[SerializeField] private bool timedShootingEnabled = false;
	[SerializeField] [Min(0)] private float shootOffset;
	[SerializeField] [Min(.5f)] private float reloadTime = 2;

	private float _lastTimedShot;
	private int _savedProjectileCount;
	private float _savedLastTimeShot;
	
	private void Start() {
		_lastTimedShot = shootOffset - reloadTime;
		SaveState();
	}

	private void Update() {
		if (timedShootingEnabled && ReloadTimePassed()) {
			Shoot();
			_lastTimedShot += reloadTime;
		}
	}

	public override void OnSwitchToggle(bool isEnabled) {
		if (isEnabled) {
			Shoot();
		}
	}
	
	public void Shoot() {
		GameObject projectile = Instantiate(projectileType, transform.position, Quaternion.identity);
		Quaternion shootRotation = transform.rotation;

		if (transform.localScale.x < 0) {
			shootRotation *= Quaternion.Euler(0, 0, 180);
		}
		projectile.GetComponent<Rigidbody2D>().velocity = shootRotation * Vector2.right * projectileSpeed;
	}
	
	private bool ReloadTimePassed() {
		return LevelTime.time - _lastTimedShot >= reloadTime;
	}
	
	public void SaveState() {
		_savedLastTimeShot = _lastTimedShot;
	}

	public void ResetState() {
		_lastTimedShot = _savedLastTimeShot;
	}
}
