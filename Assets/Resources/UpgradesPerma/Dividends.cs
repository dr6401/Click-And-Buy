using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/Dividends")]
public class Dividends : PermaUpgrade
{
    public float multiplier;
    
    private void OnEnable()
    {
        description = $"Increase <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.greenColor)}>passive income</color> from <color=#{ColorUtility.ToHtmlStringRGB(color)}>Sold accounts</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{multiplier}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.soldAccountsProfitMultiplier += multiplier * 0.01f;
        PermaUpgradesManager.Instance.soldAccountsProfitMultiplierLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.soldAccountsProfitMultiplierLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return (PlayerStats.Instance.soldAccountsProfitMultiplier - 1) * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.soldAccountsProfitMultiplierLvl >= upgradeProgression.Count;
    }

    public override string GetDescription()
    {
        return $"Increase <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.greenColor)}>passive income</color> from <color=#{ColorUtility.ToHtmlStringRGB(color)}>Sold accounts</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{multiplier}%</color>";
    }
}