using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/LuckyStrike")]
public class LuckyStrike : Augment
{
    public AugmentTier currentTier;
    public override void Apply()
    {
        AugmentTier higherTier = (AugmentTier) Mathf.Min((int) currentTier + 1, Enum.GetValues(typeof(AugmentTier)).Length - 1);
        /*switch (higherTier)
        {
            case AugmentTier.Common:
                higherTierAugmentsPool = UpgradesSelectionUI.Instance.commonAugments;
                break;
            case AugmentTier.Rare:
                higherTierAugmentsPool = UpgradesSelectionUI.Instance.rareAugments;
                break;
            case AugmentTier.Epic:
                higherTierAugmentsPool = UpgradesSelectionUI.Instance.epicAugments;
                break;
            case AugmentTier.Legendary:
                higherTierAugmentsPool = UpgradesSelectionUI.Instance.legendaryAugments;
                break;
            default:
                higherTierAugmentsPool = UpgradesSelectionUI.Instance.epicAugments;
                break;
        }*/
        
        List<Augment> higherTierAugmentsPool = UpgradesSelectionUI.Instance.augmentTierAugmentPools[higherTier];
        if (higherTierAugmentsPool.Count <= 0)
        {
            higherTier--;
            higherTierAugmentsPool = UpgradesSelectionUI.Instance.augmentTierAugmentPools[(AugmentTier)Mathf.Max(0, (int)higherTier)];
            
        }
        Augment randomAugment = higherTierAugmentsPool[Random.Range(0, higherTierAugmentsPool.Count)];
        randomAugment.Apply();
        Debug.Log($"Applied power-up {randomAugment.name} of tier {higherTier}.");
    }
}