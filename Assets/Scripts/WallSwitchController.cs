using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SwitchToggleEvent : UnityEvent<bool> {}

public class WallSwitchController : Interactable {

    private SpriteRenderer _renderer;
    private bool _isEnabled;
    
    public SwitchToggleEvent toggleAction;

    // Start is called before the first frame update
    private void Start() {
        _renderer = GetComponent<SpriteRenderer>();
		interactAction.AddListener(ToggleSwitch);
    }

    public void ToggleSwitch() {
        _renderer.flipY = !_renderer.flipY;
        _isEnabled = _renderer.flipY;
		toggleAction.Invoke(_isEnabled);
    }
}