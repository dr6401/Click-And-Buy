using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntrySceneManager : MonoBehaviour
{
    [SerializeField] private MMFeedbacks loadMainSceneFeedback;
    [SerializeField] private MMFeedbacks loadCutscenesSceneFeedback;
    private SaveData data;
    //private SaveData.GeneralData generalData;

    private void Awake()
    {
        data = SaveSystem.Load();
    }

    private void Start()
    {
        if (data != null)
        {
            if (!data.settingsData.hasPlayerSeenIntro)
            {
                SettingsManager.Instance.hasPlayerSeenIntro = true;
                data.settingsData.hasPlayerSeenIntro = true;
                SaveSystem.Save(data);
                loadCutscenesSceneFeedback.PlayFeedbacks();
                //SceneManager.LoadScene("CutscenesScene");
                return;
            }
            loadMainSceneFeedback.PlayFeedbacks();
            return;
        }
        
        // Keep this for safety if JSON data loading not working
        // IF YOU SEE BELOW CODE COMMENTED, UNCOMMENT IT AFTER MAKING SURE SAVEDATA SYSTEM WORKS
        /*if (!PlayerPrefs.HasKey("HasSeenIntro"))
        {
            PlayerPrefs.SetInt("HasSeenIntro", 1);
            PlayerPrefs.Save();
            loadCutscenesSceneFeedback.PlayFeedbacks();
            //SceneManager.LoadScene("CutscenesScene");
            return;
        }
        loadMainSceneFeedback.PlayFeedbacks();*/
    }

}
