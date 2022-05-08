using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileTrapController : MonoBehaviour {

	[FormerlySerializedAs("arrowCount")] [SerializeField] private int projectileCount = 1;
	[SerializeField] private GameObject projectileType;
	[SerializeField] private float projectileSpeed = 30;

	public void Shoot() {
		if (projectileCount > 0) {
			GameObject projectile = Instantiate(projectileType, transform.position, Quaternion.identity);
			Quaternion shootRotation = transform.rotation;

			if (transform.localScale.x < 0) {
				shootRotation *= Quaternion.Euler(0, 0, 180);
			}
			// projectile.transform.rotation = shootRotation;
			projectile.GetComponent<Rigidbody2D>().velocity = shootRotation * Vector2.right * projectileSpeed;
			projectileCount -= 1;
		}
	}
}
