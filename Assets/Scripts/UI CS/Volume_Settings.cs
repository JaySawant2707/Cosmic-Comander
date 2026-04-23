using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Load saved values (default = 1)
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Set sliders without triggering listeners
        masterSlider.SetValueWithoutNotify(master);
        musicSlider.SetValueWithoutNotify(music);
        sfxSlider.SetValueWithoutNotify(sfx);

        // Apply to AudioManager
        AudioManager.instance.SetMasterVolume(master);
        AudioManager.instance.SetMusicVolume(music);
        AudioManager.instance.SetSFXVolume(sfx);

        // Add listeners
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetMasterVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}