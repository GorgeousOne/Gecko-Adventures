using UnityEngine;

public class WallSwitchController : MonoBehaviour {

    private SpriteRenderer _renderer;
    
    public GameObject objectToMove;
    public Transform initialPosition;
    public Transform targetPosition;
    private Vector3 oldPosition;
    public float movingSpeed;
    private bool switchEnabled;
    
    // Start is called before the first frame update
    private void Start() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void EnableWallSwitch() {

        _renderer.flipY = !_renderer.flipY;
        switchEnabled = _renderer.flipY;
    }

    public void MoveDoorUp() {

        Vector3 a = objectToMove.transform.position;
        Vector3 b = targetPosition.position;
        objectToMove.transform.position = Vector3.Lerp(a, b, movingSpeed);
    }

    public void MoveDoorDown() {

        Vector3 a = objectToMove.transform.position;
        Vector3 b = initialPosition.position;
        objectToMove.transform.position = Vector3.Lerp(a, b, movingSpeed);
    }

    public void Update() {

        if (switchEnabled) {
            MoveDoorUp();
        }
    
        else {
            MoveDoorDown();
        }
    }
}