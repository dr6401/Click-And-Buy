using UnityEngine;

public class TradeData
{
    [SerializeField] public TradeType tradeType;
    public string timeOfPurchase;

    public float quantity;
    public float entryPrice;

    public float leverage = 1f;

    public TradeData(TradeType tradeType, string timeOfPurchase, float quantity, float entryPrice, float leverage)
    {
        this.tradeType = tradeType;
        this.timeOfPurchase = timeOfPurchase;
        this.quantity = quantity;
        this.entryPrice = entryPrice;
        this.leverage = leverage;
    }
}

public enum TradeType
{
    Buy,
    Sell
}
