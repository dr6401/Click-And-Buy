using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/VolatilityIncrease")]
public class VolatilityIncrease : Augment
{
    public int increaseAmount;

    private void OnEnable()
    {
        description = $"Increase Volatility (bigger price swings) by {increaseAmount}%";
    }

    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.volatility += increaseAmount;
        Debug.Log($"PlayerStats.Instance.volatility before Mathf.Max: {PlayerStats.Instance.volatility}");
        PlayerStats.Instance.volatility = Mathf.Clamp(PlayerStats.Instance.volatility, 0, GameConstants.maxVolatility);
        if (PlayerStats.Instance.volatility >= GameConstants.maxVolatility)
        {
            UpgradesSelectionUI.Instance.MarkAsRemovedFromPool(category);
        }
    }
}