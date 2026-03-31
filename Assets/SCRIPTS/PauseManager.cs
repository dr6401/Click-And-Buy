using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    private bool isGamePaused = false;
    [SerializeField] private GameObject settingsMenu;
    public bool inputBlocked = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !inputBlocked)
        {
            PauseGame();
        }
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
        inputBlocked = true;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
        inputBlocked = false;
    }
    
    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        LevelManager.Instance.isInputBlocked = isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        settingsMenu.SetActive(isGamePaused);
        GameEvents.OnGamePaused?.Invoke(isGamePaused);
        Debug.Log($"isGamePaused: {isGamePaused}");
    }

    private void OnEnable()
    {
        GameEvents.OnLevelUp += StopTime;
        GameEvents.OnUpgradeChosen += ResumeTime;
    }
    
    private void OnDisable()
    {
        GameEvents.OnLevelUp -= StopTime;
        GameEvents.OnUpgradeChosen -= ResumeTime;
    }
}
