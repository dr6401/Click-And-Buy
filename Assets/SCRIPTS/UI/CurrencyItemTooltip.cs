using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyItemTooltip : MonoBehaviour
{
    [SerializeField] private PlayerCurrencies.Currency currency;
    
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text unlockCost;
    [SerializeField] private Image unlockCurrencyImage;

    [SerializeField] private MMF_Player openTooltipFeedback;
    
    private CurrencyStats currencyStats;

    private void Start()
    {
        CurrencyItem currencyItem = GetComponentInParent<CurrencyItem>();
        if (currencyItem != null)
        {
            currency = currencyItem.currency;
            currencyStats = currencyItem.currencyStats;
        }
    }

    private void OnEnable()
    {
        openTooltipFeedback.PlayFeedbacks();
    }

    private void Update()
    {
        PlayerCurrencies.Currency previousCurrency = (PlayerCurrencies.Currency)Mathf.Max((int)(currency - 1), 0);
        if (!PlayerCurrencies.Instance.IsCurrencyUnlocked(previousCurrency))
        {
            unlockCost.text = $"LOCKED";
            unlockCurrencyImage?.gameObject.SetActive(false);
        }
        else if (!PlayerCurrencies.Instance.IsCurrencyUnlocked(currency))
        {
            unlockCost.text = $"{NumberFormatter.FormatNumber(currencyStats.GetUnlockPrice(currency))}";
            unlockCurrencyImage.sprite = currencyStats.GetIconOfCurrency(previousCurrency);
            unlockCurrencyImage?.gameObject.SetActive(true);
        }
        else
        {
            unlockCurrencyImage?.gameObject.SetActive(false);
            unlockCost.text = $"UNLOCKED";
        }
    }
}