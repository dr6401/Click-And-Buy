using System.Collections.Generic;
using UnityEngine;

public abstract class PermaUpgrade : Augment
{
    [Header("Perma Upgrade Settings")]
    public List<PermaUpgradeTierEntry> upgradeProgression;
    //public List<float> costProgression;
    public string leftText;
    public string rightText;
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
    public abstract string GetDescription();
}

[System.Serializable]
public class PermaUpgradeTierEntry
{
    public float upgradeEffectAmount;
    public float upgradeCost;
}
