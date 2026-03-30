using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class UpgradesSelectionUI : MonoBehaviour
{
    public static UpgradesSelectionUI Instance;
    
    public GameObject player;
    public Transform buttonParent;
    public GameObject augmentButtonPrefab;
    public List<Augment> silverAugments, goldAugments, prismaticAugments;
    [SerializeField] private int numberOfChoices = 3;
    private int availableAugmentsAtStart;
    [SerializeField] private CanvasGroup canvasGroup;
    private Coroutine fadeInCoroutine;
    [Header("Augment Persistence")]
    [SerializeField] private RunAugmentData runAugmentData;
    [Header("-----TESTING-----")]
    [SerializeField] private bool testing_offerOnlyGoldAugments = false;

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

        silverAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/1-Silver"));
        goldAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/2-Gold"));
        prismaticAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/3-Prismatic"));
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

        foreach (var silverAugment in silverAugments)
        {
            availableAugmentsAtStart++;
        }
        foreach (var goldAugment in goldAugments)
        {
            availableAugmentsAtStart++;
        }
        foreach (var prismaticAugment in prismaticAugments)
        {
            availableAugmentsAtStart++;
        }
    }
    
    public void TriggerAugmentSelection(AugmentTier tier)
    {
        List<Augment> pool = testing_offerOnlyGoldAugments ? GetPoolByTier(AugmentTier.Gold) : GetPoolByTier(tier);// if we're testing, enable only silver augments
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
                <= 50 => AugmentTier.Silver,
                <= 80 => AugmentTier.Gold,
                _ => AugmentTier.Prismatic
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
            btnObjScript.Setup(choice, player, this);
        }
        GameEvents.OnUpgradesOffered?.Invoke();
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
    }
    
    private List<Augment> GetPoolByTier(AugmentTier tier)
    {
        return tier switch
        {
            AugmentTier.Silver => silverAugments,
            AugmentTier.Gold => goldAugments,
            AugmentTier.Prismatic => prismaticAugments,
            _ => silverAugments,
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
