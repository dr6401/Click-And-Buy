using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    private float silverAugmentChance = 0.5f;
    private float goldAugmentChance = 0.3f;
    
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
    
    private void StartUpgradeSelection()
    {
        int augmentChance = Random.Range(1, 101);
        AugmentTier augmentTier;
        if (augmentChance <= silverAugmentChance * 100) augmentTier = AugmentTier.Silver;
        else if (augmentChance <= (silverAugmentChance + goldAugmentChance) * 100) augmentTier = AugmentTier.Gold;
        else augmentTier = AugmentTier.Prismatic;
        if (useTestingEqualAugmentOdds)
        {
            augmentTier = augmentChance switch
            {
                <= 33 => AugmentTier.Silver,
                <= 67 => AugmentTier.Gold,
                _ => AugmentTier.Prismatic
            };
        }
        Debug.Log($"AugmentChance: {augmentChance}, Augment Tier: {augmentTier}");
        UpgradesSelectionUI.Instance.TriggerAugmentSelection(augmentTier);
    }

    private void OnEnable()
    {
        GameEvents.OnLevelUp += StartUpgradeSelection;
    }
    
    private void OnDisable()
    {
        GameEvents.OnLevelUp -= StartUpgradeSelection;
    }
}
