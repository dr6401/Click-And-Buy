using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/Tokenize")]
public class Tokenize : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.tokenColor)}>Token</color> gain from all sources by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.tokenIncomeMultiplier += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.tokenIncomeMultiplierLvl++;
        
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.tokenIncomeMultiplierLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return (PlayerStats.Instance.tokenIncomeMultiplier - 1) * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.tokenIncomeMultiplierLvl >= upgradeProgression.Count;
    }
}
