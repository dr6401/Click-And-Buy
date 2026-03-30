using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public bool hasPlayerSeenIntro;
    public bool hasPlayerCompletedFTUE;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public int targetFPS;
    public bool screenShakeEnabled;
    public float lastCameraZoom;
    private SaveData data;
    
    public static SettingsManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        LoadSettings();
    }
    
    public void SaveSettings()
    {
        data = SaveSystem.Load();
        data.settingsData.hasPlayerSeenIntro = hasPlayerSeenIntro;
        data.settingsData.hasPlayerCompletedFTUE = hasPlayerCompletedFTUE;
        data.settingsData.masterVolume = masterVolume;
        data.settingsData.musicVolume = musicVolume;
        data.settingsData.sfxVolume = sfxVolume;
        data.settingsData.targetFPS = targetFPS;
        data.settingsData.screenShakeEnabled = screenShakeEnabled;
        data.settingsData.lastCameraZoom = lastCameraZoom;
        Debug.Log($"Saving settings data");
        SaveSystem.Save(data);
    }

    private void LoadSettings()
    {
        data = SaveSystem.Load();
        hasPlayerSeenIntro = data.settingsData.hasPlayerSeenIntro;
        hasPlayerCompletedFTUE = data.settingsData.hasPlayerCompletedFTUE;
        masterVolume = data.settingsData.masterVolume;
        musicVolume = data.settingsData.musicVolume;
        sfxVolume = data.settingsData.sfxVolume;
        targetFPS = data.settingsData.targetFPS;
        screenShakeEnabled = data.settingsData.screenShakeEnabled;
        lastCameraZoom = data.settingsData.lastCameraZoom;
    }

    private void SetLastCameraZoom(float lastZoom)
    {
        lastCameraZoom = lastZoom;
        //Debug.Log($"Updated lastCameraZoom to: {lastCameraZoom}");
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }

    private void OnEnable()
    {
    }
    
    private void OnDisable()
    {
    }
}
