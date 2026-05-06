using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class PermaUpgrade : Augment
{
    [Header("Perma Upgrade Settings")]
    public List<PermaUpgradeTierEntry> upgradeProgression;
    //public List<float> costProgression;
    public string leftText;
    public string rightText;
    public abstract string GetDescription();
    public abstract override void Apply();

    public abstract int GetCurrentRuntimeLevel();

    public float GetCurrentRuntimeEffectAmount()
    {
        if (GetCurrentRuntimeLevel() >= upgradeProgression.Count)
        {
            return GetCurrentRuntimeValue();
        } // else
        return upgradeProgression[Mathf.Min(upgradeProgression.Count, GetCurrentRuntimeLevel())].upgradeEffectAmount;   
    }
    public float GetCurrentRuntimeCost()
    {
        return upgradeProgression[Mathf.Min(upgradeProgression.Count, GetCurrentRuntimeLevel())].upgradeCost;
    }
    public abstract float GetCurrentRuntimeValue();
    public abstract bool IsUpgradeMaxedOut();
}

[System.Serializable]
public class PermaUpgradeTierEntry
{
    public float upgradeEffectAmount;
    public float upgradeCost;
}
