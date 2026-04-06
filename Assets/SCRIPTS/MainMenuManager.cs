using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sFXVolumeSlider;
    [SerializeField] private AudioMixer mixer;
    private bool isHardcore = false;
    private bool followWithCamera = false;
    private bool screenShake = false;
    [SerializeField] private Button anomaliesButton;
    [SerializeField] private GameObject anomaliesButtonClickableText;
    [SerializeField] private GameObject anomaliesButtonUnclickableText;
    [SerializeField] private Image anomaliesButtonImage;
    [SerializeField] private Color endlessButtonLockedColor;
    [SerializeField] private Color endlessButtonUnlockedColor;
    [SerializeField] private GameObject anomaliesLockedImage;
    [SerializeField] private TMP_Text anomaliesTooltipText;
    [SerializeField] private Button idlerButton;
    [SerializeField] private GameObject idlerButtonClickableText;
    [SerializeField] private GameObject idlerButtonUnclickableText;
    [SerializeField] private Image idlerButtonImage;
    [SerializeField] private TMP_Text idlerTooltipText;
    [SerializeField] private GameObject hardcoreEndlessButton;
    [SerializeField] private GameObject followWithCameraCheckBoxImage;
    [SerializeField] private GameObject toggleFPSCheckBoxImage;
    [SerializeField] private GameObject toggleScreenShakeCheckBoxImage;
    [SerializeField] private GameObject areYouSureObject;
    [SerializeField] private GameObject loseNormalEndlessProgressObject;
    [SerializeField] private GameObject loseHardcoreEndlessProgressObject;

    public int targetFps = 60;

    [Header("Menu Transitions")]
    [SerializeField] private MMFeedbacks openMainMenuPanelFeedback;
    [SerializeField] private MMFeedbacks openPlayPanelFeedback;
    [SerializeField] private MMFeedbacks openAnomaliesPanelFeedback;
    [SerializeField] private MMFeedbacks openSettingsPanelFeedback;
    [SerializeField] private MMFeedbacks openCreditsPanelFeedback;
    [SerializeField] private MMFeedbacks closeMainMenuPanelFeedback;
    [SerializeField] private MMFeedbacks closePlayPanelFeedback;
    [SerializeField] private MMFeedbacks closeAnomaliesPanelFeedback;
    [SerializeField] private MMFeedbacks closeSettingsPanelFeedback;
    [SerializeField] private MMFeedbacks closeCreditsPanelFeedback;

    [Header("Scene Loading")]
    [SerializeField] private MMFeedbacks loadLvl1Feedback;
    [SerializeField] private MMFeedbacks loadCutscenesSceneFeedback;

    void Start()
    {
        if (!GameConstants.isPlaytestBuild)
        {
            anomaliesLockedImage.SetActive(true);
            anomaliesButton.interactable = false;
            anomaliesButtonImage.color = endlessButtonLockedColor;
            anomaliesTooltipText.text = "Unlocks after completing Campaign!";
            anomaliesButtonClickableText.SetActive(false);
            anomaliesButtonUnclickableText.SetActive(true);
            hardcoreEndlessButton.SetActive(false);
            
            //idlerLockedImage.SetActive(true);
            idlerButton.interactable = false;
            idlerButtonImage.color = endlessButtonLockedColor;
            idlerTooltipText.text = "Unlocks after completing Campaign!";
            idlerButtonClickableText.SetActive(false);
            idlerButtonUnclickableText.SetActive(true);
        }
        else
        {
            anomaliesLockedImage.SetActive(false);
            anomaliesButton.interactable = true;
            anomaliesButtonImage.color = endlessButtonUnlockedColor;
            anomaliesTooltipText.text = "Anomalies!\nBend space, time\nand rules!";
            anomaliesButtonClickableText.SetActive(true);
            anomaliesButtonUnclickableText.SetActive(false);
            TMP_Text anomaliesButtonText = anomaliesButtonClickableText.GetComponent<TMP_Text>();
            anomaliesButtonText.alignment = TextAlignmentOptions.Center;
            hardcoreEndlessButton.SetActive(true);
            
            //idlerLockedImage.SetActive(false);
            idlerButton.interactable = true;
            idlerButtonImage.color = endlessButtonUnlockedColor;
            idlerTooltipText.text = "No timer, no pressure.\nChill. Munch. Repeat.";
            idlerTooltipText.fontSize = 19.5f;
            idlerButtonClickableText.SetActive(true);
            idlerButtonUnclickableText.SetActive(false);
        }
        Time.timeScale = 1f;
        followWithCameraCheckBoxImage.SetActive(followWithCamera);
        if (SettingsManager.Instance != null)
        {
            if (SettingsManager.Instance.masterVolume <= 0.001f) mixer.SetFloat("MasterVolume", -80f);
            else mixer.SetFloat("MasterVolume", Mathf.Log10(SettingsManager.Instance.masterVolume) * 20f);
            if (SettingsManager.Instance.musicVolume <= 0.001f) mixer.SetFloat("MusicVolume", -80f);
            else mixer.SetFloat("MusicVolume", Mathf.Log10(SettingsManager.Instance.musicVolume) * 20f);
            if (SettingsManager.Instance.sfxVolume <= 0.001f) mixer.SetFloat("SFXVolume", -80f);
            else mixer.SetFloat("SFXVolume", Mathf.Log10(SettingsManager.Instance.sfxVolume) * 20f);
            if (SettingsManager.Instance.targetFPS <= 0)
            {
                toggleFPSCheckBoxImage.SetActive(false);
                Application.targetFrameRate = -1;
            }
            else
            {
                toggleFPSCheckBoxImage.SetActive(true);
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = targetFps;
            }

            if (!SettingsManager.Instance.screenShakeEnabled)
            {
                screenShake = false;
                toggleScreenShakeCheckBoxImage.SetActive(false);
            }
            else
            {
                screenShake = true;
                toggleScreenShakeCheckBoxImage.SetActive(true);
            }
        }
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
    
    public void OpenPlayPanel()
    {
        openPlayPanelFeedback?.PlayFeedbacks();
        closeMainMenuPanelFeedback?.PlayFeedbacks();
    }
    public void ClosePlayPanel()
    {
        openMainMenuPanelFeedback?.PlayFeedbacks();
        closePlayPanelFeedback?.PlayFeedbacks();
    }
    
    public void OpenAnomaliesPanel()
    {
        openAnomaliesPanelFeedback?.PlayFeedbacks();
        closePlayPanelFeedback?.PlayFeedbacks();
    }
    public void CloseAnomaliesPanel()
    {
        openPlayPanelFeedback?.PlayFeedbacks();
        closeAnomaliesPanelFeedback?.PlayFeedbacks();
    }
    
    public void OpenSettingsPanel()
    {
        openSettingsPanelFeedback?.PlayFeedbacks();
        closeMainMenuPanelFeedback?.PlayFeedbacks();
    }
    public void CloseSettingsPanel()
    {
        openMainMenuPanelFeedback?.PlayFeedbacks();
        closeSettingsPanelFeedback?.PlayFeedbacks();
    }
    
    public void OpenCreditsPanel()
    {
        openCreditsPanelFeedback?.PlayFeedbacks();
        closeSettingsPanelFeedback?.PlayFeedbacks();
    }
    
    public void CloseCreditsPanel()
    {
        openSettingsPanelFeedback?.PlayFeedbacks();
        closeCreditsPanelFeedback?.PlayFeedbacks();
    }
    
    public void PlayLevel()
    {
        PlayLevel1();
        /*switch ((CutsceneSO.CutsceneID)CampaignProgressManager.Instance.campaignProgressLevel)
        {
            case CutsceneSO.CutsceneID.Intro:
                PlayChickenLevel();
                break;
            case CutsceneSO.CutsceneID.ChickenToPig:
                PlayPigLevel();
                break;
            case CutsceneSO.CutsceneID.PigToSheep:
                PlaySheepLevel();
                break;
            case CutsceneSO.CutsceneID.SheepToCow:
                PlayCowLevel();
                break;
            case CutsceneSO.CutsceneID.CowToChick:
                PlayChickLevel();
                break;
            case CutsceneSO.CutsceneID.ChickToEndless:
                //PlayEndlessLevel();
                PlayChickLevel();
                break;
            default:
                PlayCutscenesScene();
                break;
        }*/
    }
    
    private void PlayCutscenesScene()
    {
        loadCutscenesSceneFeedback.PlayFeedbacks();
    }

    private void PlayLevel1()
    {
        loadLvl1Feedback.PlayFeedbacks();
    }

    public void Wishlist()
    {
        Application.OpenURL(GameConstants.SteamWishlistUrl);
    }

    public void Feedback()
    {
        Application.OpenURL(GameConstants.FeedbackUrl);
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

    public void ToggleFpsCap()
    {
        bool isFpsCapped = false;
        if (Application.targetFrameRate < 0) // something more than target
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFps;
            isFpsCapped = true;
            SettingsManager.Instance.targetFPS = targetFps;
        }
        else
        {
            Application.targetFrameRate = -1;
            SettingsManager.Instance.targetFPS = -1;
        }
        toggleFPSCheckBoxImage?.SetActive(isFpsCapped);
    }

    public void ToggleScreenShake()
    {
        screenShake = !screenShake;
        SettingsManager.Instance.screenShakeEnabled = screenShake;
        toggleScreenShakeCheckBoxImage.SetActive(screenShake);
    }

    public void OpenAreYouSureObject()
    {
        areYouSureObject.SetActive(true);
    }
    
    public void CloseAreYouSureObject()
    {
        areYouSureObject.SetActive(false);
    }

    public void OpenLoseNormalEndlessProgressObject()
    {
        loseNormalEndlessProgressObject.SetActive(true);
    }
    
    public void CloseLoseNormalEndlessProgressObject()
    {
        loseNormalEndlessProgressObject.SetActive(false);
    }
    
    public void OpenLoseHardcoreEndlessProgressObject()
    {
        loseHardcoreEndlessProgressObject.SetActive(true);
    }
    
    public void CloseLoseHardcoreEndlessProgressObject()
    {
        loseHardcoreEndlessProgressObject.SetActive(false);
    }
    
    
    public void ResetCampaignProgress()
    {

    }
    
}
