using UnityEngine;

public class ArrowController : MonoBehaviour {

	[SerializeField] [Min(0f)] private float despawnRate = 10;
	
	private Rigidbody2D _rigid;
	
	private void Start() {
		_rigid = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		// transform.rotation = Quaternion.LookRotation(_rigid.velocity, Vector2.up);
		float rotation = Mathf.Atan2(_rigid.velocity.y, _rigid.velocity.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
	}

	private void OnTriggerExit2D(Collider2D other) {
		GetComponent<Collider2D>().isTrigger = false;
	}

	private void OnCollisionEnter2D(Collision2D other) {
		_rigid.velocity = Vector2.zero;
		_rigid.isKinematic = true;
		Destroy(GetComponent<Collider2D>());
		Destroy(gameObject, despawnRate);
		// transform.parent = other.transform;
	}
}
