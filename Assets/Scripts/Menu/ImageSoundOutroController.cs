using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSoundOutroController : MonoBehaviour {

    [SerializeField] private GameObject canvas;

    private GameObject _outro;
    private GameObject _comicTextMuseum;
    private GameObject _continueBlack;
    private GameObject _continueWhite;
    private GameObject _jungleBackground;
    private GameObject _museumBackground;
    private GameObject _credits;
    private GameObject _blackBackground;

    private GameObject _blackImage;
    private AudioSource[] _listOfOutroAudios;

    private GameObject _otherBlackImage;

    // Start is called before the first frame update
    void Start() {
        _listOfOutroAudios = GetComponentsInChildren<AudioSource>(true);
        // background audio 1 (jungle)
        _listOfOutroAudios[0].enabled = true;
        // background audio 2 (jungle)
        _listOfOutroAudios[1].enabled = true;

        _jungleBackground = canvas.transform.GetChild(0).gameObject;
        _museumBackground = canvas.transform.GetChild(1).gameObject;
        _blackBackground = canvas.transform.GetChild(2).gameObject;
        _outro = canvas.transform.GetChild(4).gameObject;
        _credits = canvas.transform.GetChild(5).gameObject;
        _continueBlack = _outro.transform.GetChild(1).gameObject;
        _continueWhite = _outro.transform.GetChild(2).gameObject;
        _comicTextMuseum = _outro.transform.GetChild(8).gameObject;
        _blackImage = _outro.transform.GetChild(13).gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (_comicTextMuseum.activeSelf) {
            _listOfOutroAudios[0].enabled = false;
            _listOfOutroAudios[1].enabled = false;
            _jungleBackground.SetActive(false);
            _museumBackground.SetActive(true);
            _continueBlack.SetActive(false);
            _continueWhite.SetActive(true);
        }

        if (_credits.activeSelf) {
            _museumBackground.SetActive(false);
            _blackBackground.SetActive(true);
            _continueWhite.SetActive(false);
        }

    }
}
