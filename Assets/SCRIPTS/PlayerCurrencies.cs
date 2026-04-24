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
}
