using UnityEngine;
using Random = UnityEngine.Random;

public class FireflyController : MonoBehaviour, Resettable {

    private Animator _animator;
    private Vector3 _savedPos;
    private bool _savedWasActive;

    private void Start() {
        GetComponentInChildren<Animator>().SetFloat("Offset", Random.Range(0f, 1f));
        SaveState();    
    }

    public void SaveState() {
        _savedWasActive = gameObject.activeSelf;
        _savedPos = transform.position;
    }

    public void ResetState() {
        gameObject.SetActive(_savedWasActive);
        transform.position = _savedPos;
    }
}
