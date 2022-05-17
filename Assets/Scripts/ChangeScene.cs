using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : Interactable {

    [SerializeField] private string sceneName;

    protected override void OnInteract() {
        SceneManager.LoadScene(sceneName);
    }
}
