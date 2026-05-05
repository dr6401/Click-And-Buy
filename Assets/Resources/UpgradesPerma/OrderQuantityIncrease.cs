using UnityEngine;

[CreateAssetMenu(fileName = "PermaAugment", menuName = "PermaAugments/OrderQuantityIncrease")]
public class OrderQuantityIncrease : PermaUpgrade
{
    public int amount;
    
    private void OnEnable()
    {
        description = $"Increase max <color=#{ColorUtility.ToHtmlStringRGB(color)}>Order quantity</color> by <color=#{ColorUtility.ToHtmlStringRGB(color)}>{amount}</color>";
    }
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.maxOrderQuantity += amount;
        PermaUpgradesManager.Instance.orderQuantityLvl++;
    }

    public override int GetCurrentRuntimeLevel()
    {
        return PermaUpgradesManager.Instance.orderQuantityLvl;
    }

    public override float GetCurrentRuntimeValue()
    {
        return PlayerStats.Instance.maxOrderQuantity;
    }

    public override bool IsUpgradeMaxedOut()
    {
        return PermaUpgradesManager.Instance.orderQuantityLvl >= costProgression.Count;
    }
}