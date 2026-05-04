using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/ProfitMultiplier")]
public class ProfitMultiplier : Augment
{
    public float multiplier;
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.moneyGainMultiplier += multiplier;
    }
}