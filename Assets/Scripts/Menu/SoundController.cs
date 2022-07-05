using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour{

    [SerializeField] Slider VolumeSlider;

    // Start is called before the first frame update
    void Start() {
        if(!PlayerPrefs.HasKey("volume")) {
            PlayerPrefs.SetFloat("volume", 1);
            Load();
        }
        else {
            Load();
        }
        
    }

    public void ChangeVolume() {
        AudioListener.volume = VolumeSlider.value;
        Save();
    }

    private void Load() {
        VolumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    private void Save() {
        PlayerPrefs.SetFloat("volume", VolumeSlider.value);
    }
   
}
