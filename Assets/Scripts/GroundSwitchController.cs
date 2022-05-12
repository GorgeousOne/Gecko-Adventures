using UnityEngine;
using UnityEngine.Events;

public class GroundSwitchController : MonoBehaviour {

    [SerializeField] private Vector2 openOffset;
	[SerializeField] private float moveTime;
    
    private bool _isEnabled;
    private Vector2 _startPos;
    private float _moveStart;
    public SwitchToggleEvent toggleAction;

    // Start is called before the first frame update
    private void Start() {
		//interactAction.AddListener(ToggleSwitch);
        _startPos = transform.position;
    }

    public void Update() {
        Vector2 startPos = _startPos;
        Vector2 targetPos = _startPos;

        if (_isEnabled) {
			targetPos += openOffset;
		}
		else {
			startPos += openOffset;
		}

        float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
		transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);
        
		//toggleAction.Invoke(_isEnabled);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            collider.transform.parent = transform;
            _isEnabled = true;
            toggleAction.Invoke(_isEnabled);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            collider.transform.parent = null;
            _isEnabled = false;
            toggleAction.Invoke(_isEnabled);
        }
    }
}