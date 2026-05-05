using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/MaxTradesIncrease")]
public class MaxTradesIncrease : PermaUpgrade
{
    public int maxTradesIncrease;

    private void OnEnable()
    {
        description = $"Increase max number of <color=#{ColorUtility.ToHtmlStringRGB(color)}>Active trades</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{maxTradesIncrease}</color>";
    }

    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.maxAliveTrades += maxTradesIncrease;
        PermaUpgradesManager.Instance.maxTradesLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.maxTradesLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.maxAliveTrades;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.maxTradesLvl >= upgradeProgression.Count;
    }

    public override string GetDescription()
    {
        return
            $"Increase max number of <color=#{ColorUtility.ToHtmlStringRGB(color)}>Active trades</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{maxTradesIncrease}</color>";
    }
}