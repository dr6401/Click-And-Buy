using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/PercentMoneyIncrease")]
public class PercentMoneyIncrease : Augment
{
    public float percentMoneyIncrease;
    public float maxIncreaseAmount;

    private void OnEnable()
    {
        description = $"Gain {percentMoneyIncrease}% of current Cash\n(max {maxIncreaseAmount}$)";
    }

    //public float maxIncrease;
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        //float increase = Mathf.Min(LevelManager.Instance.cash * (percentMoneyIncrease * 0.01f),  maxIncrease);
        float increase = LevelManager.Instance.cash * (percentMoneyIncrease * 0.01f);
        increase = Mathf.Clamp(increase, 0, maxIncreaseAmount);
        LevelManager.Instance.cash += increase;
        LevelManager.Instance.SpawnReceivedMoneyDamageNumbers(increase);
    }
}