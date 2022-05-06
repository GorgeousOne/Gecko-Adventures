using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ArrowTrapController : MonoBehaviour {

	[FormerlySerializedAs("arrowCount")] [SerializeField] private int projectileCount = 1;
	[SerializeField] private GameObject projectileType;
	[SerializeField] private float projectileSpeed = 30;

	private SpriteRenderer _renderer;
	
	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
	}

	public void Shoot() {
		if (projectileCount > 0) {
			GameObject projectile = Instantiate(projectileType, transform.position, Quaternion.identity);
			Quaternion shootRotation = transform.rotation;

			if (_renderer.flipX) {
				shootRotation *= Quaternion.Euler(0, 0, 180);
			}
			projectile.transform.rotation = shootRotation;
			projectile.GetComponent<Rigidbody2D>().velocity = shootRotation * Vector2.right * projectileSpeed;
			projectileCount -= 1;
		}
	}
}
