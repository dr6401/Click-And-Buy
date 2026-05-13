using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/LuckyStrike")]
public class LuckyStrike : Augment
{
    public AugmentTier currentTier;
    public override void Apply()
    {
        AugmentTier higherTier = (AugmentTier) Mathf.Min((int) currentTier + 1, (int)AugmentTier.BasicTenth);
        
        List<Augment> higherTierAugmentsPool = new List<Augment>();
        foreach (Augment aug in UpgradesSelectionUI.Instance.augmentTierAugmentPools[higherTier])
        {
            higherTierAugmentsPool.Add(aug);
        }
        higherTierAugmentsPool.RemoveAll(augment => augment.category == AugmentCategory.LuckyStrike);
        
        if (higherTierAugmentsPool.Count <= 0)
        {
            higherTier--;
            higherTierAugmentsPool = UpgradesSelectionUI.Instance.augmentTierAugmentPools[(AugmentTier)Mathf.Max(0, (int)higherTier)];
        }
        
        Augment randomAugment = higherTierAugmentsPool[Random.Range(0, higherTierAugmentsPool.Count)];
        
        Debug.Log("Higher tier: " + higherTier + " pool: " + string.Join(", ", higherTierAugmentsPool.Select(a => a.augmentName)));
        Debug.Log("Removing old cards from UpgradeSelectionUI");
        
        UpgradesSelectionUI.Instance.RemoveUpgradeCardsFromUpgradePanel();
        UpgradesSelectionUI.Instance.TriggerAugmentSelectionForOneAugment(randomAugment);
        
        Debug.Log($"Applied power-up {randomAugment.name} of tier {higherTier}.");
    }
}