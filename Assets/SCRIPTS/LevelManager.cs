using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    private bool isGamePaused = false;
    [SerializeField] private GameObject settingsMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        settingsMenu.SetActive(isGamePaused);
        GameEvents.OnGamePaused?.Invoke(isGamePaused);
        Debug.Log($"isGamePaused: {isGamePaused}");
    }
    
}
