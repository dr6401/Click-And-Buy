using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/Prophet")]
public class Prophet : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Increase chance to <color=#{ColorUtility.ToHtmlStringRGB(color)}>Identify trends</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{GetCurrentRuntimeEffectAmount()}%</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.chanceToIdentifyTrends += GetCurrentRuntimeEffectAmount() * 0.01f;
        PermaUpgradesManager.Instance.prophetLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.prophetLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.chanceToIdentifyTrends * 100;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.prophetLvl >= upgradeProgression.Count;
    }
}