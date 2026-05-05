using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/LeverageIncrease")]
public class LeverageIncrease : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase max <color=#{ColorUtility.ToHtmlStringRGB(color)}>Multiplier</color>\nby <color=#{ColorUtility.ToHtmlStringRGB(color)}>{GetCurrentRuntimeEffectAmount()}X</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.maxLeverage += GetCurrentRuntimeEffectAmount();
        PermaUpgradesManager.Instance.riskyMovesLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.riskyMovesLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.maxLeverage;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.riskyMovesLvl >= upgradeProgression.Count;
    }
}