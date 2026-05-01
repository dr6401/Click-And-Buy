using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrencies : MonoBehaviour
{
    public static PlayerCurrencies Instance;

    public enum Currency
    {
        forex,
        fivex,
        amazoom,
        toyYoda,
        tesluck,
        moonCoin,
        poopCoin,
        timeCoin,
        infinityCoin,
        godCoin
    }
    
    public List<CurrencyRuntimeEntry> currencyRuntimeData = new List<CurrencyRuntimeEntry>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeCurrencyRuntimeData();
        LockAllCurrencies();
        foreach (var currency in currencyRuntimeData)
        {
            Debug.Log($"Currency: {currency.currency}, unlock Status:  {currency.isUnlocked}");
        }
    }
    
    public void AddCurrency(float amount, Currency currency)
    {
        Debug.Log($"Adding amount: {amount} to current amount: {GetTokensAmount(currency)}");
        GetCurrencyRuntimeEntry(currency).currentAmount += amount;
        GetCurrencyRuntimeEntry(currency).currentAmount = Mathf.Max(0, GetCurrencyRuntimeEntry(currency).currentAmount);
    }

    public void InitializeCurrencyRuntimeData()
    {
        foreach (Currency cur in System.Enum.GetValues(typeof(Currency)))
        {
            currencyRuntimeData.Add(
                new CurrencyRuntimeEntry(cur));
        }
    }

    public float GetTokensAmount(Currency currency)
    {
        //Debug.Log($"Current {currency} tokens: {GetCurrencyRuntimeEntry(currency).currentAmount}");
        return GetCurrencyRuntimeEntry(currency).currentAmount;
    }

    public void LockAllCurrencies()
    {
        foreach (CurrencyRuntimeEntry currency in currencyRuntimeData)
        {
            currency.isUnlocked = false;
        }
        currencyRuntimeData[0].isUnlocked = true; // Unlock base currency
    }

    public bool IsCurrencyUnlocked(Currency currency)
    {
        return GetCurrencyRuntimeEntry(currency).isUnlocked;
    }

    public CurrencyRuntimeEntry GetCurrencyRuntimeEntry(Currency currency)
    {
        foreach (var curRuntimeEntry in currencyRuntimeData)
        {
            if (curRuntimeEntry.currency == currency)
            {
                return curRuntimeEntry;
            }
        }
        Debug.LogWarning($"There is NO CURRENCY ENTRY for currency: {currency}");
        return null;
    }
}
