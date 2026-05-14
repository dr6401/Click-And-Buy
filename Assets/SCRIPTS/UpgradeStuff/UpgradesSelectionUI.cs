using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class UpgradesSelectionUI : MonoBehaviour
{
    public static UpgradesSelectionUI Instance;
    
    public Transform buttonParent;
    public GameObject augmentButtonPrefab;
    public List<Augment> basicFirstAugments, basicSecondAugments, basicThirdAugments, basicForthAugments, basicFifthAugments, basicSixthAugments, basicSeventhAugments, basicEighthAugments, basicNinthAugments, basicTenthAugments;
    public List<Augment> forexAugments, fivexAugments, amazoomAugments, toyYodaAugments, tesluckAugments, moonCoinAugments, poopCoinAugments, timeCoinAugments, infinityCoinAugments, godCoinAugments;
    [SerializeField] private int numberOfChoices = 3;
    private int availableAugmentsAtStart;
    [SerializeField] private CanvasGroup canvasGroup;
    private Coroutine fadeInCoroutine;
    [Header("Augment Persistence")]
    [SerializeField] private RunAugmentData runAugmentData;
    [Header("-----TESTING-----")]
    [SerializeField] private bool testing_offerOnlyGoldAugments = false;

    private bool hasRespinedYet = false;

    public Dictionary<AugmentTier, List<Augment>> augmentTierAugmentPools => new Dictionary<AugmentTier, List<Augment>>()
    {
        { AugmentTier.BasicFirst, basicFirstAugments },
        { AugmentTier.BasicSecond, basicSecondAugments },
        { AugmentTier.BasicThird, basicThirdAugments },
        { AugmentTier.BasicForth, basicForthAugments },
        { AugmentTier.BasicFifth, basicFifthAugments },
        { AugmentTier.BasicSixth, basicSixthAugments },
        { AugmentTier.BasicSeventh, basicSeventhAugments },
        { AugmentTier.BasicEighth, basicEighthAugments },
        { AugmentTier.BasicNinth, basicNinthAugments },
        { AugmentTier.BasicTenth, basicTenthAugments },

        { AugmentTier.Forex, forexAugments },
        { AugmentTier.Fivex, fivexAugments },
        { AugmentTier.Amazoom, amazoomAugments },
        { AugmentTier.ToyYoda, toyYodaAugments },
        { AugmentTier.Tesluck, tesluckAugments },
        { AugmentTier.MoonCoin, moonCoinAugments },
        { AugmentTier.PoopCoin, poopCoinAugments },
        { AugmentTier.TimeCoin, timeCoinAugments },
        { AugmentTier.InfinityCoin, infinityCoinAugments },
        { AugmentTier.GodCoin, godCoinAugments }
    };

    public Dictionary<AugmentTier, float> baseAugmentRespinPrices => new Dictionary<AugmentTier, float>()
    {
        { AugmentTier.BasicFirst, 10 },
        { AugmentTier.BasicSecond, 50 },
        { AugmentTier.BasicThird, 200 },
        { AugmentTier.BasicForth, 1000 },
        { AugmentTier.BasicFifth, 2500 },
        { AugmentTier.BasicSixth, 5000 },
        { AugmentTier.BasicSeventh, 10000 },
        { AugmentTier.BasicEighth, 25000 },
        { AugmentTier.BasicNinth, 50000 },
        { AugmentTier.BasicTenth, 100000 },

        { AugmentTier.Forex, 1 },
        { AugmentTier.Fivex, 5 },
        { AugmentTier.Amazoom, 10 },
        { AugmentTier.ToyYoda, 15 },
        { AugmentTier.Tesluck, 25 },
        { AugmentTier.MoonCoin, 50 },
        { AugmentTier.PoopCoin, 50 },
        { AugmentTier.TimeCoin, 100 },
        { AugmentTier.InfinityCoin, 250 },
        { AugmentTier.GodCoin, 1000 }
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

        basicFirstAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/1-Basic"));
        basicSecondAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/2-Basic"));
        basicThirdAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/3-Basic"));
        basicForthAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/4-Basic"));
        basicFifthAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/5-Basic"));
        basicSixthAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/6-Basic"));
        basicSeventhAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/7-Basic"));
        basicEighthAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/8-Basic"));
        basicNinthAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/9-Basic"));
        basicTenthAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/10-Basic"));
        
        forexAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/1-Forex"));
        fivexAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/2-Fivex"));
        amazoomAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/3-Amazoom"));
        toyYodaAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/4-ToyYoda"));
        tesluckAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/5-Tesluck"));
        moonCoinAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/6-MoonCoin"));
        poopCoinAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/7-PoopCoin"));
        timeCoinAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/8-TimeCoin"));
        infinityCoinAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/9-InfinityCoin"));
        godCoinAugments = new List<Augment>(Resources.LoadAll<Augment>("Upgrades/Divine/10-GodCoin"));
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

        foreach (var silverAugment in basicFirstAugments)
        {
            availableAugmentsAtStart++;
        }
        foreach (var goldAugment in basicSecondAugments)
        {
            availableAugmentsAtStart++;
        }
        foreach (var prismaticAugment in basicThirdAugments)
        {
            availableAugmentsAtStart++;
        }
    }
    
    public void TriggerAugmentSelection(AugmentTier tier)
    {
        List<Augment> pool = testing_offerOnlyGoldAugments ? GetPoolByTier(AugmentTier.BasicSecond) : GetPoolByTier(tier);// if we're testing, enable only silver augments
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
                <= 50 => AugmentTier.BasicFirst,
                <= 80 => AugmentTier.BasicSecond,
                _ => AugmentTier.BasicThird
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
    
    public void TriggerAugmentSelectionForOneAugment(Augment augment)
    {
        Debug.Log("Triggered augment selection for ONLY one augment: " + augment.augmentName);
        
        var btnObj = Instantiate(augmentButtonPrefab, buttonParent);
        var btnObjScript = btnObj.GetComponent<UpgradeButton>();
        btnObjScript.Setup(augment, this);
        
        gameObject.SetActive(true);
        GameEvents.OnUpgradesOffered?.Invoke();
        canvasGroup.alpha = 1f;
    }
    
    private List<Augment> GetPoolByTier(AugmentTier tier)
    {
        return tier switch
        {
            AugmentTier.BasicFirst => basicFirstAugments,
            AugmentTier.BasicSecond => basicSecondAugments,
            AugmentTier.BasicThird => basicThirdAugments,
            AugmentTier.BasicForth => basicForthAugments,
            
            AugmentTier.Forex => forexAugments,
            AugmentTier.Fivex => fivexAugments,
            AugmentTier.Amazoom => amazoomAugments,
            AugmentTier.ToyYoda => toyYodaAugments,
            AugmentTier.Tesluck => tesluckAugments,
            AugmentTier.MoonCoin => moonCoinAugments,
            AugmentTier.PoopCoin => poopCoinAugments,
            AugmentTier.TimeCoin => timeCoinAugments,
            AugmentTier.InfinityCoin => infinityCoinAugments,
            AugmentTier.GodCoin => godCoinAugments,
            _ => basicFirstAugments,
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

    public void MarkAsRemovedFromPool(AugmentCategory category)
    {
        // Store all powerups of this type as chosen (so they get in the runAugmentData.ChosenAugments
        foreach (Augment augment in basicFirstAugments)
        {
            if (augment.category == category)
            {
                StoreChosenAugment(augment);
            }
        }
        foreach (Augment augment in basicSecondAugments)
        {
            if (augment.category == category)
            {
                StoreChosenAugment(augment);
            }
        }
        foreach (Augment augment in basicThirdAugments)
        {
            if (augment.category == category)
            {
                StoreChosenAugment(augment);
            }
        }
        foreach (Augment augment in basicForthAugments)
        {
            if (augment.category == category)
            {
                StoreChosenAugment(augment);
            }
        }
        // Mark those down as chosen (not actual powerup SOs in folders)
        foreach (Augment augment in runAugmentData.chosenAugments)
        {
            if (augment.category == category)
            {
                augment.removeFromPoolAfterPicking = true;
            }
        }
    }
    
    public void CloseUI()
    {
        canvasGroup.alpha = 0f;
        RemoveUpgradeCardsFromUpgradePanel();
        gameObject.SetActive(false);
    }

    public void RemoveUpgradeCardsFromUpgradePanel()
    {
        foreach (Transform upgradeCard in buttonParent.gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (upgradeCard == buttonParent.gameObject.transform) continue;
            Destroy(upgradeCard.gameObject);
        }
    }

    public void Respin(RectTransform rectTransform)
    {
        if (LevelManager.Instance.effectiveCash < LevelManager.Instance.currentRespinPrice)
        {
            LevelManager.Instance.SpawnTextDamageNumbers("Not enough Money!", position: rectTransform, canvasParent: GetComponent<RectTransform>());
            Debug.Log("Not enough Money for Respin!");
        }
        else
        {
            LevelManager.Instance.cash -= LevelManager.Instance.currentRespinPrice;
            Debug.Log($"Respinning!");
            LevelManager.Instance.currentRespinPrice *= 2;
            RemoveUpgradeCardsFromUpgradePanel();
            TriggerAugmentSelection(LevelManager.Instance.currentBasicCashOutTier);   
        }
    }
}
