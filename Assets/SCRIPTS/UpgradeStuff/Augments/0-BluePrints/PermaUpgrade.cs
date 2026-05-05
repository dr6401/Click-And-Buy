using System.Collections.Generic;
using UnityEngine;

public class PermaUpgrade : Augment
{
    [Header("Perma Upgrade Settings")]
    public List<float> costProgression;
    public string leftText;
    public string rightText;
    public override void Apply()
    {
        
    }
    
    public virtual int GetCurrentRuntimeLevel()
    {
        return 0;
    }
    
    public virtual float GetCurrentRuntimeCost()
    {
        return 10;
    }

    public virtual float GetCurrentRuntimeValue()
    {
        return 0;
    }

    public virtual bool IsUpgradeMaxedOut()
    {
        return false;
    }
}
