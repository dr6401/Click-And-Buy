using UnityEngine;

public class TradeData
{
    [SerializeField] public TradeType tradeType;
    public string timeOfPurchase;

    public float quantity;
    public float entryPrice;

    public TradeData(TradeType tradeType, string timeOfPurchase, float quantity, float entryPrice)
    {
        this.tradeType = tradeType;
        this.timeOfPurchase = timeOfPurchase;
        this.quantity = quantity;
        this.entryPrice = entryPrice;
    }
}

public enum TradeType
{
    Buy,
    Sell
}
