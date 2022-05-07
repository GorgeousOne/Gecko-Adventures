using UnityEngine;
using UnityEngine.Events;

public class GroundSwitchController : Interactable {

    [SerializeField] private Vector2 openOffset;
	[SerializeField] private float moveTime;
    
    private SpriteRenderer _renderer;

    private bool _isEnabled;
    private bool enableMovement = false;
    
    private Vector2 _startPos;
    
    private float _moveStart;
    
    public SwitchToggleEvent toggleAction;

    // Start is called before the first frame update
    private void Start() {
        _renderer = GetComponent<SpriteRenderer>();
		interactAction.AddListener(ToggleSwitch);
        _startPos = transform.position;
    }

    public void ToggleSwitch() {
        enableMovement = !enableMovement;
        Vector2 startPos = _startPos;
        Vector2 targetPos = _startPos;

        if (!enableMovement) {
			targetPos += openOffset;
		}
		else {
			startPos += openOffset;
		}

		float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
		transform.position = Vector2.Lerp(startPos, targetPos, 0.01f);
        
        _isEnabled = enableMovement;
		toggleAction.Invoke(_isEnabled);
    }
}