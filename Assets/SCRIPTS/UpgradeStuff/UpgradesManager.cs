using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    private float silverAugmentChance = 0.5f;
    private float goldAugmentChance = 0.3f;
    
    // Basic
    public static float basicFirstCashoutPrice = 100f;
    public static float basicSecondCashoutPrice = 500f;
    public static float basicThirdCashoutPrice = 1000f;
    public static float basicFourthCashoutPrice = 2500f;
    public static float basicFifthCashoutPrice = 5000f;
    public static float basicSixthCashoutPrice = 10000f;
    public static float basicSeventhCashoutPrice = 25000f;
    public static float basicEighthCashoutPrice = 50000f;
    public static float basicNinthCashoutPrice = 100000f;
    public static float basicTenthCashoutPrice = 250000f;
    
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
    
    public Dictionary<AugmentTier, float> originalCashOutTierPrices = new Dictionary<AugmentTier, float>()
    {
        { AugmentTier.BasicFirst, basicFirstCashoutPrice },
        { AugmentTier.BasicSecond, basicSecondCashoutPrice },
        { AugmentTier.BasicThird, basicThirdCashoutPrice },
        { AugmentTier.BasicForth, basicFourthCashoutPrice },
        { AugmentTier.BasicFifth, basicFifthCashoutPrice },
        { AugmentTier.BasicSixth, basicSixthCashoutPrice },
        { AugmentTier.BasicSeventh, basicSeventhCashoutPrice },
        { AugmentTier.BasicEighth, basicEighthCashoutPrice },
        { AugmentTier.BasicNinth, basicNinthCashoutPrice },
        { AugmentTier.BasicTenth, basicTenthCashoutPrice },
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


    public Dictionary<AugmentTier, float> cashOutTierPrices = new Dictionary<AugmentTier, float>();
    
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

        foreach (var entry in originalCashOutTierPrices)
        {
            cashOutTierPrices.Add(entry.Key, entry.Value);
        }
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
        if (tier <= AugmentTier.BasicForth)
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

    public void ResetTierPricesToOriginal()
    {
        Debug.Log($"Resetting cashOutTier prices to original");
        cashOutTierPrices.Clear();
        foreach (var entry in originalCashOutTierPrices)
        {
            cashOutTierPrices.Add(entry.Key, entry.Value);
        }
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
