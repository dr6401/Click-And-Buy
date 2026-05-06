using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/Visionary")]
public class Visionary : PermaUpgrade
{
    public override string GetDescription()
    {
        if (GetCurrentRuntimeLevel() <= 0)
        {
            return $"See into the <color=#{ColorUtility.ToHtmlStringRGB(color)}>Future</color>!"; 
        }
        return $"See <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())} seconds</color> into the future";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.foresightDuration += GetCurrentRuntimeEffectAmount();
        PermaUpgradesManager.Instance.visionaryLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.visionaryLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.foresightDuration;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.visionaryLvl >= upgradeProgression.Count;
    }
}