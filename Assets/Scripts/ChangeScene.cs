using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : Interactable {

    [SerializeField] private string sceneName;
    // [SerializeField] private int currentSceneNumber;
    // [SerializeField] private int sceneNumberToLoad;
    // private Vector3 positionPreviousScene;
    // [SerializeField] private GameObject player;

    // void Awake() {
    //     DontDestroyOnLoad(player);
    // }

    protected override void OnInteract() {

        // positionPreviousScene = transform.position;
        // DontDestroyOnLoad(this.player);
        SceneManager.LoadScene(sceneName);

        // if (currentSceneNumber < sceneNumberToLoad) {
        //     LoadNextScene()
        // }
        // else {
        //     LoadPreviousScene()
        // }
        
    }

    // private void LoadNextScene() {
    //     SceneManager.LoadScene(sceneName);
    // }

    // private void LoadPreviousScene() {
        
    // }
}
