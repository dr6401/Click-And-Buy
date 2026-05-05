using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/LossReductionIncrease")]
public class LossReductionIncrease : PermaUpgrade
{
    public int increaseAmount;

    private void OnEnable()
    {
        description = $"Reduce losses from <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.redColor)}>Losing trades</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{increaseAmount}%</color>";
    }

    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.lossReduction += increaseAmount * 0.01f;
        PermaUpgradesManager.Instance.lossShieldLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.lossShieldLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.lossReduction * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.lossShieldLvl >= costProgression.Count;
    }
}