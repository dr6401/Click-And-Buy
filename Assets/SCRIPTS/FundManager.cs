using System.Collections.Generic;
using UnityEngine;

public class FundManager : MonoBehaviour
{
    
    public List<ArchivedFund> archivedFunds;
    
    public static FundManager Instance;
    
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

    public void SellFund()
    {
        LevelManager.Instance.ResetLvlManagerValuesAtFundSell();
        PlayerCurrencies.Instance.ResetAllCurrencies();
    }
}
[System.Serializable]
public class ArchivedFund
{
    public string fundName;
    public float passiveIncome;
    public PlayerCurrencies.Currency highestUnlockedCurrency;
    public float lifetimeProfit;
}