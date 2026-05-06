using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    
    public int level = 1;

    [Header("Cash")]
    public float profitMultiplier = 1f;
    public float lossReduction = 0f;
    public int maxOrderQuantity = 1;
    public float maxLeverage = 1;
    public float comboBonusMultiplier = 1f;
    public float soldAccountsProfitMultiplier = 1f;

    // Stocks Stuff
    [Header("Stocks")]
    public float tokenIncomeMultiplier = 1f;
    public int maxAliveTrades = 5;
    public float volatility = 1f;
    public float openWinningTradesPerSecondMultiplier = 0f;
    public float chanceToIdentifyTrends = 0f;
    public float passiveTokenIncomeMultiplier = 0f;
    
    public float maxComboDuration = 2f;
    
    public float passiveIncome = 0;
    public float passiveIncomeTriggerInterval = 1f;
    
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
