using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/DualYield")]
public class DualYield : PermaUpgrade
{
    public override string GetDescription()
    {
        if (GetCurrentRuntimeLevel() <= 0)
        {
            return $"Sold accounts also generate <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.tokenColor)}>Tokens</color>!"; 
        }
        return $"Increase <color=#{ColorUtility.ToHtmlStringRGB(color)}>passive</color> <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.tokenColor)}>Token</color> <color=#{ColorUtility.ToHtmlStringRGB(color)}>income</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.passiveTokenIncomeMultiplier += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.passiveTokenIncomeMultiplierLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.passiveTokenIncomeMultiplierLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.passiveTokenIncomeMultiplier * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.passiveTokenIncomeMultiplierLvl >= upgradeProgression.Count;
    }
}