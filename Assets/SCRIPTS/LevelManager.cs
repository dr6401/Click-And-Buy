using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Schema;
using DamageNumbersPro;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [SerializeField] private RectTransform priceChart;
    [SerializeField] private RectTransform candleArea;
    [SerializeField] private RectTransform tradeIndicatorArea;
    [SerializeField] private GameObject candlePrefab;
    [SerializeField] private GameObject tradeEntryIndicatorPrefab;
    
    [SerializeField] private RectTransform tradePanel;
    [SerializeField] private GameObject tradeEntryPrefab;

    public ChartController currentChart;
    public List<ChartController> allCharts = new List<ChartController>();
    
    public float cash = 1000f;
    public float effectiveCash;
    public float equity;
    public float openProfitAndLoss;
    public float unrealizedProfit;
    public float unrealizedLoss;
    public float leverage = 1;
    private float currentOrderQuantity = 1f;

    public PlayerCurrencies.Currency currentCurrency;

    public ArchivedFund currentFund;

    public float amountToWin = 1000000f;

    public float baseComboMultiplier = 0.01f;
    public float comboBonus = 0f;
    public int comboFeverIncrease = 0;

    private float passiveIncomeTimer;
    
    public CurrencyStats currencyStats;
    
    // Combo System
    private float comboTimer;
    private int comboAmount = 0;
    
    [Header("Upgrade System")]
    public AugmentTier currentBasicCashOutTier = AugmentTier.Common;
    public AugmentTier currentDivineCashOutTier = AugmentTier.Forex;
    public float currentBasicCashOutPrice = 300;
    
    public float currentRespinPrice = 50;

    public List<TradeEntryStatsDisplay> activeTrades = new List<TradeEntryStatsDisplay>();
    
    
    [SerializeField] private PriceMoveEvent tutorialPump;
    [SerializeField] private PriceMoveEvent tutorialDump;
    
    [Header("DamageNumbersPro")]
    [SerializeField] private DamageNumber profitDamageNumbersPrefab;
    [SerializeField] private DamageNumber lossDamageNumbersPrefab;
    [SerializeField] private DamageNumber textDamageNumbersPrefab;
    [SerializeField] private DamageNumber textDamageNumbersScatterPrefab;
    [SerializeField] private DamageNumber textDamageNumbersComboPrefab;
    [SerializeField] private DamageNumber textDamageNumbersComboBreakerPrefab;
    [SerializeField] private DamageNumber iconDamageNumbersTokensPrefab;

    public bool hasLevelEnded = false;
    public bool isInputBlocked = false;
    
    [Header("Canvas stuff")]
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text currentTokensText;
    [SerializeField] private TMP_Text openProfitLossText;
    [SerializeField] private TMP_Text basicCashOutText;
    [SerializeField] private TMP_Text divineCashOutText;
    [SerializeField] private Button cashOutButton;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private TMP_Text currentFaithText;
    //[SerializeField] private TMP_Text profitMultText;
    [SerializeField] private TMP_Text freebieTradesText;
    //[SerializeField] private TMP_Text volatilityText;
    //[SerializeField] private TMP_Text passiveIncomeText;
    //[SerializeField] private TMP_Text divineLuckText;
    //[SerializeField] private TMP_Text lossShieldText;
    [SerializeField] private TMP_Text quantityOrderText;
    [SerializeField] private Slider timeScaleSlider;

    [Header("Power Ups")]
    public int numberOfFutureFreebieTrades;

    [Header("Random Shit")]
    private BadTradeTextsSO badTradesSO;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        currencyStats = Resources.Load<CurrencyStats>("Currency/CurrencyStats");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        equity = cash;
        currentBasicCashOutPrice = UpgradesManager.Instance.PriceOfCashOutTier(currentBasicCashOutTier);
        //GenerateNewPrice();
        PlayPriceMoveEvent(currentChart, tutorialPump);
        //currentChart.SpawnNewCandle();
        badTradesSO = Resources.Load<BadTradeTextsSO>("BadTradesText/BadTradesTexts");
        currentCurrency = PlayerCurrencies.Currency.forex;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLevelEnded) return;

        foreach (ChartController chart in allCharts)
        {
            if (chart.hasChartBeenUnlocked)
            {
                chart.Tick();
            }
        }
        passiveIncomeTimer += Time.deltaTime;
        comboTimer += Time.unscaledDeltaTime;

        if (passiveIncomeTimer >= PlayerStats.Instance.passiveIncomeTriggerInterval)
        {
            passiveIncomeTimer = 0;
            if (PlayerStats.Instance.passiveIncome > 0)
            {
                cash += PlayerStats.Instance.passiveIncome;
                SpawnReceivedMoneyDamageNumbers(PlayerStats.Instance.passiveIncome);
            }
        }
        
        if (comboTimer > PlayerStats.Instance.maxComboDuration)
        {
            comboAmount = 0;
        }

        comboBonus = Mathf.Max(0, (comboAmount - 1) * 5f); // "-1" because we start actually comboing at 2x combo

        openProfitAndLoss = 0f;
        float invested = 0f;
        unrealizedProfit = 0;
        unrealizedLoss = 0;
        effectiveCash = cash;
        
        foreach (var trade in activeTrades)
        {
            openProfitAndLoss += trade.GetUnrealizedProfit();
            invested += trade.entryPrice * trade.quantity;
            effectiveCash += trade.GetLossBeyondMargin();
            //Debug.Log($"Open position: {trade.name} with P/L: {trade.GetUnrealizedProfit()}");
            if (trade.GetUnrealizedProfit() > 0) unrealizedProfit += trade.GetUnrealizedProfit();
            else if (trade.GetUnrealizedProfit() < 0) unrealizedLoss -= trade.GetUnrealizedProfit();
        }
        
        effectiveCash = Mathf.Max(0, effectiveCash);
        
        equity = cash + openProfitAndLoss + invested;

        priceText.text = $"Price: {NumberFormatter.FormatDecimalNumber(currentChart.price)}$";
        cashText.text = $"Cash: " + NumberFormatter.FormatDecimalNumber(effectiveCash) + "$";
        currentTokensText.text = $"Tokens: {NumberFormatter.FormatDecimalNumber(PlayerCurrencies.Instance.GetTokensAmount(currentCurrency))}";
        openProfitLossText.text = $"Open P/L: {NumberFormatter.FormatDecimalNumber(openProfitAndLoss)}$";
        basicCashOutText.text = $"{NumberFormatter.FormatDecimalNumber(UpgradesManager.Instance.PriceOfCashOutTier(currentBasicCashOutTier))}$";
        basicCashOutText.color = GetColorForCurrentTier();
        divineCashOutText.text = $"{NumberFormatter.FormatDecimalNumber(UpgradesManager.Instance.PriceOfDivineCashOutTier(currentCurrency))}";
        multiplierText.text = $"Multiplier: {NumberFormatter.FormatDecimalNumber(leverage)}X";
        currentFaithText.text = $"Faith: {NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.faith)}";
        //profitMultText.text = "Profit Mult: "+ NumberFormatter.FormatNumber((PlayerStats.Instance.profitMultiplier - 1) * 100f) + "%";
        freebieTradesText.text = "Freebie trades: " + NumberFormatter.FormatNumber(numberOfFutureFreebieTrades);
        //volatilityText.text = $"Volatility: {NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.volatility)}%";
        //passiveIncomeText.text = $"Passive: +{NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.passiveIncome)}/s";
        //divineLuckText.text = $"Divine Luck: {NumberFormatter.FormatNumber(PlayerStats.Instance.divineLuck * 100f)}%";
        //lossShieldText.text =  $"Loss Shield: {NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.lossReduction)}%";
        
        quantityOrderText.text = $"{NumberFormatter.FormatDecimalNumber(currentOrderQuantity)}";
        if (openProfitAndLoss > 0)
        {
            openProfitLossText.color = GameConstants.greenColor;
        }
        else if (openProfitAndLoss < 0)
        {
            openProfitLossText.color = GameConstants.redColor;
        }
        else openProfitLossText.color = GameConstants.lightGreyColor;
        
        if (!hasLevelEnded)
        {
            if (cash >= amountToWin)
            {
                hasLevelEnded = true;
                isInputBlocked = true;
                WinGame();
            }
            if (effectiveCash <= 0f && activeTrades.Count > 0)
            {
                MarginCall(); // Deletes trade with biggest loss
                if (effectiveCash <= 0 && activeTrades.Count <= 0 && equity <= 0)
                {
                    hasLevelEnded = true;
                    isInputBlocked = true;
                    LoseGame();
                }
            }
        }

        currentFund.valuation = CalculateCurrentFundValuation();

        if (GameConstants.isDevHacksEnabled) // Dev hacks
        {
            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                cash += 1000;
            }
            
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                PlayerStats.Instance.faith += 500;
            }

            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                currentChart.maxPrice += 50f;
            }
        }
    }
    
    public void Buy()
    {
        if (isInputBlocked) return;
        SpendMoney(TradeType.Buy);
    }
    
    public void Sell()
    {
        if (isInputBlocked) return;
        SpendMoney(TradeType.Sell);
    }
    
    public void SpendMoney(TradeType tradeType)
    {
        float cost = currentChart.price * currentOrderQuantity;
        if (cash < cost && !IsNextTradeFree())
        {
            GameEvents.onNotEnoughMoney?.Invoke();
            Debug.Log("Not enough money");
            return;
        }

        if (PlayerStats.Instance != null && activeTrades.Count >= PlayerStats.Instance.maxAliveTrades)
        {
            string displayText = $"Max Trades {activeTrades.Count}/{PlayerStats.Instance.maxAliveTrades}!";
            SpawnTextDamageNumbers(displayText, tradePanel, Anchor01ToScreen(tradePanel, new Vector2(1, 0)));
            GameEvents.onNotEnoughAliveTrades?.Invoke();
            //Debug.Log($"Max amount of alive trades reached: {PlayerStats.Instance.maxAliveTrades}! ");
            return;
        }

        if (IsNextTradeFree())
        {
            numberOfFutureFreebieTrades--;
            SpawnLostMoneyDamageNumbers(0);
            Debug.Log($"Used freebie, cash stays the same");
        }
        else
        {
            Debug.Log($"Money: {cash} - Cost: {cost} (Price: {currentChart.price} * Quantity: {currentOrderQuantity}) = Current Money: {cash - currentChart.price}");
            cash -= cost;
            cash = Mathf.Clamp(cash, 0f, cash);
            SpawnLostMoneyDamageNumbers(cost);
        }
        GameObject tradeEntry = Instantiate(tradeEntryPrefab, tradePanel);
        TradeEntryStatsDisplay stats = tradeEntry.GetComponent<TradeEntryStatsDisplay>();
        TradeData data = new TradeData(tradeType, System.DateTime.Now.ToString("HH:mm:ss"), currentOrderQuantity, currentChart.price, leverage, currentCurrency);
        
        GameObject tradeEntryIndicator = Instantiate(currentChart.tradeEntryIndicatorPrefab, currentChart.tradeIndicatorArea);
        RectTransform rectTransform = tradeEntryIndicator.GetComponent<RectTransform>();
        CandleData tradeEntryIndicatorData = tradeEntryIndicator.GetComponent<CandleData>();
        tradeEntryIndicatorData.open = currentChart.price;

        rectTransform.anchoredPosition = new Vector2(currentChart.xPos, currentChart.PriceToY(currentChart.price));
        
        Image indicatorImage = tradeEntryIndicator.GetComponent<Image>();
        if (tradeType == TradeType.Buy)
        {
            indicatorImage.color = GameConstants.greenColor;
        }
        else if (tradeType == TradeType.Sell)
        {
            indicatorImage.color = GameConstants.redColor;
        }
        
        stats.LinkTradeEntryIndicator(tradeEntryIndicator);
        currentChart.tradeEntryIndicators.Add(rectTransform);
        
        stats.Setup(data);
        activeTrades.Add(stats);
        GameEvents.onMoneySpent?.Invoke();
    }

    public void CloseTrade(TradeEntryStatsDisplay trade)
    {
        if (isInputBlocked) return;
        if (trade.GetUnrealizedProfit() > 0)
        {
            GameEvents.onMoneyEarned?.Invoke();
            comboAmount++;
            comboAmount += comboFeverIncrease;
            if (comboAmount > 1) // If combo is at least 2
            {
                float comboBonusMoney = comboBonus * baseComboMultiplier * trade.GetUnrealizedProfit();
                if (comboBonusMoney >= 1) // and we would get at least 1$ from bonus
                {
                    Debug.Log($"trade.GetUnrealizedProfit: {trade.GetUnrealizedProfit()},  comboBonus: {comboBonus}, comboBonusMoney: {comboBonusMoney}");
                    SpawnReceivedMoneyDamageNumbers(comboBonusMoney, anchor: new Vector2(-300f, 25f));
                }
                else
                {
                    Debug.Log($"Combo Amount: {comboAmount},  comboBonus: {comboBonus}, comboBonusMoney: {comboBonusMoney}");
                }

                if (comboBonus > 0)
                {
                    float comboBonusTokens = Mathf.Max(0.01f, comboBonusMoney * 0.1f);
                    PlayerCurrencies.Instance.AddCurrency(comboBonusTokens, trade.currencyTraded);
                    SpawnReceivedTokensDamageNumbers(comboBonusTokens, icon: currencyStats.GetIconOfCurrency(trade.currencyTraded));
                }
            }
        }
        else if (trade.GetUnrealizedProfit() < 0) GameEvents.onMoneyLost?.Invoke();
        activeTrades.Remove(trade);
        if (trade.tradeEntryIndicator != null)
        {
            ChartController chartAffected = GetChartControllerOfCurrency(trade.currencyTraded);
            chartAffected.tradeEntryIndicators.Remove(trade.tradeEntryIndicator.GetComponent<RectTransform>());
        }
    }

    public void DuplicateTrade(TradeEntryStatsDisplay trade)
    {
        GameObject tradeEntry = Instantiate(tradeEntryPrefab, tradePanel);
        TradeEntryStatsDisplay stats = tradeEntry.GetComponent<TradeEntryStatsDisplay>();
        TradeData data = new TradeData(trade.tradeType, System.DateTime.Now.ToString("HH:mm:ss"), trade.quantity, trade.entryPrice, trade.multiplier, currentCurrency);

        ChartController chartToAffect = GetChartControllerOfCurrency(data.tradedCurrency);
        GameObject tradeEntryIndicator = Instantiate(chartToAffect.tradeEntryIndicatorPrefab, chartToAffect.priceChart);
        RectTransform rectTransform = tradeEntryIndicator.GetComponent<RectTransform>();
        CandleData tradeEntryIndicatorData = tradeEntryIndicator.GetComponent<CandleData>();
        tradeEntryIndicatorData.open = trade.entryPrice;

        rectTransform.anchoredPosition = new Vector2(chartToAffect.xPos, chartToAffect.PriceToY(trade.entryPrice));
        
        Image indicatorImage = tradeEntryIndicator.GetComponent<Image>();
        if (trade.tradeType == TradeType.Buy)
        {
            indicatorImage.color = GameConstants.greenColor;
        }
        else if (trade.tradeType == TradeType.Sell)
        {
            indicatorImage.color = GameConstants.redColor;
        }
        
        stats.LinkTradeEntryIndicator(tradeEntryIndicator);
        chartToAffect.tradeEntryIndicators.Add(rectTransform);
        
        stats.Setup(data);
        activeTrades.Add(stats);
        Debug.Log($"Duplicated trade from {trade.timeOfPurchase}");
    }

    public void MarginCall()
    {
        TradeEntryStatsDisplay worstTrade = GetWorstTrade();
        if (worstTrade.GetUnrealizedProfit() > 0) return;
        Debug.Log($"Current Cash: {cash}, current effectiveCash: {effectiveCash}, equity: {equity}");
        Debug.Log($"Gonna close trade in position {activeTrades.IndexOf(worstTrade)}");
        Debug.Log($"Investment returned from closing position: {worstTrade.entryPrice * worstTrade.quantity} ({worstTrade.entryPrice} * {worstTrade.quantity})\n" +
                  $"Loss from trade: {worstTrade.GetUnrealizedProfit()}");
        worstTrade.Close();
        RecalculateBalances();
        Debug.Log($"Cash: {cash}, Equity: {equity}");
    }

    public TradeEntryStatsDisplay GetWorstTrade()
    {
        if (activeTrades.Count <= 0) return null;
        TradeEntryStatsDisplay worstTrade = activeTrades[0];
        foreach (var trade in activeTrades)
        {
            if (trade.GetUnrealizedProfit() < worstTrade.GetUnrealizedProfit())
            {
                worstTrade = trade;
            }
        }
        return worstTrade;
    }

    private void RecalculateBalances()
    {
        float openProfitAmount = 0;
        float investedAmount = 0;
        effectiveCash = cash;
        foreach (var trade in activeTrades)
        {
            openProfitAmount += trade.GetUnrealizedProfit();
            investedAmount += trade.entryPrice * trade.quantity;
            effectiveCash += trade.GetLossBeyondMargin();
            //Debug.Log($"Open position: {trade.name} with P/L: {trade.GetUnrealizedProfit()}");
        }
        
        effectiveCash = Mathf.Max(0, effectiveCash);
        
        equity = cash + openProfitAmount + investedAmount;
    }

    public void BasicCashOut()
    {
        if (PauseManager.Instance.ShouldInputBeBlocked()) return;
        float currentTierCashOutPrice =
            UpgradesManager.Instance.PriceOfCashOutTier(currentBasicCashOutTier);
        if (effectiveCash < currentTierCashOutPrice)
        {
            GameEvents.onNotEnoughMoney?.Invoke();
            return;
        }

        if (PowerUpInventoryManager.Instance.AreAllSlotsFull())
        {
            SpawnTextDamageNumbers("Offering inventory full!", position: cashOutButton.gameObject.GetComponent<RectTransform>(), color: Color.white);
            return;
        }
        cash -= currentTierCashOutPrice;
        cash = Mathf.Max(0, cash);
        currentRespinPrice = UpgradesSelectionUI.Instance.baseAugmentRespinPrices[currentBasicCashOutTier];
        GameEvents.OnCashOut?.Invoke(currentBasicCashOutTier);
    }
    
    public void DivineCashOut()
    {
        if (PauseManager.Instance.ShouldInputBeBlocked()) return;
        if (PlayerCurrencies.Instance.GetTokensAmount(currentCurrency) < UpgradesManager.Instance.PriceOfDivineCashOutTier(currentCurrency))
        {
            GameEvents.onNotEnoughTokens?.Invoke();
            return;
        }

        if (PowerUpInventoryManager.Instance.AreAllSlotsFull())
        {
            SpawnTextDamageNumbers("Offering inventory full!", position: cashOutButton.gameObject.GetComponent<RectTransform>(), color: Color.white);
            return;
        }

        float costOfCashOut = UpgradesManager.Instance.PriceOfDivineCashOutTier(currentCurrency);
        PlayerCurrencies.Instance.AddCurrency(-costOfCashOut, currentCurrency);
        PlayerStats.Instance.faith += 0.1f * costOfCashOut;
        currentRespinPrice = UpgradesSelectionUI.Instance.baseAugmentRespinPrices[currentBasicCashOutTier];
        GameEvents.OnCashOut?.Invoke(currentDivineCashOutTier);
        GameEvents.OnDivineCashOut?.Invoke();
        Debug.Log($"Spent {UpgradesManager.Instance.PriceOfDivineCashOutTier(currentCurrency)} {currentCurrency} tokens");
    }

    public ChartController GetChartControllerOfCurrency(PlayerCurrencies.Currency currency)
    {
        foreach (ChartController chart in allCharts)
        {
            if (currency == chart.chartCurrency)
            {
                return chart;
            }
        }
        Debug.LogError($"Couldn't find chart with currency {currency}. Returning current chart: {currentChart}");
        return currentChart;
    }
    
    public void WinGame()
    {
        Debug.Log($"Win Game");
        GameEvents.onVictory?.Invoke();
        victoryCanvas.SetActive(true);
    }
    public void LoseGame()
    {
        Debug.Log($"Lost Game");
        GameEvents.onDefeat?.Invoke();
        loseCanvas.SetActive(true);
    }

    public void ChangeTimeScale()
    {
        if (Time.timeScale <= 0) return;
        float scale = timeScaleSlider.value;
        if (scale <= 0.25f)
        {
            Time.timeScale = Mathf.Lerp(0.25f, 1, scale / 0.25f);
        }
        else if (scale <= 0.5f)
        {
            Time.timeScale = Mathf.Lerp(1, 20, (scale - 0.25f) / 0.25f);
        }
        else if (scale <= 0.75f)
        {
            Time.timeScale = Mathf.Lerp(20, 50, (scale - 0.5f) / 0.25f);
        }
        else
        {
            Time.timeScale = Mathf.Lerp(50, 100, (scale - 0.75f) / 0.25f);
        }
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
        timeScaleSlider.value = 0.25f;
    }

    public void IncreaseLeverage()
    {
        if (isInputBlocked) return;

        if (leverage >= PlayerStats.Instance.maxLeverage)
        {
            SpawnTextDamageNumbers($"Max Multiplier!", position: multiplierText.gameObject.GetComponent<RectTransform>(), anchor: new Vector2(50, 0));
        }
        if (leverage < 1) leverage += 0.5f;
        else leverage += 1f;
        leverage = Mathf.Clamp(leverage, 0.5f, PlayerStats.Instance.maxLeverage);
    }
    public void DecreaseLeverage()
    {
        if (isInputBlocked) return;
        leverage -= 1f;
        leverage = Mathf.Clamp(leverage, 0.5f, PlayerStats.Instance.maxLeverage);
    }

    public void IncreaseCashOutTier()
    {
        currentBasicCashOutTier++;
        currentBasicCashOutTier = (AugmentTier) Mathf.Min((int) currentBasicCashOutTier, Enum.GetValues(typeof(AugmentTier)).Length - 1);
        currentBasicCashOutPrice = UpgradesManager.Instance.PriceOfCashOutTier(currentBasicCashOutTier);
    }
    
    public void DecreaseCashOutTier()
    {
        currentBasicCashOutTier--;
        currentBasicCashOutTier = (AugmentTier) Mathf.Max((int) currentBasicCashOutTier, 0);
        currentBasicCashOutPrice = UpgradesManager.Instance.PriceOfCashOutTier(currentBasicCashOutTier);
    }

    public void ChangeOrderQuantity(bool increase)
    {
        float step;
        if (increase)
        {
            if (currentOrderQuantity < 0.25f) step = 0.05f;
            else if (currentOrderQuantity < 1) step = 0.25f;
            else if (currentOrderQuantity < 10) step = 1f;
            else if (currentOrderQuantity < 50f) step = 5;
            else step = 10f;
            currentOrderQuantity += step;
        }
        else
        {
            if (currentOrderQuantity <= 0.25f) step = 0.05f;
            else if (currentOrderQuantity <= 1) step = 0.25f;
            else if (currentOrderQuantity <= 10) step = 1f;
            else if (currentOrderQuantity <= 50f) step = 5;
            else step = 10f;
            currentOrderQuantity -= step;
        }
        currentOrderQuantity = Mathf.Clamp(currentOrderQuantity, 0.1f, PlayerStats.Instance.maxOrderQuantity);
    }

    public void PlayPriceMoveEvent(ChartController chart, PriceMoveEvent priceMoveEvent)
    {
        if (priceMoveEvent.targetPricePercentIncrease != 0)
        {
            priceMoveEvent.targetPrice = Mathf.Max(chart.minPrice, (1 + priceMoveEvent.targetPricePercentIncrease) * chart.price);
        }
        Debug.Log($"{priceMoveEvent.name} event targetPrice: {priceMoveEvent.targetPrice}");
        chart.activeEvent.data = priceMoveEvent;

        chart.activeEvent.startPrice = currentChart.price;
        chart.activeEvent.elapsed = 0;

        chart.activeEvent.isTargetHigherThanCurrentPrice = currentChart.price < priceMoveEvent.targetPrice;

        chart.activeEvent.active = true;
    }

    private void HandleCombo()
    {
        if (comboTimer > PlayerStats.Instance.maxComboDuration)
        {
            comboAmount = 1 + comboFeverIncrease;
        }
        comboTimer = 0;
        if (comboAmount > 1)
        {
            SpawnComboTextDamageNumbers(number: comboAmount, dmgNumberPrefab: textDamageNumbersComboPrefab);
        }
    }

    private void BreakCombo()
    {
        if (comboAmount > 1)
        {
            SpawnComboTextDamageNumbers(badTradesSO.GetRandomBadTradesText(), color: GameConstants.redColor, dmgNumberPrefab: textDamageNumbersComboBreakerPrefab);
        }
        comboAmount = 0;
    }

    public void SetInputUnblocked()
    {
        isInputBlocked = false;
    }
    
    public void SetInputBlocked()
    {
        isInputBlocked = true;
    }

    public void SpawnReceivedMoneyDamageNumbers(float amount, RectTransform position = null, Vector2 anchor = new Vector2())
    {
        if (position == null) position = cashText.rectTransform;
        RectTransform gameCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
        DamageNumber newDamageNumber = profitDamageNumbersPrefab.SpawnGUI(gameCanvas, position, anchor, amount);
    }
    public void SpawnLostMoneyDamageNumbers(float amount, RectTransform position = null, Vector2 anchor = new Vector2())
    {
        if (position == null) position = cashText.rectTransform;
        RectTransform gameCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
        DamageNumber newDamageNumber = lossDamageNumbersPrefab.SpawnGUI(gameCanvas, position, anchor, amount);
    }

    public void SpawnReceivedTokensDamageNumbers(float amount, RectTransform position = null, Vector2 anchor = new Vector2(), Sprite icon = null)
    {
        SpawnTextDamageNumbers("",  damageNumberPrefab: iconDamageNumbersTokensPrefab, spawnIcon: true, number: amount, icon: icon);
        Debug.Log($"Spawning received TOKENS prefab");
    }
    
    public void SpawnTextDamageNumbers(string text, RectTransform position = null, Vector2 anchor = new Vector2(), RectTransform canvasParent = null, DamageNumber damageNumberPrefab = null, Color? color = null, bool spawnAtLatestCandle = false, bool scatterTextOnSpawn = false, bool spawnComboText = false, bool spawnIcon = false, float number = 0, Sprite icon = null)
    {
        if (position == null) position = cashText.rectTransform;
        if (spawnAtLatestCandle) position = currentChart.currentCandle;
        if (canvasParent == null) canvasParent = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
        if (damageNumberPrefab == null) damageNumberPrefab = textDamageNumbersPrefab; 
        DamageNumber newDamageNumber;
        if (scatterTextOnSpawn)
        {
            newDamageNumber = textDamageNumbersScatterPrefab.SpawnGUI(canvasParent, position, anchor, text);
        }
        else if (spawnComboText)
        {
            anchor = new Vector2(-300f, -125f);
            if (damageNumberPrefab == textDamageNumbersComboBreakerPrefab)
            {
                anchor = new Vector2(-300f, -25f);
            }
            if (number <= 0) // If it's a combo breaker text
            {
                newDamageNumber = damageNumberPrefab.SpawnGUI(canvasParent, position, anchor, text);   
            }
            else // Else it's a normal combo text
            {
                newDamageNumber = damageNumberPrefab.SpawnGUI(canvasParent, position, anchor, number);
            }
            newDamageNumber.lifetime = PlayerStats.Instance.maxComboDuration;
        }
        else if (spawnIcon)
        {
            anchor = new Vector2(-300f, -25f);
            newDamageNumber = damageNumberPrefab.SpawnGUI(canvasParent, position, anchor, number);
            newDamageNumber.lifetime = PlayerStats.Instance.maxComboDuration;
            Image dmgNumIcon = newDamageNumber.GetComponentInChildren<Image>();
            if (dmgNumIcon != null)
            {
                dmgNumIcon.sprite = icon;   
            }
            else
            {
                Debug.Log($"No Image component on dmg numbers object");
            }
        }
        else
        {
            newDamageNumber = damageNumberPrefab.SpawnGUI(canvasParent, position, anchor, text);
            Debug.Log($"Spawned basic damageNumberPrefab");
        }
        if (color.HasValue)
        {
            newDamageNumber.SetColor(color.Value);
        }
        Debug.Log($"Spawned dmgNumbersProf with prefab: {damageNumberPrefab.ToString()}");
    }

    public void SpawnComboTextDamageNumbers(string text = "", Color? color = null, DamageNumber dmgNumberPrefab = null, float number = 0)
    {
        SpawnTextDamageNumbers(text, spawnComboText: true, color: color, damageNumberPrefab: dmgNumberPrefab, number: number);
    }

    private bool IsNextTradeFree()
    {
        return numberOfFutureFreebieTrades > 0;
    }

    private Vector2 Anchor01ToScreen(RectTransform rect, Vector2 anchor01)
    {
        return new Vector2(
            rect.rect.width * anchor01.x,
            rect.rect.height * anchor01.y
        );
    }

    private Color GetColorForCurrentTier()
    {
        switch (currentBasicCashOutTier)
        {
            case AugmentTier.Common:
                return GameConstants.commonTierColor;
                break;
            case AugmentTier.Rare:
                return GameConstants.rareTierColor;
                break;
            case AugmentTier.Epic:
                return GameConstants.epicTierColor;
                break;
            case AugmentTier.Legendary:
                return GameConstants.divineBlessingColor;
                break;
            default:
                return GameConstants.whiteColor;
        }
    }

    public void ResetLvlManagerValuesAtFundSell()
    {
        CloseAndClearAllCandlesForAllCharts();
        CloseAndClearAllTrades();
        
        passiveIncomeTimer = 0;
        
        leverage = 1;
        currentOrderQuantity = 1;

        currentBasicCashOutTier = AugmentTier.Common;
        currentCurrency = PlayerCurrencies.Currency.forex;
        currentFund.highestUnlockedCurrency = PlayerCurrencies.Currency.forex;
        UpgradesManager.Instance.ResetTierPricesToOriginal();
        
        currentBasicCashOutPrice = UpgradesManager.Instance.PriceOfCashOutTier(currentBasicCashOutTier);

        ResetTimeScale();

        currentFund = new ArchivedFund();
        currentFund.DebugFundStats();
        
        foreach (ChartController chart in allCharts)
        {
            chart.ResetChartValuesAtFundSell();
        }
    }

    private void CloseAndClearAllTrades()
    {
        List<TradeEntryStatsDisplay> tempActiveTrades = new List<TradeEntryStatsDisplay>(activeTrades);
        foreach (var trade in tempActiveTrades)
        {
            trade.Close();
        }
        activeTrades.Clear();
    }

    private void CloseAndClearAllCandlesForAllCharts()
    {
        foreach (ChartController chart in allCharts)
        {
            chart.CloseAndClearAllCandles();
        }
    }

    public List<TradeEntryStatsDisplay> GetAllActiveTradesOfCurrentCurrency()
    {
        List<TradeEntryStatsDisplay> tempActiveTrades = new List<TradeEntryStatsDisplay>();
        foreach (TradeEntryStatsDisplay trade in activeTrades)
        {
            if (trade.currencyTraded == currentCurrency)
            {
                tempActiveTrades.Add(trade);
            }
        }
        return tempActiveTrades;
    }

    private float CalculateCurrentFundValuation()
    {
        if (equity * PlayerStats.Instance.fundValuationMultiplier < 0.1f) return 0;
        return equity * PlayerStats.Instance.fundValuationMultiplier;
    }

    private float CalculateMoneyTransferredFromPreviousFund()
    {
        if (equity * PlayerStats.Instance.moneyTransferMultiplier < 10) return 0;
        return equity * PlayerStats.Instance.moneyTransferMultiplier;
    }

    public void SwitchChart(PlayerCurrencies.Currency currency)
    {
        foreach (ChartController chart in allCharts)
        {
            chart.priceChart.gameObject.SetActive(false);
        }
        foreach (ChartController chart in allCharts)
        {
            if (chart.chartCurrency == currency)
            {
                currentChart = chart;
                currentCurrency = currency;
                chart.priceChart.gameObject.SetActive(true);
                return;
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.OnUpgradesOffered += SetInputBlocked;
        GameEvents.OnUpgradeChosen += SetInputUnblocked;
        GameEvents.onMoneyEarned += HandleCombo;
        GameEvents.onMoneyLost += BreakCombo;
    }
    
    private void OnDisable()
    {
        GameEvents.OnUpgradesOffered -= SetInputBlocked;
        GameEvents.OnUpgradeChosen -= SetInputUnblocked;
        GameEvents.onMoneyEarned -= HandleCombo;
        GameEvents.onMoneyLost -= BreakCombo;
    }
}
