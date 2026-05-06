using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/ProfitMultiplier")]
public class ProfitMultiplier : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase profit from <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.greenColor)}>Winning trades</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.profitMultiplier += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.profitMultLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.profitMultLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return (PlayerStats.Instance.profitMultiplier - 1) * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.profitMultLvl >= upgradeProgression.Count;
    }
}