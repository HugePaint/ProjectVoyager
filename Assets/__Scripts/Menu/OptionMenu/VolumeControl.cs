using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;
    public AudioManager audioManager;
    public MenuBgmManager menuBgmManager;

    private const string musicVolumePrefKey = "VolumeMusic";
    private const string soundVolumePrefKey = "VolumeSounds";
    private void Start()
    {
        Global.Misc.soundEffectVolume = PlayerPrefs.HasKey(soundVolumePrefKey) ? PlayerPrefs.GetFloat(soundVolumePrefKey) : 1f;
        Global.Misc.bgmVolume = PlayerPrefs.HasKey(musicVolumePrefKey) ? PlayerPrefs.GetFloat(musicVolumePrefKey) : 1f;
        
        musicSlider.value = Global.Misc.bgmVolume;
        soundSlider.value = Global.Misc.soundEffectVolume;

        // Add the listener to the slider
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundsVolume);
    }

    private void SetMusicVolume(float sliderValue)
    {
        Global.Misc.bgmVolume = sliderValue;
        audioManager.UpdateBgmVolume();
        menuBgmManager.UpdateVolume();
    }
    
    private void SetSoundsVolume(float sliderValue)
    {
        Global.Misc.soundEffectVolume = sliderValue;
    }

    public void SaveVolumePref()
    {
        PlayerPrefs.SetFloat(soundVolumePrefKey, Global.Misc.soundEffectVolume);
        PlayerPrefs.SetFloat(musicVolumePrefKey, Global.Misc.bgmVolume);
    }
}
