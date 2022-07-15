using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectController : Triggerable, Resettable {

    [SerializeField] public GameObject fallingObject;
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private float reloadTime = 5;

    // private bool _isEnabled;
    private Transform _startTransform;
    private GameObject _newFallingObject;
    float _lastTimedObject;

    private void Start() {

        _startTransform = fallingObject.transform;
    }

    private void Update() {
        if (isEnabled) {
            if (PassedMovementTime()) {
                InstantiateObject(isEnabled);
                _lastTimedObject += reloadTime;
            }
        }
    }

    private void InstantiateObject(bool isEnabled) {
        if (_newFallingObject != null) {
            Destroy(_newFallingObject);
        }

        if (isEnabled) {
            _newFallingObject = Instantiate(fallingObject, transform.position, Quaternion.identity);
        }
    }

    public override void OnSwitchToggle(bool isEnabled) {
        InstantiateObject(isEnabled);
	}

    private bool PassedMovementTime() {
		return LevelTime.time - _lastTimedObject > reloadTime;
	}

    private void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "sv_icon_dot14_pix16_gizmo.png", true);
	}

    public void SaveState() {
		
	}

	public void ResetState() {
		if (_newFallingObject != null) {
            Destroy(_newFallingObject);
        }
	}

}
