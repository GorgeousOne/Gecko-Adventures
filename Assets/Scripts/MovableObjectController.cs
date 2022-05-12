using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectController : MonoBehaviour {

	[SerializeField] private Vector2 offSet;
    [SerializeField] private float moveTime = 2;
    [SerializeField] private float delayTime = 2;

    private Vector2 _currentPos;
    private bool _isOnBottom = true;
    private float _moveStart;

    // Start is called before the first frame update
    void Start() {

        _currentPos = transform.position;
        StartCoroutine("Waiter");
    }

    // Update is called once per frame
    void Update() {    

        Move();
    }

    void Move() {

        Vector2 startPos = _currentPos;
		Vector2 targetPos = _currentPos;

        if (_isOnBottom) {
            targetPos += offSet;
        }
        else {
            startPos += offSet;
        }

        float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
        transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);

    }

    IEnumerator Waiter() {
        
        if (_isOnBottom) {
            Debug.Log("bottom");
        }
        else {
            Debug.Log("on top");
        }

        yield return new WaitForSeconds(moveTime + delayTime);
        _isOnBottom = !_isOnBottom;
        _moveStart = Time.time;
        StartCoroutine("Waiter");
    }
}
