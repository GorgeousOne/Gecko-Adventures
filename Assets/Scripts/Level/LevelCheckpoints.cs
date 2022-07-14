using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelCheckpoints : MonoBehaviour {

	private List<Checkpoint> _checkpoints;
	private Checkpoint _currentCheckpoint;
	private float _currentCheckpointTime;
	
	private void OnEnable() {
		_checkpoints = new List<Checkpoint>();
		
		foreach(Transform checkpointTransform in transform) {
			Checkpoint checkpoint = checkpointTransform.gameObject.GetComponent<Checkpoint>();
			checkpoint.SetGame(this);
			_checkpoints.Add(checkpoint);
			
			if (_currentCheckpoint == null) {
				_currentCheckpoint = checkpoint;
			}
		}
		if (!_checkpoints.Any()) {
			Debug.LogWarning("No children of type Checkpoint found. Please add at least 1 Checkpoint prefab as a child as the level start position.");			
		}
	}

	public Vector2 GetCurrentSpawnPoint() {
		return _currentCheckpoint ? _currentCheckpoint.GetSpawnPoint() : Vector2.zero;
	}

	public void ResetToLastCheckpoint() {
		LevelTime.SetTime(_currentCheckpointTime);
		
		foreach (Resettable resettable in FindObjectsOfType<MonoBehaviour>(true).OfType<Resettable>()){
			resettable.ResetState();
		}
	}
	
	public void OnCheckpointReach(Checkpoint checkpoint) {
		if (_checkpoints.IndexOf(checkpoint) > _checkpoints.IndexOf(_currentCheckpoint)) {
			_currentCheckpoint = checkpoint;
			_currentCheckpointTime = LevelTime.time;
			
			foreach (Resettable resettable in FindObjectsOfType<MonoBehaviour>(true).OfType<Resettable>()){
				resettable.SaveState();
			}
		}
	}
}