using UnityEngine;

public enum AugmentTier
{
    Common,
    Rare,
    Epic,
    Legendary
};
public enum AugmentCategory
{
    MoneyIncrease,
    LeverageIncrease,
    MoneyGainMultiplier,
    SwitchTradeTypes,
    NextTradeFree,
    PriceChange,
    MaxTradesIncrease,
    EventSchedule,
    ConvertAllToBuy,
    ConvertAllToSell,
    DuplicateBestTrade
    
};

public abstract class Augment : ScriptableObject
{
    public string augmentName;
    [TextArea] public string description;
    public Sprite icon;
    public AugmentTier tier;
    public AugmentCategory category;
    public Color color;
    public bool removeFromPoolAfterPicking;
    
    public abstract void Apply(GameObject player);
}
