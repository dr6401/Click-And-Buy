using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/ProfitMultiplier")]
public class ProfitMultiplier : PermaUpgrade
{
    public float multiplier;
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.profitMultiplier += multiplier;
        PermaUpgradesManager.Instance.profitMultLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.profitMultLvl;
    }

    public override float GetCurrentRuntimeCost()
    {
        return costProgression[Mathf.Min(costProgression.Count, GetCurrentRuntimeLevel())];
    }

    public override float GetCurrentRuntimeValue()
    {
        return (PlayerStats.Instance.profitMultiplier - 1) * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.profitMultLvl >= costProgression.Count;
    }
}