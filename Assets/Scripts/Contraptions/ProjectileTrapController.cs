using UnityEngine;

public class ProjectileTrapController : Triggerable, IResettable {
	
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

	protected override void OnToggle(bool isEnabled) {
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
	
	public new void SaveState() {
		base.SaveState();
		_savedLastTimeShot = _lastTimedShot;
	}

	public new void ResetState() {
		base.ResetState();
		_lastTimedShot = _savedLastTimeShot;
	}
}
