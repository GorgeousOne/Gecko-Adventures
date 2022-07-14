using UnityEngine;

public class ArrowController : MonoBehaviour, Resettable {

	[SerializeField] [Min(0f)] private float despawnRate = 5;
	
	private Rigidbody2D _rigid;
	private bool _isStuck;
	
	private void Start() {
		_rigid = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		if (!_isStuck) {
			float rotation = Mathf.Atan2(_rigid.velocity.y, _rigid.velocity.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		GetComponent<Collider2D>().isTrigger = false;
	}

	private void OnCollisionEnter2D(Collision2D other) {
		transform.parent = other.transform;
		_rigid.velocity = Vector2.zero;
		_rigid.isKinematic = true;
		_isStuck = true;
		Destroy(GetComponent<Collider2D>());
		Destroy(gameObject, despawnRate);
		// transform.parent = other.transform;
	}

	public  void SaveState() {}

	public void ResetState() {
		Destroy(gameObject);
	}
}
