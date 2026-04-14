using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/LossReductionIncrease")]
public class LossReductionIncrease : Augment
{
    public int increaseAmount;

    private void OnEnable()
    {
        description = $"Reduce losses from Losing Trades by {increaseAmount}%";
    }

    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.lossReduction += increaseAmount;
    }
}