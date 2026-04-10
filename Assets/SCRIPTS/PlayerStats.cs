using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    
    public int level = 1;

    public float moneyGainMultiplier = 1f;
    public float maxLeverage = 1;

    public int maxAliveTrades = 5;
    
    public float volatility = 1.25f;
    public float passiveIncome = 16f;
    public float lossShield = 20f;
    public float divineLuck = 1f;
    
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
