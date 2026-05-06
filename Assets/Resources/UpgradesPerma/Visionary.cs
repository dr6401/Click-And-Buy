using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/Visionary")]
public class Visionary : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase chance to <color=#{ColorUtility.ToHtmlStringRGB(color)}>Identify trends</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{GetCurrentRuntimeEffectAmount()}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.chanceToIdentifyTrends += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.chanceToIdentifyTrendsLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.chanceToIdentifyTrendsLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.chanceToIdentifyTrends * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.chanceToIdentifyTrendsLvl >= upgradeProgression.Count;
    }
}