using UnityEngine;

public class LetterEController : MonoBehaviour {

    private SpriteRenderer _renderer;
    
    // Start is called before the first frame update
    private void Start() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchColor() {
        _renderer.color =  new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );
    }
}
