using UnityEngine;

public class ColliderDisplay : MonoBehaviour {

	[SerializeField] private Color fillColor = new (.5f, .5f, .5f, -5f);
	
	private void OnDrawGizmos() {
		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		Vector3 size = collider.size;
		size.Scale(transform.localScale);
		
		Gizmos.color = fillColor;
		Gizmos.DrawCube(collider.transform.position + (Vector3) collider.offset, size);
	}
}
