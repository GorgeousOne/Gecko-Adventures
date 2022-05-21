using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPreviousScene : Interactable {

    [SerializeField] private string previousSceneName;
    [SerializeField] private string currentSceneName;
    public GameObject player;

    protected override void OnInteract() {

        Vector3 savedPos = Vector3.zero;

        savedPos.x = PlayerPrefs.GetFloat("Player_X_" + previousSceneName);
        savedPos.y = PlayerPrefs.GetFloat("Player_Y_" + previousSceneName);
        savedPos.z = PlayerPrefs.GetFloat("Player_Z_" + previousSceneName);

        // player.transform.position = savedPos;

        // Debug.Log("after loading: \n");
        // Debug.Log(player.transform.position);

        SceneManager.LoadScene(previousSceneName);
        // SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(previousSceneName));

        // GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];

        player.transform.position = savedPos;

        Debug.Log("loaded coordinates of the player: \n");
        Debug.Log(player.transform.position);
        
    }

}
