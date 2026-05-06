using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/DivineIntervention")]
public class DivineIntervention : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"After opening <color=#{ColorUtility.ToHtmlStringRGB(color)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}</color> trades, your worst <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.redColor)}>losing trade</color> turns <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.greenColor)}>profitable</color> and closes";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.divineInterventionNumber = GetCurrentRuntimeEffectAmount();
        PermaUpgradesManager.Instance.divineInterventionLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.divineInterventionLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.divineInterventionNumber;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.divineInterventionLvl >= upgradeProgression.Count;
    }
}