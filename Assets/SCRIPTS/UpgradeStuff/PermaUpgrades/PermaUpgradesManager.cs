using UnityEngine;

public class PermaUpgradesManager : MonoBehaviour
{
    [Header("Permanent Upgrades Current Levels")]
    [Header("Cash")]
    public int profitMultLvl;
    public int lossShieldLvl;
    public int orderQuantityLvl;
    public int riskyMovesLvl;
    public int comboBonusMultiplierLvl;
    public int soldAccountsProfitMultiplierLvl;
    [Header("Stocks")]
    public int tokenIncomeMultiplierLvl;
    public int maxTradesLvl;
    public int volatilityLvl;
    

    public static PermaUpgradesManager Instance;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
}
