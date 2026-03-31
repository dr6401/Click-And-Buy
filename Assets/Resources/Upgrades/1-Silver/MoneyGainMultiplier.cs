using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/MoneyGainMultiplier")]
public class MoneyGainMultiplier : Augment
{
    public float multiplier;
    public override void Apply(GameObject player)
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.moneyGainMultiplier += multiplier;
    }
}