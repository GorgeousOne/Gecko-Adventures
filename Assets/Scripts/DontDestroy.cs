using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    [HideInInspector] public string objectID;

    private void Awake() {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < Object.FindObjectsOfType<DontDestroy>().Length; i++) {

            if (Object.FindObjectsOfType<DontDestroy>()[i] != this) {
               
                if (Object.FindObjectsOfType<DontDestroy>()[i].objectID == objectID) {
                
                    Destroy(gameObject);
                }
            }
        }        

        DontDestroyOnLoad(gameObject);
    }

    // void Start() {

    //     GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    //     int id = players[0].GetInstanceID();

    //     for (int i = 0; i < players.Length; i++)
    //     {
    //         if (players[i].GetInstanceID() != id) {
    //             Destroy(this.gameObject);
    //         }
    //         else {
    //             DontDestroyOnLoad(this.gameObject);
    //         }
    //     }
    // }

}
