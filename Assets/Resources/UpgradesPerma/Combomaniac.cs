using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/Combomaniac")]
public class Combomaniac : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase <color=#{ColorUtility.ToHtmlStringRGB(color)}>Combo bonus</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.comboBonusMultiplier += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.comboBonusMultiplierLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.comboBonusMultiplierLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return (PlayerStats.Instance.comboBonusMultiplier - 1) * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.comboBonusMultiplierLvl >= upgradeProgression.Count;
    }
}