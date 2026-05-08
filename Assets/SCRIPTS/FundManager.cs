using System.Collections.Generic;
using UnityEngine;

public class FundManager : MonoBehaviour
{
    public GameObject fundItemPrefab;
    [SerializeField] Transform archivedFundsParent;
    
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
        ArchivedFund fundInQuestion = LevelManager.Instance.currentFund;
        fundInQuestion.fundName = "Mamaguevo Fund";
        fundInQuestion.DebugFundStats();
        AddFundToDisplay(fundInQuestion);
        PlayerStats.Instance.faith += fundInQuestion.valuation;
        archivedFunds.Add(fundInQuestion);
        LevelManager.Instance.ResetLvlManagerValuesAtFundSell();
        PlayerCurrencies.Instance.ResetAllCurrencies();
    }

    private void AddFundToDisplay(ArchivedFund fund)
    {
        GameObject newFundItem = Instantiate(fundItemPrefab, archivedFundsParent);
        FundItem fundItemScript = newFundItem.GetComponent<FundItem>();
        fundItemScript.Setup(fund);
    }
}
[System.Serializable]
public class ArchivedFund
{
    public string fundName;
    public float valuation;
    public float passiveIncome;
    public PlayerCurrencies.Currency highestUnlockedCurrency;
    public float lifetimeProfit;

    public void DebugFundStats()
    {
        Debug.Log($"Name: {fundName}\n" +
                  $"Valuation: {valuation}\n" +
                  $"PassiveIncome: {passiveIncome}\n" +
                  $"HighestUnlockedCurrency: {highestUnlockedCurrency}\n" +
                  $"LifetimeProfit: {lifetimeProfit}");
    }
}