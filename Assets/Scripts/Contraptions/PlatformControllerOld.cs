using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControllerOld : MonoBehaviour {

	[SerializeField] private Vector2 offset;
    [SerializeField] private float moveTime = 2;
    [SerializeField] private float delayTime = 2;

    private Vector2 _startPos;
    private bool _isOnBottom = true;
    private float _moveStart;

    // Start is called before the first frame update
    void Start() {

        _startPos = transform.position;
        StartCoroutine("Waiter");
    }

    // Update is called once per frame
    void Update() {    

        Move();
    }

    void Move() {

        Vector2 startPos = _startPos;
		Vector2 targetPos = _startPos;

        if (_isOnBottom) {
            targetPos += offset;
        }
        else {
            startPos += offset;
        }

        float elapsedTime = Mathf.Clamp(Time.time - _moveStart, 0, moveTime);
        transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);

    }

    IEnumerator Waiter() {
        
        // if (_isOnBottom) {
        //     Debug.Log("bottom");
        // }
        // else {
        //     Debug.Log("on top");
        // }

        yield return new WaitForSeconds(moveTime + delayTime);
        _isOnBottom = !_isOnBottom;
        _moveStart = Time.time;
        StartCoroutine("Waiter");
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            collider.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            collider.transform.parent = null;
        }
    }

    private void OnDrawGizmos() {
		Vector2 position = _startPos != Vector2.zero ? _startPos : transform.position;
        Gizmos.DrawLine(position, position + offset);
	}

}
