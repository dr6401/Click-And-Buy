using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CurrencyStats", menuName = "Currency/CurrencyStats")]
public class CurrencyStats : ScriptableObject
{
    public List<CurrencyIconEntry> currencyIcons;

    public Sprite GetIconOfCurrency(PlayerCurrencies.Currency currency)
    {
        foreach (var currencyIconEntry in currencyIcons)
        {
            if (currencyIconEntry.currency == currency)
            {
                return currencyIconEntry.icon;
            }
        }
        Debug.LogWarning($"No ICON for currency: {currency}");
        return null;
    }
}


[System.Serializable]
public class CurrencyIconEntry
{
    public PlayerCurrencies.Currency currency;
    public Sprite icon;
}
