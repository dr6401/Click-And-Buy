using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/PassiveIncomeIncrease")]
public class PassiveIncomeIncrease : Augment
{
    public int increaseAmount;

    private void OnEnable()
    {
        description = $"Increase Passive Income by {increaseAmount}$/sec";
    }

    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.passiveIncome += increaseAmount;
    }
}