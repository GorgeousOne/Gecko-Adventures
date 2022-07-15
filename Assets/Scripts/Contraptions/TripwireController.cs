using UnityEngine;

public class TripwireController : Trigger, Resettable {

	private LineRenderer _line;
	private bool _savedIsIntact;
	
	private void Start() {
		_line = GetComponent<LineRenderer>();
		_CreateCollider();
		SaveState();
	}

	private void _CreateCollider() {
		Vector3[] linePoints = new Vector3[_line.positionCount];
		_line.GetPositions(linePoints);

		Vector3 start = linePoints[0];
		Vector3 end = linePoints[1];
		Vector3 distance = end - start;

		Vector3 colliderOffset = start - transform.position + .5f * distance;
		float colliderAngle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
		
		BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
		collider.offset = Quaternion.AngleAxis(-colliderAngle, Vector3.forward) * colliderOffset;
		collider.size = new Vector2(distance.magnitude, _line.startWidth);
		collider.isTrigger = true;
		transform.rotation = Quaternion.Euler(0, 0,  colliderAngle);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			Activate();
			gameObject.SetActive(false);
		}
	}

	public void SaveState() {
		_savedIsIntact = gameObject.activeSelf;
	}

	public void ResetState() {
		gameObject.SetActive(_savedIsIntact);
	}
}
