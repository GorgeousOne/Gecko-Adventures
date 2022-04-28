using UnityEngine;

public class DestroyFirefly : MonoBehaviour
{

    public GameObject objectToDestroy;


    private void OnCollisionEnter2D(Collision2D collision) {

        Destroy(objectToDestroy);
    }
}
