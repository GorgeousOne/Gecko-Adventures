using UnityEngine;

public class FireflyController : MonoBehaviour {

    private Animator _animator;
    
    private void Start() {
        _animator = GetComponent<Animator>();
        _animator.SetFloat("Offset", Random.Range(0f, 1f));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(transform.parent.gameObject);
    }
}
