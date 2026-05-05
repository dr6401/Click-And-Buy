using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/OrderQuantityIncrease")]
public class OrderQuantityIncrease : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase max <color=#{ColorUtility.ToHtmlStringRGB(color)}>Order quantity</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{GetCurrentRuntimeEffectAmount()}</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.maxOrderQuantity += (int)GetCurrentRuntimeEffectAmount();
        PermaUpgradesManager.Instance.orderQuantityLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.orderQuantityLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.maxOrderQuantity;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.orderQuantityLvl >= upgradeProgression.Count;
    }
}