using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/MarketGod")]
public class MarketGod : PermaUpgrade
{
    public override string GetDescription()
    {
        if (GetCurrentRuntimeLevel() <= 0)
        {
            return $"Opening positions <color=#{ColorUtility.ToHtmlStringRGB(color)}>Shifts</color> the <color=#{ColorUtility.ToHtmlStringRGB(color)}>Market</color> in your <color=#{ColorUtility.ToHtmlStringRGB(color)}>Favor</color>!"; 
        }
        return $"Opening positions <color=#{ColorUtility.ToHtmlStringRGB(color)}>Shifts</color> the <color=#{ColorUtility.ToHtmlStringRGB(color)}>Market</color> in your <color=#{ColorUtility.ToHtmlStringRGB(color)}>Favor</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}</color>%";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.marketGodShitPercent += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.marketGodLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.marketGodLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.marketGodShitPercent * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.marketGodLvl >= upgradeProgression.Count;
    }
}