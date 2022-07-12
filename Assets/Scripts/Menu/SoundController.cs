using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SoundController : MonoBehaviour{

    [FormerlySerializedAs("VolumeSlider")] [SerializeField] private Slider volumeSlider;

    // Start is called before the first frame update
    private void Start() {
        if(!PlayerPrefs.HasKey("volume")) {
            PlayerPrefs.SetFloat("volume", 1);
        }
        LoadVolumeSetting();
    }

    public void ChangeVolume() {
        AudioListener.volume = volumeSlider.value;
        SaveVolumeSetting();
    }

    private void LoadVolumeSetting() {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    private void SaveVolumeSetting() {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
   
}
