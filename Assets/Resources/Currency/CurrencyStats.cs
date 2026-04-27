using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CurrencyStats", menuName = "Currency/CurrencyStats")]
public class CurrencyStats : ScriptableObject
{
    public List<CurrencyEntry> currencyEntries;

    public Sprite GetIconOfCurrency(PlayerCurrencies.Currency currency)
    {
        foreach (var currencyIconEntry in currencyEntries)
        {
            if (currencyIconEntry.currency == currency)
            {
                return currencyIconEntry.icon;
            }
        }
        Debug.LogWarning($"No ICON for currency: {currency}");
        return null;
    }

    public float GetUnlockPrice(PlayerCurrencies.Currency currency)
    {
        foreach (var currencyIconEntry in currencyEntries)
        {
            if (currencyIconEntry.currency == currency)
            {
                return currencyIconEntry.unlockAmount;
            }
        }
        Debug.LogWarning($"No ICON for currency: {currency}");
        return 0;
    }
}


[System.Serializable]
public class CurrencyEntry
{
    public PlayerCurrencies.Currency currency;
    public float unlockAmount;
    public Sprite icon;
}
