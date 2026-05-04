using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/ProfitMultiplier")]
public class ProfitMultiplier : PermaUpgrade
{
    public float multiplier;
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.profitMultiplier += multiplier;
    }

    public override float GetCurrentRuntimeValue()
    {
        return (PlayerStats.Instance.profitMultiplier - 1) * 100;
    }
}