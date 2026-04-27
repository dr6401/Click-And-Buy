using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyItem : MonoBehaviour
{
    [SerializeField] private PlayerCurrencies.Currency currency;
    [SerializeField] private float unlockCost;
    [SerializeField] private CurrencyStats currencyStats;
    
    [SerializeField] private bool isUnlocked;
    
    [SerializeField] private TMP_Text unlockCostText;
    [SerializeField] private Image icon;

    private void Awake()
    {
        currencyStats = Resources.Load<CurrencyStats>("Currency/CurrencyStats");
    }

    void Start()
    {
        icon.sprite = currencyStats.GetIconOfCurrency(currency);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
