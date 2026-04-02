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
    public float currentTimeScale = 1f;

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
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        inputBlocked = true;
    }

    public void ResumeTime()
    {
        Time.timeScale = currentTimeScale;
        inputBlocked = false;
    }
    
    public void PauseGame()
    {
        if (!isGamePaused) currentTimeScale = Time.timeScale;
        isGamePaused = !isGamePaused;
        LevelManager.Instance.isInputBlocked = isGamePaused;
        Time.timeScale = isGamePaused ? 0f : currentTimeScale;
        settingsMenu.SetActive(isGamePaused);
        GameEvents.OnGamePaused?.Invoke(isGamePaused);
        Debug.Log($"isGamePaused: {isGamePaused}");
    }
    
    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
        currentTimeScale = Time.timeScale;
    }

    private void OnEnable()
    {
        GameEvents.OnUpgradesOffered += StopTime;
        GameEvents.OnUpgradeChosen += ResumeTime;
    }
    
    private void OnDisable()
    {
        GameEvents.OnUpgradesOffered -= StopTime;
        GameEvents.OnUpgradeChosen -= ResumeTime;
    }
}
