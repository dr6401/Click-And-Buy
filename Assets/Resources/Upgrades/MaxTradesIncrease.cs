using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/MaxTradesIncrease")]
public class MaxTradesIncrease : Augment
{
    public int maxTradesIncrease;

    private void OnEnable()
    {
        description = $"Increase number of trades by {maxTradesIncrease}";
    }

    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.maxAliveTrades += maxTradesIncrease;
    }
}