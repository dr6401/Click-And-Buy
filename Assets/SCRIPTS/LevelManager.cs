using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public float currentMoney = 100f;
    private bool isGamePaused = false;
    [SerializeField] private GameObject settingsMenu;

    [Header("Canvas stuff")] [SerializeField]
    private TMP_Text currentMoneyText;
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

        currentMoneyText.text = $"Money: {currentMoney}$";
    }
    
    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        settingsMenu.SetActive(isGamePaused);
        GameEvents.OnGamePaused?.Invoke(isGamePaused);
        Debug.Log($"isGamePaused: {isGamePaused}");
    }

    public void SpendMoney(float amount)
    {
        currentMoney -= amount;
        currentMoney = Mathf.Clamp(currentMoney, 0f, currentMoney);
        if (currentMoney <= 0f)
        {
            LoseGame();
        }
    }
    
    public void GainMoney(float amount)
    {
        currentMoney += amount;
    }

    public void LoseGame()
    {
        // Lose game or sum
    }
    
}
