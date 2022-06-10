using UnityEngine;
using Random = UnityEngine.Random;

public class FireflyController : Resettable {

    private Animator _animator;
    private Vector3 _savedPos;
    private bool _savedWasActive;

    private void Start() {
        GetComponentInChildren<Animator>().SetFloat("Offset", Random.Range(0f, 1f));
        SaveState();    
    }

    public override void SaveState() {
        _savedWasActive = gameObject.activeSelf;
        _savedPos = transform.position;
    }

    public override void ResetState() {
        gameObject.SetActive(_savedWasActive);
        transform.position = _savedPos;
    }
}
