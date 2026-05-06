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
    public int openWinningTradesPerSecondMultiplierLvl;
    public int prophetLvl;
    public int passiveTokenIncomeMultiplierLvl;
    [Header("Divine")]
    public int faithIncomeMultiplierLvl;
    public int echoProfitsMultiplierLvl;
    public int blessingOverflowLvl;
    public int divineInterventionLvl;
    public int visionaryLvl;
    public int marketGodLvl;
    

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
