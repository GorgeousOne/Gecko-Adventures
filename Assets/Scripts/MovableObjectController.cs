using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectController : MonoBehaviour
{

	[SerializeField] public Vector2 offSet;
    [SerializeField] private float moveTime;

    private Vector2 currentPos;
    private bool isOnBottom = true;
    private float _moveStart;

    // Start is called before the first frame update
    void Start() {

        currentPos = transform.position;
        StartCoroutine("Waiter");
    }

    // Update is called once per frame
    void Update() {    

        Move();
    }

    void Move() {

        Vector2 startPos = currentPos;
		Vector2 targetPos = currentPos;

        if (isOnBottom) {
            targetPos += offSet;
        }

        else {
            startPos += offSet;
        }

        float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
        transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);

    }

    IEnumerator Waiter() {
        if (!isOnBottom) {
            Debug.Log("bottom");
        }

        else {
            Debug.Log("on top");
        }

        yield return new WaitForSeconds(4);
        isOnBottom = !isOnBottom;

        StartCoroutine("Waiter");
    }


 
}
