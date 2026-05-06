using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/PriceSwings")]
public class PriceSwings : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase <color=#{ColorUtility.ToHtmlStringRGB(color)}>Volatility</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.volatility += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.volatilityLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.volatilityLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return (PlayerStats.Instance.volatility - 1) * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.volatilityLvl >= upgradeProgression.Count;
    }
}