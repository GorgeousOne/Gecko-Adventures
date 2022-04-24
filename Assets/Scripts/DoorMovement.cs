using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{

    public Transform target;
    public float t;

    private SpriteRenderer _renderer;
    
    // Start is called before the first frame update
    private void Start() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //transform.position = target.position;        
        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.Lerp(a, b, t);
    }

    // private void MoveDoor()
    // {
    //     //transform.position = target.position;        
    //     Vector3 a = transform.position;
    //     Vector3 b = target.position;
    //     transform.position = Vector3.Lerp(a, b, t);
    // }
}
