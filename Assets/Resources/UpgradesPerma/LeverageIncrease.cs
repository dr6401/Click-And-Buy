using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/LeverageIncrease")]
public class LeverageIncrease : PermaUpgrade
{
    public float leverageIncrease;
    
    private void OnEnable()
    {
        description = $"Increase <color=#{ColorUtility.ToHtmlStringRGB(color)}>Max Multiplier</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{leverageIncrease}</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.maxLeverage += leverageIncrease;
        PermaUpgradesManager.Instance.riskyMovesLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.riskyMovesLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.maxLeverage;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.riskyMovesLvl >= costProgression.Count;
    }
}