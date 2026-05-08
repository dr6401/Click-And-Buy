using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FundItem : MonoBehaviour
{
    [SerializeField] private ArchivedFund fund;
    [SerializeField] private TMP_Text fundNameText;
    [SerializeField] private TMP_Text passiveIncomeText;
    [SerializeField] private Image highestUnlockedCurrencyImage;

    public void Setup(ArchivedFund fnd)
    {
        if (fnd == null) return;
        fund = fnd;
        fundNameText.text = fnd.fundName;
        passiveIncomeText.text = $"Income: {NumberFormatter.FormatDecimalNumber(fnd.passiveIncome * PlayerStats.Instance.soldAccountsProfitMultiplier)}$/s";
        highestUnlockedCurrencyImage.sprite = LevelManager.Instance.currencyStats.GetIconOfCurrency(fnd.highestUnlockedCurrency);
    }

    private void OnEnable()
    {
        Setup(fund);
    }
}
