using System.Collections.Generic;
using UnityEngine;

public abstract class PermaUpgrade : Augment
{
    [Header("Perma Upgrade Settings")]
    public List<float> costProgression;
    public string leftText;
    public string rightText;
    public abstract override void Apply();

    public abstract int GetCurrentRuntimeLevel();

    public float GetCurrentRuntimeCost()
    {
        return costProgression[Mathf.Min(costProgression.Count, GetCurrentRuntimeLevel())];
    }
    public abstract float GetCurrentRuntimeValue();
    public abstract bool IsUpgradeMaxedOut();
}
