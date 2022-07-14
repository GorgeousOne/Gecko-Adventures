using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : Interactable {

    [SerializeField] private string sceneName;
    [SerializeField] private bool triggerOnEnter;
    
    protected override void OnInteract() {
        LevelLoader.Instance.LoadLevel(sceneName);
    }
    
    protected new void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (triggerOnEnter) {
                OnInteract();
            } else {
                base.OnTriggerEnter2D(other);
            }
        }
    }
    public new void SaveState() {
    }

    public new void ResetState() {
    }
}
