using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    
    public int level = 1;

    public float moneyGainMultiplier = 1f;
    public float maxLeverage = 1;

    public int maxAliveTrades = 5;

    public float maxComboDuration = 2f;
    
    public float volatility = 0f;
    
    public float passiveIncome = 0;
    public float passiveIncomeTriggerInterval = 1f;
    
    public float lossReduction = 0f;
    public float divineLuck = 0f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    
}
