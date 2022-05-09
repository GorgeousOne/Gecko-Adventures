using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpoints : MonoBehaviour {

	private List<Checkpoint> _checkpoints;
	private Checkpoint _currentCheckpoint;
	
	void Awake() {
		_checkpoints = new List<Checkpoint>();
		
		foreach(Transform checkpointTransform in transform) {
			Checkpoint checkpoint = checkpointTransform.gameObject.GetComponent<Checkpoint>();
			checkpoint.SetGame(this);
			_checkpoints.Add(checkpoint);
			
			if (_currentCheckpoint == null) {
				_currentCheckpoint = checkpoint;
			}
		}
	}

	public Vector2 GetCurrentSpawnPoint() {
		return _currentCheckpoint.GetSpawnPoint();
	}
	
	public void OnCheckpointReach(Checkpoint checkpoint) {
		if (_checkpoints.IndexOf(checkpoint) > _checkpoints.IndexOf(_currentCheckpoint)) {
			_currentCheckpoint = checkpoint;
		}
	}
}