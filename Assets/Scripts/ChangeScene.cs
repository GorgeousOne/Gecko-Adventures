using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : Interactable {

    [SerializeField] private string sceneName;

    protected override void OnInteract() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetFloat("Player_X_" + currentSceneName, transform.position.x);
        PlayerPrefs.SetFloat("Player_Y_" + currentSceneName, transform.position.y);
        PlayerPrefs.SetFloat("Player_Z_" + currentSceneName, transform.position.z);
        LevelLoader.Instance.LoadLevel(sceneName);
    }

    public override void SaveState() {
    }

    public override void ResetState() {
    }
}
