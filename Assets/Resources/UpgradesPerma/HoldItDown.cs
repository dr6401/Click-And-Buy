using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/HoldItDown")]
public class HoldItDown : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Open <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.greenColor)}>Winning trades</color> earn <color=#{ColorUtility.ToHtmlStringRGB(color)}>{GetCurrentRuntimeEffectAmount()}%</color> of <color=#{ColorUtility.ToHtmlStringRGB(color)}>Unrealized profit</color> per second";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.openWinningTradesPerSecondMultiplier += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.openWinningTradesPerSecondMultiplierLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.openWinningTradesPerSecondMultiplierLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.openWinningTradesPerSecondMultiplier * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.openWinningTradesPerSecondMultiplierLvl >= upgradeProgression.Count;
    }
}