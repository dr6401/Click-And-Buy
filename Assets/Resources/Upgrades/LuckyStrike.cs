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
        
        List<Augment> higherTierAugmentsPool = UpgradesSelectionUI.Instance.augmentTierAugmentPools[higherTier];
        if (higherTierAugmentsPool.Count <= 0)
        {
            higherTier--;
            higherTierAugmentsPool = UpgradesSelectionUI.Instance.augmentTierAugmentPools[(AugmentTier)Mathf.Max(0, (int)higherTier)];
            
        }
        
        Augment randomAugment = higherTierAugmentsPool[Random.Range(0, higherTierAugmentsPool.Count)];
        if (randomAugment.autoApply)
        {
            randomAugment.Apply();
        }
        else
        {
            PowerUpInventoryManager.Instance.AddPowerUp(randomAugment);
        }
        Debug.Log($"Applied power-up {randomAugment.name} of tier {higherTier}.");
    }
}