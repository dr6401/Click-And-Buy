using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ClearPrefsOnPlay
{
    static ClearPrefsOnPlay()
    {
        EditorApplication.playModeStateChanged += ClearOnPlay;
    }

    private static void ClearOnPlay(PlayModeStateChange state)
    {
        //if (state == PlayModeStateChange.ExitingPlayMode)
        //{
        //   PlayerPrefs.DeleteAll();
        //  CampaignProgressManager.Instance?.ResetAllAnimalScores();
        //}
    } 
}
