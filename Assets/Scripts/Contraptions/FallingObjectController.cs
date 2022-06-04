using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectController : Triggerable {

    [SerializeField] public GameObject fallingObject;

    private bool _isEnabled;
    private Transform _startTransform;
    private GameObject _newFallingObject;

    private void Start() {
        _startTransform = fallingObject.transform;
    }

    public override void OnSwitchToggle(bool isEnabled) {
        if (_newFallingObject != null) {
            Destroy(_newFallingObject);
        }

        if (isEnabled) {
            _newFallingObject = Instantiate(fallingObject, transform.position, Quaternion.identity);
        }
	}

    private void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "sv_icon_dot14_pix16_gizmo.png", true);
	}

    public override void SaveState() {
		
	}

	public override void ResetState() {
		if (_newFallingObject != null) {
            Destroy(_newFallingObject);
        }
	}

}
