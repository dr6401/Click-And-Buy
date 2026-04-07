using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class UpgradesSelectionUI : MonoBehaviour
{
    public static UpgradesSelectionUI Instance;
    
    public Transform buttonParent;
    public GameObject augmentButtonPrefab;
    public List<Augment> commonAugments, rareAugments, epicAugments, legendaryAugments;
    [SerializeField] private int numberOfChoices = 3;
    private int availableAugmentsAtStart;
    [SerializeField] private CanvasGroup canvasGroup;
    private Coroutine fadeInCoroutine;
    [Header("Augment Persistence")]
    [SerializeField] private RunAugmentData runAugmentData;
    [Header("-----TESTING-----")]
    [SerializeField] private bool testing_offerOnlyGoldAugments = false;

    public Dictionary<AugmentTier, List<Augment>> augmentTierAugmentPools => new Dictionary<AugmentTier, List<Augment>>()
    {
        { AugmentTier.Common, commonAugments },
        { AugmentTier.Rare, rareAugments },
        { AugmentTier.Epic, epicAugments },
        { AugmentTier.Legendary, legendaryAugments }
    };

    private bool hasSettingsCoveredUpAugmentUI;
    
    private static UpgradesSelectionUI instance;

    private void Awake()
    {
        if (Instance != null &&  Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        commonAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/1-Common"));
        rareAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/2-Rare"));
        epicAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/3-Epic"));
        legendaryAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/4-Legendary"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);

        foreach (var silverAugment in commonAugments)
        {
            availableAugmentsAtStart++;
        }
        foreach (var goldAugment in rareAugments)
        {
            availableAugmentsAtStart++;
        }
        foreach (var prismaticAugment in epicAugments)
        {
            availableAugmentsAtStart++;
        }
    }
    
    public void TriggerAugmentSelection(AugmentTier tier)
    {
        List<Augment> pool = testing_offerOnlyGoldAugments ? GetPoolByTier(AugmentTier.Rare) : GetPoolByTier(tier);// if we're testing, enable only silver augments
        pool.RemoveAll(augment => runAugmentData.IsAugmentInChosenAugments(augment) && augment.removeFromPoolAfterPicking);
        Debug.Log(tier + " pool: " + string.Join(", ", pool.Select(a => a.augmentName)));
        List<Augment> choices = GetRandomAugments(pool, numberOfChoices);
        Debug.Log(tier + " choices: " + string.Join(", ", choices.Select(a => a.augmentName)));
        
        Debug.Log("Current pool of " + tier + " augments: " + string.Join(", ", pool.Select(a => a.augmentName)));

        if (pool.Count <= 0) // If there are no more augments left in this tier, try again
        {
            if (AreAllAugmentsTaken())
            {
                Debug.Log("All Augments taken!");
                return;
            }
            int augmentChance = Random.Range(1, 100);
            AugmentTier augmentTier = augmentChance switch
            {
                <= 50 => AugmentTier.Common,
                <= 80 => AugmentTier.Rare,
                _ => AugmentTier.Epic
            };
            Debug.Log("Tier: " + tier + " did not have any augments left. Retrying augments with tier: " + augmentTier);
            TriggerAugmentSelection(augmentTier);
            return;
        }

        foreach (var choice in choices)
        {
            Debug.Log("Given you the choice: " + choice.augmentName);
            var btnObj = Instantiate(augmentButtonPrefab, buttonParent);
            var btnObjScript = btnObj.GetComponent<UpgradeButton>();
            btnObjScript.Setup(choice, this);
        }
        gameObject.SetActive(true);
        GameEvents.OnUpgradesOffered?.Invoke();
        canvasGroup.alpha = 1f;
    }
    
    private List<Augment> GetPoolByTier(AugmentTier tier)
    {
        return tier switch
        {
            AugmentTier.Common => commonAugments,
            AugmentTier.Rare => rareAugments,
            AugmentTier.Epic => epicAugments,
            _ => commonAugments,
        };
    }
    
    private List<Augment> GetRandomAugments(List<Augment> pool, int count)
    {
        var offeredAugments = new List<Augment>();
        var availableAugments = new List<Augment>(pool);

        for (int i = 0; i < count && availableAugments.Count > 0; i++)
        {
            int idx = Random.Range(0, availableAugments.Count);
            offeredAugments.Add(availableAugments[idx]);
            availableAugments.RemoveAt(idx);
        }

        return offeredAugments;
    }
    
    public bool AreAllAugmentsTaken()
    {
        return runAugmentData.NumberOfChosenAugments() >= availableAugmentsAtStart;
    }
    
    public void StoreChosenAugment(Augment augment)
    {
        runAugmentData.AddToChosenAugments(augment);
    }
    
    public void CloseUI()
    {
        canvasGroup.alpha = 0f;
        RemoveUpgradeCardsFromUpgradePanel();
        gameObject.SetActive(false);
    }

    private void RemoveUpgradeCardsFromUpgradePanel()
    {
        foreach (Transform upgradeCard in buttonParent.gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (upgradeCard == buttonParent.gameObject.transform) continue;
            Destroy(upgradeCard.gameObject);
        }
    }
}
