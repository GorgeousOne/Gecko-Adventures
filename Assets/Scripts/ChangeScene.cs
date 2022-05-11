using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    [SerializeField] public string scenename;

    void Start() {
        Debug.Log("Load Scene");
    }

    public void ChangeTheScene() {
        Debug.Log("loading scene " + scenename);
        SceneManager.LoadScene(scenename);
    }
}
