using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : Interactable { 

    [SerializeField] private string currentSceneName;
    [SerializeField] private string nextSceneName;
    public GameObject player;

    protected override void OnInteract() {

        // Vector3 pos = transform.position;

        PlayerPrefs.SetFloat("Player_X_" + currentSceneName, player.transform.position.x);
        PlayerPrefs.SetFloat("Player_Y_" + currentSceneName, player.transform.position.y);
        PlayerPrefs.SetFloat("Player_Z_" + currentSceneName, player.transform.position.z);

        Debug.Log("saved coordinates of the player: \n");
        Debug.Log(player.transform.position);

        SceneManager.LoadScene(nextSceneName);
        
    }
}
