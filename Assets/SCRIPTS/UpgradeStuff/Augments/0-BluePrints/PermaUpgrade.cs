using UnityEngine;

public class PermaUpgrade : Augment
{
    [Header("Perma Upgrade Settings")]
    public float baseCost;
    public string currencySymbol;
    public override void Apply()
    {
        
    }

    public virtual float GetCurrentRuntimeValue()
    {
        return 0;
    }
}
