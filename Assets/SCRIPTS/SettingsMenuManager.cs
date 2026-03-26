using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sFXVolumeSlider;
    [SerializeField] private AudioMixer mixer;   
    void Start()
    {
        if (mixer.GetFloat("MasterVolume", out float masterDb))
        {
            masterVolumeSlider.value = Mathf.Pow(10f, masterDb / 20f);
        }
        if (mixer.GetFloat("MusicVolume", out float musicDb))
        {
            musicVolumeSlider.value = Mathf.Pow(10f, musicDb / 20f);
        }
        if (mixer.GetFloat("SFXVolume", out float sfxDb))
        {
            sFXVolumeSlider.value = Mathf.Pow(10f, sfxDb / 20f);
        }

    }
    public void SetMasterVolume()
    {
        float value = masterVolumeSlider.value;
        float db = Mathf.Log10(value) * 20f;

        if (value <= 0.001f)
        {
            mixer.SetFloat("MasterVolume", -80f);    
        }
        else
        {
            mixer.SetFloat("MasterVolume", db);   
        }
        SettingsManager.Instance.masterVolume = value;
    }
    public void SetMusicVolume()
    {
        float value = musicVolumeSlider.value;
        float db = Mathf.Log10(value) * 20f;

        if (value <= 0.001f)
        {
            mixer.SetFloat("MusicVolume", -80f);    
        }
        else
        {
            mixer.SetFloat("MusicVolume", db);    
        }
        SettingsManager.Instance.musicVolume = value;
    }
    
    public void SetSfxVolume()
    {
        float value = sFXVolumeSlider.value;
        float db = Mathf.Log10(value) * 20f;

        if (value <= 0.001f)
        {
            mixer.SetFloat("SFXVolume", -80f);    
        }
        else
        {
            mixer.SetFloat("SFXVolume", db);   
        }
        SettingsManager.Instance.sfxVolume = value;
    }
}
