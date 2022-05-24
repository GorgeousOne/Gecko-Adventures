using UnityEngine;

public class Checkpoint : MonoBehaviour {

	private LevelCheckpoints _level;
	private Color _gizmoColor = CheckpointColor.unreached();
	
	public void SetGame(LevelCheckpoints level) {
		_level = level;
	}

	public Vector2 GetSpawnPoint() {
		return transform.Find("SpawnPoint").position;
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		_level.OnCheckpointReach(this);
		_gizmoColor = CheckpointColor.reached();
	}
	
	private void OnDrawGizmos() {
		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		Transform spawn = transform.Find("SpawnPoint");
		
		Gizmos.color = _gizmoColor;
		Gizmos.DrawCube(collider.transform.position + (Vector3) collider.offset, collider.size);
		Gizmos.DrawIcon(spawn.position, "sv_icon_dot9_pix16_gizmo.png", true);
	}
}

public static class CheckpointColor {
	public static Color unreached() {return new Color(0, 0, 1, .2f);}
	public static Color reached() {return new Color(0, 1, 0, .2f);}
}