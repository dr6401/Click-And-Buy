using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    }
    
    public void AddCurrency(float amount, Currency currency)
    {
        Debug.Log($"Adding amount: {amount} to current amount: {GetTokensAmount(currency)}");
        GetCurrencyRuntimeEntry(currency).currentAmount += amount;
        GetCurrencyRuntimeEntry(currency).currentAmount = Mathf.Max(0, GetCurrencyRuntimeEntry(currency).currentAmount);
    }

    public void InitializeCurrencyRuntimeData()
    {
        Debug.Log($"Initializing Currency RuntimeData");
        foreach (Currency cur in Enum.GetValues(typeof(Currency)))
        {
            CurrencyRuntimeEntry entry = new CurrencyRuntimeEntry(cur);
            /*foreach (CurrencyEntry curEntry in LevelManager.Instance.currencyStats.currencyEntries)
            {
                if (entry.currency == curEntry.currency)
                {
                    entry.unlockAmount = curEntry.unlockAmount;
                    break;
                }
            }*/
            currencyRuntimeData.Add(entry);
            //Debug.Log($"Added Currency Entry: {cur} with unlock amount: {entry.unlockAmount}");
        }
    }

    public float GetTokensAmount(Currency currency)
    {
        //Debug.Log($"Current {currency} tokens: {GetCurrencyRuntimeEntry(currency).currentAmount}");
        return GetCurrencyRuntimeEntry(currency).currentAmount;
    }

    public float GetCurrencyUnlockAmount(Currency currency)
    {
        foreach (var curRuntimeEntry in currencyRuntimeData)
        {
            if (curRuntimeEntry.currency == currency)
            {
                return curRuntimeEntry.unlockAmount;
            }
        }
        return 100;
    }

    public void LockAllCurrencies()
    {
        foreach (CurrencyRuntimeEntry currency in currencyRuntimeData)
        {
            currency.isUnlocked = false;
        }
        currencyRuntimeData[0].isUnlocked = true; // Unlock base currency
    }

    public void UnlockCurrency(Currency currency)
    {
        foreach (var curRuntimeEntry in currencyRuntimeData)
        {
            if (curRuntimeEntry.currency == currency)
            {
                curRuntimeEntry.isUnlocked = true;
                return;
            }
        }
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
