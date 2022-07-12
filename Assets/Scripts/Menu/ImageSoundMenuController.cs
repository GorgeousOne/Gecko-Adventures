using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSoundMenuController : MonoBehaviour {

    [SerializeField] private GameObject canvas;

    private GameObject _intro;
    private GameObject _blackImage;
    private GameObject _blackImageAudio;
    private AudioSource[] _listOfMenuAudios;

    private GameObject _otherBlackImage;

    // Start is called before the first frame update
    void Start() {
        _listOfMenuAudios = GetComponentsInChildren<AudioSource>(true);
        // background audio
        _listOfMenuAudios[0].enabled = true;
        _intro = canvas.transform.GetChild(3).gameObject;
        _blackImage = _intro.transform.GetChild(6).gameObject;
        _blackImageAudio = _blackImage.transform.GetChild(0).gameObject;

        _otherBlackImage = canvas.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (_blackImageAudio.activeSelf) {
            _listOfMenuAudios[0].enabled = false;
            _otherBlackImage.SetActive(true);
        }

    }
}
