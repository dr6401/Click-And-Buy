using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    private float silverAugmentChance = 0.5f;
    private float goldAugmentChance = 0.3f;
    
    public static float commonCashOutPrice = 100f;
    public static float rareCashOutPrice = 750f;
    public static float epicCashOutPrice = 2000f;
    public static float legendaryCashOutPrice = 10000f;
    

    public Dictionary<AugmentTier, float> cashOutTierPrices = new Dictionary<AugmentTier, float>()
    {
        { AugmentTier.Common, commonCashOutPrice },
        { AugmentTier.Rare, rareCashOutPrice },
        { AugmentTier.Epic, epicCashOutPrice },
        { AugmentTier.Legendary, legendaryCashOutPrice }
    };
    
    [Header("TESTING")] [SerializeField] private bool useTestingEqualAugmentOdds = false;

    public static UpgradesManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void StartUpgradeSelection(AugmentTier tier)
    {
        /*?int augmentChance = Random.Range(1, 101);
        AugmentTier augmentTier;
        if (augmentChance <= silverAugmentChance * 100) augmentTier = AugmentTier.Common;
        else if (augmentChance <= (silverAugmentChance + goldAugmentChance) * 100) augmentTier = AugmentTier.Rare;
        else augmentTier = AugmentTier.Epic;
        if (useTestingEqualAugmentOdds)
        {
            augmentTier = augmentChance switch
            {
                <= 33 => AugmentTier.Common,
                <= 67 => AugmentTier.Rare,
                _ => AugmentTier.Epic
            };
        }
        Debug.Log($"AugmentChance: {augmentChance}, Augment Tier: {augmentTier}");*/
        UpgradesSelectionUI.Instance.TriggerAugmentSelection(tier);
    }

    private void OnEnable()
    {
        GameEvents.OnCashOut += StartUpgradeSelection;
    }
    
    private void OnDisable()
    {
        GameEvents.OnCashOut -= StartUpgradeSelection;
    }
}
