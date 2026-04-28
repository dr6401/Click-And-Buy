using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrencies : MonoBehaviour
{
    public static PlayerCurrencies Instance;

    public float forexTokens;
    public float fivexTokens;
    public float amazoomTokens;
    public float toyYodaTokens;
    public float tesluckTokens;
    public float moonCoinTokens;
    public float poopCoinTokens;
    public float timeCoinTokens;
    public float infinityCoinTokens;
    public float godCoinTokens;

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
        switch (currency)
        {
            case Currency.forex:
                forexTokens += amount;
                break;
            case Currency.fivex:
                fivexTokens += amount;
                break;
            case Currency.amazoom:
                amazoomTokens += amount;
                break;
            case Currency.toyYoda:
                toyYodaTokens += amount;
                break;
            case Currency.tesluck:
                tesluckTokens += amount;
                break;
            case Currency.moonCoin:
                moonCoinTokens += amount;
                break;
            case Currency.poopCoin:
                poopCoinTokens += amount;
                break;
            case Currency.timeCoin:
                timeCoinTokens += amount;
                break;
            case Currency.infinityCoin:
                infinityCoinTokens += amount;
                break;
            case Currency.godCoin:
                godCoinTokens += amount;
                break;
        }
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
