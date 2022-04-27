using UnityEngine;

public class WallSwitchController : MonoBehaviour {

    private SpriteRenderer _renderer;
    
    public GameObject objectToMove;
    public Transform initialPosition;
    public Transform targetPosition;
    private Vector3 oldPosition;
    public float movingSpeed;
    private int counter = 0;
    private bool up;
    private bool down;
    
    // Start is called before the first frame update
    private void Start() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void setOldGameobjectPosition(Vector3 position) {
        oldPosition = position;
    }

    public Vector3 getOldGameobjectPosition() {
        return objectToMove.transform.position;
    }

    public void EnableWallSwitch() {

        counter += 1;

        if (counter % 2 != 0) {
            _renderer.flipY = true;
            up = true;
            down = false;
            
        }
        else {
            _renderer.flipY = false;
            up = false;
            down = true;
        }
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

        if (up == true) {
            MoveDoorUp();
        }

        //up = false;
    
        if (down == true) {
            MoveDoorDown();
        }

        //down = false;
        
    }
}