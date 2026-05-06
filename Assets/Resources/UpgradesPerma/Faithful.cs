using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/Faithful")]
public class Faithful : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.faithColor)}>Faith</color> gain from all sources by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.faithIncomeMultiplier += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.faithIncomeMultiplierLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.faithIncomeMultiplierLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return (PlayerStats.Instance.faithIncomeMultiplier - 1) * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.faithIncomeMultiplierLvl >= upgradeProgression.Count;
    }
}