using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    private float silverAugmentChance = 0.5f;
    private float goldAugmentChance = 0.3f;
    
    // Basic
    public static float commonCashOutPrice = 100f;
    public static float rareCashOutPrice = 750f;
    public static float epicCashOutPrice = 2000f;
    public static float legendaryCashOutPrice = 10000f;
    
    // Currencies
    public static float forexCashOutPrice = 1f;
    public static float fivexCashOutPrice = 10f;
    public static float amazoomCashOutPrice = 10f;
    public static float toyYodaCashOutPrice = 10f;
    public static float tesluckCashOutPrice = 10f;
    public static float moonCoinCashOutPrice = 10f;
    public static float poopCoinCashOutPrice = 10f;
    public static float timeCoinCashOutPrice = 10f;
    public static float infinityCoinCashOutPrice = 10f;
    public static float godCoinCashOutPrice = 10f;
    

    public Dictionary<AugmentTier, float> cashOutTierPrices = new Dictionary<AugmentTier, float>()
    {
        { AugmentTier.Common, commonCashOutPrice },
        { AugmentTier.Rare, rareCashOutPrice },
        { AugmentTier.Epic, epicCashOutPrice },
        { AugmentTier.Legendary, legendaryCashOutPrice },
        { AugmentTier.Forex, forexCashOutPrice },
        { AugmentTier.Fivex, fivexCashOutPrice },
        { AugmentTier.Amazoom, amazoomCashOutPrice },
        { AugmentTier.ToyYoda, toyYodaCashOutPrice },
        { AugmentTier.Tesluck, tesluckCashOutPrice },
        { AugmentTier.MoonCoin, moonCoinCashOutPrice },
        { AugmentTier.PoopCoin, poopCoinCashOutPrice },
        { AugmentTier.TimeCoin, timeCoinCashOutPrice },
        { AugmentTier.InfinityCoin, infinityCoinCashOutPrice },
        { AugmentTier.GodCoin, godCoinCashOutPrice }
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

    // Basic
    public float PriceOfCashOutTier(AugmentTier tier)
    {
        return cashOutTierPrices.GetValueOrDefault(tier, 500);
    }
    
    private void StartUpgradeSelection(AugmentTier tier)
    {
        UpgradesSelectionUI.Instance.TriggerAugmentSelection(tier);
    }

    private void IncreaseCashOutTierPrice(AugmentTier tier)
    {
        Debug.Log($"Increased cashoutPrice of tier {tier} from {cashOutTierPrices[tier]} to {Mathf.RoundToInt(cashOutTierPrices[tier] * 1.1f)}");
        cashOutTierPrices[tier] *= 1.1f;
        if (tier <= AugmentTier.Legendary)
        {
            cashOutTierPrices[tier] = Mathf.RoundToInt(cashOutTierPrices[tier]);
        }
        else
        {
            cashOutTierPrices[tier] = Mathf.Round(cashOutTierPrices[tier] * 10f) / 10f;
        }
    }
    // Divine
    public float PriceOfDivineCashOutTier(PlayerCurrencies.Currency currency)
    {
        int index = (int)currency;
        AugmentTier tier = (AugmentTier)index + 4; // 4 = number of nonDivineCashOutTiers
        //Debug.Log($"Returning price for currency: {tier}: {cashOutTierPrices.GetValueOrDefault(tier, 500)}");
        return cashOutTierPrices.GetValueOrDefault(tier, 500);
    }
    
    /*private void StartUpgradeSelection(PlayerCurrencies.Currency currency)
    {
        UpgradesSelectionUI.Instance.TriggerAugmentSelection(currency);
    }*/

    private void IncreaseCashOutTierPrice(PlayerCurrencies.Currency currency)
    {
        int index = (int)currency;
        AugmentTier tier = (AugmentTier)index + 5;
        cashOutTierPrices[tier] = Mathf.RoundToInt(cashOutTierPrices[tier] * 1.05f);
    }

    private void OnEnable()
    {
        GameEvents.OnCashOut += StartUpgradeSelection;
        GameEvents.OnCashOut += IncreaseCashOutTierPrice;
    }
    
    private void OnDisable()
    {
        GameEvents.OnCashOut -= StartUpgradeSelection;
        GameEvents.OnCashOut -= IncreaseCashOutTierPrice;
    }
}
