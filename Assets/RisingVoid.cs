using System;
using UnityEngine;

public class RisingVoid : Resettable {

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerOffsetY = 10;

    private Vector3 _savedPos;

    private void Start() {
        SaveState();
    }

    void Update() {
        Vector2 playerPos = playerTransform.position;
        Vector2 pos = transform.position;
        
        if (playerPos.y - pos.y > playerOffsetY) {
            pos.y = playerPos.y - playerOffsetY;
            transform.position = pos;
        }
    }

    public override void SaveState() {
        _savedPos = transform.position;
    }

    public override void ResetState() {
        transform.position = _savedPos;
    }
}
