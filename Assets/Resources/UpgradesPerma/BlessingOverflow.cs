using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/BlessingOverflow")]
public class BlessingOverflow : PermaUpgrade
{
    public override string GetDescription()
    {
        return $"Every <color=#{ColorUtility.ToHtmlStringRGB(GameConstants.divineBlessingColor)}>{NumberFormatter.FormatNumber(GetCurrentRuntimeEffectAmount())}</color> Divine blessings, next one is <color=#{ColorUtility.ToHtmlStringRGB(color)}>FREE</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.blessingOverflowFreebieNumber = GetCurrentRuntimeEffectAmount();
        PermaUpgradesManager.Instance.blessingOverflowLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.blessingOverflowLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.blessingOverflowFreebieNumber;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.blessingOverflowLvl >= upgradeProgression.Count;
    }
}