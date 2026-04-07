using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class FTUEManager : MonoBehaviour
{
    [SerializeField] private GameObject fTUECanvas;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private TMP_Text nextButtonText;
    [SerializeField] private List<string> tutorialTexts;
    [SerializeField] private MMFeedbacks fadeInTextFeedback;
    [SerializeField] private int currentTextIndex = 0;

    
    private void Start()
    {
        if (true)//(!SettingsManager.Instance.hasPlayerCompletedFTUE)
        {
            Debug.Log($"Player hasn't completed FTUE yet, starting FTUE");
            StartCoroutine(InitiateFTUE());
            SettingsManager.Instance.hasPlayerCompletedFTUE = true;
        }
    }
    
    private IEnumerator InitiateFTUE()
    {
        yield return new WaitForSeconds(2f);
        GameEvents.OnFTUETriggered?.Invoke();
    }

    private void SetFTUECanvasActive()
    {
        //Time.timeScale = 0f;
        Debug.Log("FTUE initiated");
        GameEvents.OnGamePaused?.Invoke(true);
        fTUECanvas?.SetActive(true);
        tutorialText.text = tutorialTexts[currentTextIndex];
        fadeInTextFeedback?.PlayFeedbacks();
    }

    public void ShowNextText()
    {
        if (currentTextIndex == tutorialTexts.Count - 1)
        {
            GameEvents.OnFTUEEnded?.Invoke();
            fTUECanvas.SetActive(false);
            GameEvents.OnGamePaused?.Invoke(false);
            //Time.timeScale = 1f;
            return;
        }
        fadeInTextFeedback?.PlayFeedbacks();
        currentTextIndex++;
        tutorialText.text = tutorialTexts[currentTextIndex];
        if (currentTextIndex == tutorialTexts.Count - 1)
        {
            nextButtonText.text = "TRADE!";
        }

        currentTextIndex = Mathf.Clamp(currentTextIndex, 0, tutorialTexts.Count - 1);
    }

    private void OnEnable()
    {
        GameEvents.OnFTUETriggered += SetFTUECanvasActive;
    }
    
    private void OnDisable()
    {
        GameEvents.OnFTUETriggered -= SetFTUECanvasActive;
    }
}