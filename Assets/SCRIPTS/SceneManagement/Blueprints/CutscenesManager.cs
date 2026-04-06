using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutscenesManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Dictionary<CutsceneSO.CutsceneID, CutsceneSO> lookup;
    [SerializeField] private CutsceneSO current;
    [SerializeField] private CutsceneSO[] allCutscenes;
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float canvasFadeDuration = 1f;
    [SerializeField] private GameObject warningText;
    [SerializeField] private AudioSource musicAudioSource;
    [Header("Scene Loading")]
    [SerializeField] private MMFeedbacks loadMainMenuFeedback;
    [SerializeField] private MMFeedbacks loadLevel1Feedback;
    private Coroutine canvasFadeCoroutine;

    private void Awake()
    {
        LoadAllCutscenes();
    }

    void Start()
    {
        SelectCutscene();
        Time.timeScale = 1f;
        canvasGroup.interactable = true; // reset canvasGroup values
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0; 
        StartCoroutine(PlayCutsceneSequence());
    }

    private void LoadAllCutscenes()
    {
        allCutscenes = Resources.LoadAll<CutsceneSO>("Cutscenes");
        lookup = new Dictionary<CutsceneSO.CutsceneID, CutsceneSO>();

        foreach (var cutscene in allCutscenes)
        {
            if (!lookup.TryAdd(cutscene.id, cutscene))
            {
                Debug.LogWarning("Duplicate CutsceneID found: " + cutscene.id);
            }
        }
    }

    private void SelectCutscene()
    {
        SaveData saveData = SaveSystem.Load();
        int levelProgression = saveData.campaignData.campaignProgressLevel;
        var id = (CutsceneSO.CutsceneID)levelProgression; // If no next scene exists just play the intro
        current = lookup[id];
        if (id == 0) warningText?.SetActive(true);
        //Debug.Log($"PlayerPrefs.NextCutscene: {(CutsceneSO.CutsceneID)PlayerPrefs.GetInt("NextCutscene")}");
    }

    private IEnumerator PlayCutsceneSequence()
    {
        if (current.music != null)
        {
            musicAudioSource.PlayOneShot(current.music);
        }

        foreach (string line in current.text)
        {
            if (canvasFadeCoroutine != null) StopCoroutine(canvasFadeCoroutine);
            canvasFadeCoroutine = StartCoroutine(FadeCanvas(1f));
            //yield return new WaitForSeconds(canvasFadeDuration);
            text.text = line;
            yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);
            if (canvasFadeCoroutine != null) StopCoroutine(canvasFadeCoroutine);
            canvasFadeCoroutine = StartCoroutine(FadeCanvas(0));
            yield return new WaitForSeconds(canvasFadeDuration);
        }

        //PlayerPrefs.Save();
        //Debug.Log($"PlayerPrefs.NextScene: {(CutsceneSO.CutsceneID)PlayerPrefs.GetInt("NextCutscene")}");
        switch (current.id)
        {
            case CutsceneSO.CutsceneID.Intro: 
                loadMainMenuFeedback.PlayFeedbacks();
                break;
            case CutsceneSO.CutsceneID.ChickenToPig: 
                loadLevel1Feedback.PlayFeedbacks();
                break;
            default:
                Debug.LogWarning($"current.id was {current.id}, loading main menu as backup");
                loadMainMenuFeedback.PlayFeedbacks();
                break;
        }
        //SceneManager.LoadScene(current.nextScene);
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;
        while (time < canvasFadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / canvasFadeDuration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
}
