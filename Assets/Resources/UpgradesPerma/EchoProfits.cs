using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/EchoProfits")]
public class EchoProfits : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Each <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.greenColor)}>Winning trade</color> earns <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}%</color> of its profit after closing";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.echoProfitMultiplier += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.echoProfitsMultiplierLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.echoProfitsMultiplierLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.echoProfitMultiplier * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.echoProfitsMultiplierLvl >= upgradeProgression.Count;
    }
}