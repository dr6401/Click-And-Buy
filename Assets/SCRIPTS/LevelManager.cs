using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    
    public float cash = 1000f;
    public float effectiveCash;
    public float equity;
    public float openProfitAndLoss;
    public float unrealizedProfit;
    public float unrealizedLoss;
    [Header("-----------------PRICE----------------")]
    public float price;
    [Header("-----------------PRICE----------------")]
    public float upgradeOfferTarget = 300;
    private float upgradeThresholdIncrease = 500;
    public float leverage = 1;
    private float previousPrice;
    private float decimals = 0.01f;
    private float currentOrderQuantity = 1f;
    private float maxOrderQuantity = 10f;

    public PlayerCurrencies.Currency currentCurrency;

    public float amountToWin = 1000000f;
    public float minPrice = 10f;
    public float maxPrice = 200f;

    private float maxPriceIncreaseTimer;
    public float maxPriceIncreaseInterval = 10f;
    public float maxPriceIncreaseAmount = 1f;

    public float comboBonus = 0f;

    private float passiveIncomeTimer;

    private float generatePriceTimer;
    private float genetartePriceInterval = 0.1f;
    private float marginCallTimer;
    private float marginCallInterval = 0.001f;
    
    // Combo System
    private float comboTimer;
    private int comboAmount = 0;
    
    [Header("Upgrade System")]
    public AugmentTier currentCashOutTier = AugmentTier.Common;
    public float currentCashOutPrice = 300;
    
    public float currentRespinPrice = 50;

    [Header("DEBUG")]
    public bool stopGeneratingPrice;

    private List<RectTransform> candles = new List<RectTransform>();
    private List<RectTransform> tradeEntryIndicators = new List<RectTransform>();
    public List<TradeEntryStatsDisplay> activeTrades = new List<TradeEntryStatsDisplay>();
    
    [Header("Candle Spawn Settings")]
    private float xPos = 30;
    private float xStep = 50; // Distance between candles
    
    private float candleSpawnTimer;
    public float candleSpawnInterval;
    

    private RectTransform currentCandle;
    private float candleOpen;
    private float candleHigh;
    private float candleLow;
    private float candleTimer;
    
    [Header("Chart Settings")]
    [SerializeField] private GameObject gridLinePrefab;
    [SerializeField] private RectTransform gridLinesParent;
    
    [SerializeField] private int lastNPrices = 50;
    [SerializeField] private float yChartPadding = 0.2f;

    private float chartMinVisible;
    private float chartMaxVisible;
    public float gridStep = 25f;
    
    private List<float> recentPrices = new List<float>();
    
    [Header("Price Move Events")]
    private ActivePriceEvent activeEvent = new ActivePriceEvent();
    
    [SerializeField] private PriceMoveEvent tutorialPump;
    [SerializeField] private PriceMoveEvent tutorialDump;
    
    [Header("DamageNumbersPro")]
    [SerializeField] private DamageNumber profitDamageNumbersPrefab;
    [SerializeField] private DamageNumber lossDamageNumbersPrefab;
    [SerializeField] private DamageNumber textDamageNumbersPrefab;
    [SerializeField] private DamageNumber textDamageNumbersScatterPrefab;
    [SerializeField] private DamageNumber textDamageNumbersComboPrefab;
    [SerializeField] private DamageNumber textDamageNumbersComboBreakerPrefab;

    public bool hasLevelEnded = false;
    public bool isInputBlocked = false;
    
    [Header("Canvas stuff")]
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private TMP_Text openProfitLossText;
    [SerializeField] private TMP_Text cashOutText;
    [SerializeField] private Button cashOutButton;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private TMP_Text streakBonusText;
    [SerializeField] private TMP_Text profitMultText;
    [SerializeField] private TMP_Text freebieTradesText;
    [SerializeField] private TMP_Text volatilityText;
    [SerializeField] private TMP_Text passiveIncomeText;
    [SerializeField] private TMP_Text divineLuckText;
    [SerializeField] private TMP_Text lossShieldText;
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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        equity = cash;
        price = 50;//(minPrice * 0.75f + maxPrice * 0.25f);
        price = Mathf.Round(price / decimals) * decimals;
        previousPrice = price;
        chartMinVisible = 10;
        chartMaxVisible = 200;
        currentCashOutPrice = UpgradesManager.Instance.PriceOfCashOutTier(currentCashOutTier);
        recentPrices.Add(price);
        //GenerateNewPrice();
        PlayPriceMoveEvent(tutorialPump);
        SpawnNewCandle();
        badTradesSO = Resources.Load<BadTradeTextsSO>("BadTradesText/BadTradesTexts");
        currentCurrency = PlayerCurrencies.Currency.forex;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLevelEnded) return;

        candleTimer += Time.deltaTime;
        generatePriceTimer += Time.deltaTime;
        marginCallTimer += Time.deltaTime;
        passiveIncomeTimer += Time.deltaTime;
        comboTimer += Time.unscaledDeltaTime;
        if (Time.timeScale > 0) maxPriceIncreaseTimer += Time.unscaledDeltaTime; // If game isn't paused/powerups picking: increase this slowly, independently of time scale
        
        UpdateCurrentCandle();

        if (candleTimer >= candleSpawnInterval)
        {
            candleTimer = 0;
            FinalizeCandle();
        }
        
        if (generatePriceTimer >= genetartePriceInterval)
        {
            generatePriceTimer = 0;
            if (!stopGeneratingPrice) GenerateNewPrice();
        }
        candleHigh = Mathf.Max(candleHigh, price);
        candleLow = Mathf.Min(candleLow, price);

        if (maxPriceIncreaseTimer >= maxPriceIncreaseInterval)
        {
            maxPriceIncreaseTimer = 0;
            maxPrice += maxPriceIncreaseAmount;
        }

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

        priceText.text = $"Price: {NumberFormatter.FormatDecimalNumber(price)}$";
        cashText.text = $"Cash: " + NumberFormatter.FormatDecimalNumber(effectiveCash) + "$";
        targetText.text = $"Target: {NumberFormatter.FormatDecimalNumber(amountToWin)}$";
        openProfitLossText.text = $"Open P/L: {NumberFormatter.FormatDecimalNumber(openProfitAndLoss)}$";
        cashOutText.text = $"{currentCashOutTier}: {NumberFormatter.FormatDecimalNumber(UpgradesManager.Instance.PriceOfCashOutTier(currentCashOutTier))}$";
        cashOutText.color = GetColorForCurrentTier();
        multiplierText.text = $"Multiplier: {NumberFormatter.FormatDecimalNumber(leverage)}X";
        streakBonusText.text = $"Combo Bonus: +{NumberFormatter.FormatDecimalNumber(comboBonus)}%";
        profitMultText.text = "Profit Mult: "+ NumberFormatter.FormatNumber((PlayerStats.Instance.moneyGainMultiplier - 1) * 100f) + "%";
        freebieTradesText.text = "Freebie trades: " + NumberFormatter.FormatNumber(numberOfFutureFreebieTrades);
        volatilityText.text = $"Volatility: {NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.volatility)}%";
        passiveIncomeText.text = $"Passive: +{NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.passiveIncome)}/s";
        divineLuckText.text = $"Divine Luck: {NumberFormatter.FormatNumber(PlayerStats.Instance.divineLuck * 100f)}%";
        lossShieldText.text =  $"Loss Shield: {NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.lossReduction)}%";
        
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

        ToggleCashOutButtonEnabled();
        DrawGridLines();
        
        if (!hasLevelEnded)
        {
            if (cash >= amountToWin)
            {
                hasLevelEnded = true;
                isInputBlocked = true;
                WinGame();
            }
            if (effectiveCash <= 0f && activeTrades.Count > 0)// && marginCallTimer >= marginCallInterval)
            {
                marginCallTimer = 0;
                MarginCall(); // Deletes trade with biggest loss
                if (effectiveCash <= 0 && activeTrades.Count <= 0 && equity <= 0)
                {
                    hasLevelEnded = true;
                    isInputBlocked = true;
                    LoseGame();
                }
            }
        }

        if (GameConstants.isDevHacksEnabled) // Dev hacks
        {
            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                cash += 1000;
            }

            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                maxPrice += 50f;
            }
        }
    }

    private void GenerateNewPrice()
    {
        previousPrice = price;
        
        // Price Move Event
        if (activeEvent.active && activeEvent != null)
        {
            PriceMoveEvent priceEvent = activeEvent.data;

            activeEvent.elapsed += genetartePriceInterval;

            float t = Mathf.Clamp01(activeEvent.elapsed / priceEvent.duration);
            float curveT = priceEvent.animationCurve.Evaluate(t);

            float desiredPrice = Mathf.Lerp(activeEvent.startPrice, priceEvent.targetPrice, curveT);

            float move = desiredPrice - price;
            move = Mathf.Clamp(move, -priceEvent.maxStep, priceEvent.maxStep);
            
            float noise = Random.Range(-priceEvent.maxNoise,  priceEvent.maxNoise);
            price += move + noise;

            if (t >= 1) // If elapsedTime >= priceEvent.duration
            {
                price = priceEvent.targetPrice;
                activeEvent.active = false;
                Debug.Log($"Event: {priceEvent.name} finished at price: {price} (priceEvent.targetPrice was: {priceEvent.targetPrice})!");
            }
        }
        else
        {
            price += Random.Range(-5f, 5f);    
        }
        
        price = Mathf.Round(price / decimals) * decimals;
        price = Mathf.Clamp(price, minPrice, maxPrice);
        //Debug.Log($"New price: {price}");
        
        recentPrices.Add(price);
        if (recentPrices.Count > lastNPrices)
        {
            recentPrices.RemoveAt(0);
        }

        UpdateChartRange();
    }

    public void SpawnNewCandle()
    {
        float width = priceChart.rect.width * 0.8f; // So we don't spawn candles at utmost left position of the chart but a bit more in the middle
        if (xPos > width) // move everything to left and delete 1st candle
        {
            for (int i = 0; i < candles.Count; i++)
            {
                candles[i].anchoredPosition -= new Vector2(xStep, 0);
            }

            if (candles.Count > 0 && candles[0].anchoredPosition.x < 0)
            {
                Destroy(candles[0].gameObject);
                candles.RemoveAt(0);
            }
            
            for (int i = 0; i < tradeEntryIndicators.Count; i++)
            {
                tradeEntryIndicators[i].anchoredPosition -= new Vector2(xStep, 0);
            }

            if (tradeEntryIndicators.Count > 0 && tradeEntryIndicators[0].anchoredPosition.x < 0)
            {
                Destroy(tradeEntryIndicators[0].gameObject);
                tradeEntryIndicators.RemoveAt(0);
            }
            

            xPos -= xStep;
        }
        
        GameObject candle = Instantiate(candlePrefab, candleArea);
        currentCandle = candle.GetComponent<RectTransform>();
        CandleData data = candle.GetComponent<CandleData>();

        candleOpen = price;
        candleHigh = price;
        candleLow = price;
        data.open = price;

        currentCandle.anchoredPosition = new Vector2(xPos, PriceToY(price));
    }

    private void UpdateCurrentCandle()
    {
        float height = PriceToY(price) - PriceToY(candleOpen);

        if (height >= 0)
        {
            currentCandle.localRotation = Quaternion.identity;
            currentCandle.sizeDelta = new Vector2(currentCandle.sizeDelta.x, height);   
        }
        else
        {
            currentCandle.localRotation = Quaternion.Euler(0f, 0f, 180f);
            currentCandle.sizeDelta = new Vector2(currentCandle.sizeDelta.x, -height);
        }
        currentCandle.anchoredPosition = new Vector2(xPos, PriceToY(candleOpen));
        Image candleImage = currentCandle.GetComponent<Image>();
        candleImage.color = price >= candleOpen ? GameConstants.greenColor : GameConstants.redColor;
    }

    private void FinalizeCandle()
    {
        CandleData data = currentCandle.gameObject.GetComponent<CandleData>();
        data.close = price;
        Image candleImage = currentCandle.GetComponent<Image>();
        candleImage.color = price >= candleOpen ? GameConstants.greenColor : GameConstants.redColor;
        candles.Add(currentCandle);

        xPos += xStep;
        SpawnNewCandle();
    }

    private void UpdateOlderCandles()
    {
        foreach (RectTransform candle in candles)
        {
            if (candle ==  currentCandle) continue;
            CandleData candleData = candle.gameObject.GetComponent<CandleData>();
            if (candleData != null)
            {
                UpdateCandleVisual(candleData);
            }
        }
    }

    private void UpdateCandleVisual(CandleData candle)
    {
        float openY = PriceToY(candle.open);
        float closeY = PriceToY(candle.close);
            
        float height = closeY - openY;

        if (height >= 0)
        {
            candle.rectTransform.localRotation = Quaternion.identity;
            candle.rectTransform.sizeDelta = new Vector2(candle.rectTransform.sizeDelta.x, height);   
        }
        else
        {
            candle.rectTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            candle.rectTransform.sizeDelta = new Vector2(candle.rectTransform.sizeDelta.x, -height);
        }
        candle.rectTransform.anchoredPosition = new Vector2(candle.rectTransform.anchoredPosition.x, openY);
    }

    private void UpdateOlderTradeEntryIndicators()
    {
        foreach (RectTransform tradeIndicatory in tradeEntryIndicators)
        {
            CandleData candleData = tradeIndicatory.GetComponent<CandleData>();
            UpdateTradeIndicatorVisual(candleData);
        }
    }

    private void UpdateTradeIndicatorVisual(CandleData tradeEntryIndicator)
    {
        float openY = PriceToY(tradeEntryIndicator.open);
        tradeEntryIndicator.rectTransform.anchoredPosition = new Vector2(tradeEntryIndicator.rectTransform.anchoredPosition.x, openY);
    }

    private float PriceToY(float p)
    {
        float normalizedPrice = (p - chartMinVisible) / (chartMaxVisible - chartMinVisible);
        return normalizedPrice * priceChart.rect.height;
    }

    private void UpdateChartRange()
    {
        float minPriceInWindow = Mathf.Min(recentPrices.ToArray());
        float maxPriceInWindow = Mathf.Max(recentPrices.ToArray());
        
        float range = Mathf.Max(maxPriceInWindow - minPriceInWindow, 100f);
        if (range <= 0) range = 1f; // Fallback

        float padding = range * yChartPadding;
        chartMinVisible = Mathf.Lerp(chartMinVisible, minPriceInWindow - padding, 0.1f);
        chartMaxVisible = Mathf.Lerp(chartMaxVisible, maxPriceInWindow + padding, 0.1f);
        
        UpdateOlderCandles();
        UpdateOlderTradeEntryIndicators();
        //Debug.Log($"Range: {range}");
    }

    private void DrawGridLines()
    {
        foreach (Transform line in gridLinesParent)
        {
            Destroy(line.gameObject);
        }
        float minLine = Mathf.Floor(chartMinVisible / gridStep) * gridStep;
        float maxLine = Mathf.Ceil(chartMaxVisible / gridStep) * gridStep;

        for (float priceLine = minLine; priceLine <= maxLine; priceLine += gridStep)
        {
            if (priceLine <= 0) continue;
            float y = PriceToY(priceLine);
            DrawHorizontalGridLine(y, priceLine);
        }
    }

    private void DrawHorizontalGridLine(float y, float priceLine)
    {
        GameObject line = Instantiate(gridLinePrefab, gridLinesParent);
        RectTransform rect = line.GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(0, y - gridLinesParent.rect.height / 2);
        
        TMP_Text text = line.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = $"{NumberFormatter.FormatDecimalNumber(priceLine)}$";
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
        float cost = price * currentOrderQuantity;
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
            Debug.Log($"Money: {cash} - Cost: {cost} (Price: {price} * Quantity: {currentOrderQuantity}) = Current Money: {cash - price}");
            cash -= cost;
            cash = Mathf.Clamp(cash, 0f, cash);
            SpawnLostMoneyDamageNumbers(cost);
        }
        GameObject tradeEntry = Instantiate(tradeEntryPrefab, tradePanel);
        TradeEntryStatsDisplay stats = tradeEntry.GetComponent<TradeEntryStatsDisplay>();
        TradeData data = new TradeData(tradeType, System.DateTime.Now.ToString("HH:mm:ss"), currentOrderQuantity, price, leverage);
        
        GameObject tradeEntryIndicator = Instantiate(tradeEntryIndicatorPrefab, tradeIndicatorArea);
        RectTransform rectTransform = tradeEntryIndicator.GetComponent<RectTransform>();
        CandleData tradeEntryIndicatorData = tradeEntryIndicator.GetComponent<CandleData>();
        tradeEntryIndicatorData.open = price;

        rectTransform.anchoredPosition = new Vector2(xPos, PriceToY(price));
        
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
        tradeEntryIndicators.Add(rectTransform);
        
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
            if (comboAmount > 1) // If combo is at least 2
            {
                float comboBonusMoney = comboBonus * 0.01f * trade.GetUnrealizedProfit();
                if (comboBonusMoney >= 1) // and we would get at least 1$ from bonus
                {
                    Debug.Log($"trade.GetUnrealizedProfit: {trade.GetUnrealizedProfit()},  comboBonus: {comboBonus}, comboBonusMoney: {comboBonusMoney}");
                    SpawnReceivedMoneyDamageNumbers(comboBonusMoney, anchor: new Vector2(-300f, 25f));   
                }
                else
                {
                    Debug.Log($"Combo Amount: {comboAmount},  comboBonus: {comboBonus}, comboBonusMoney: {comboBonusMoney}");
                }
            }
        }
        else if (trade.GetUnrealizedProfit() < 0) GameEvents.onMoneyLost?.Invoke();
        activeTrades.Remove(trade);
        if (trade.tradeEntryIndicator != null)
        {
            tradeEntryIndicators.Remove(trade.tradeEntryIndicator.GetComponent<RectTransform>());
        }
    }

    public void DuplicateTrade(TradeEntryStatsDisplay trade)
    {
        GameObject tradeEntry = Instantiate(tradeEntryPrefab, tradePanel);
        TradeEntryStatsDisplay stats = tradeEntry.GetComponent<TradeEntryStatsDisplay>();
        TradeData data = new TradeData(trade.tradeType, System.DateTime.Now.ToString("HH:mm:ss"), trade.quantity, trade.entryPrice, trade.multiplier);
        
        GameObject tradeEntryIndicator = Instantiate(tradeEntryIndicatorPrefab, priceChart);
        RectTransform rectTransform = tradeEntryIndicator.GetComponent<RectTransform>();
        CandleData tradeEntryIndicatorData = tradeEntryIndicator.GetComponent<CandleData>();
        tradeEntryIndicatorData.open = trade.entryPrice;

        rectTransform.anchoredPosition = new Vector2(xPos, PriceToY(trade.entryPrice));
        
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
        tradeEntryIndicators.Add(rectTransform);
        
        stats.Setup(data);
        activeTrades.Add(stats);
        Debug.Log($"Duplicated trade from {trade.timeOfPurchase}");
    }

    public void MarginCall()
    {
        TradeEntryStatsDisplay worstTrade = activeTrades[0];
        foreach (var trade in activeTrades)
        {
            if (trade.GetUnrealizedProfit() < worstTrade.GetUnrealizedProfit())
            {
                worstTrade = trade;
            }
        }
        if (worstTrade.GetUnrealizedProfit() > 0) return;
        Debug.Log($"Current Cash: {cash}, current effectiveCash: {effectiveCash}, equity: {equity}");
        Debug.Log($"Gonna close trade in position {activeTrades.IndexOf(worstTrade)}");
        Debug.Log($"Investment returned from closing position: {worstTrade.entryPrice * worstTrade.quantity} ({worstTrade.entryPrice} * {worstTrade.quantity})\n" +
                  $"Loss from trade: {worstTrade.GetUnrealizedProfit()}");
        worstTrade.Close();
        RecalculateBalances();
        Debug.Log($"Cash: {cash}, Equity: {equity}");
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

    public void CashOut()
    {
        if (PauseManager.Instance.ShouldInputBeBlocked()) return;
        if (effectiveCash < currentCashOutPrice)
        {
            GameEvents.onNotEnoughMoney?.Invoke();
            return;
        }

        if (PowerUpInventoryManager.Instance.AreAllSlotsFull())
        {
            SpawnTextDamageNumbers("Offering inventory full!", position: cashOutButton.gameObject.GetComponent<RectTransform>(), color: Color.white);
            return;
        }
        float currentTierCashOutPrice =
            UpgradesManager.Instance.PriceOfCashOutTier(currentCashOutTier);
        cash -= currentTierCashOutPrice;
        cash = Mathf.Max(0, cash);
        currentRespinPrice = UpgradesSelectionUI.Instance.baseAugmentRespinPrices[currentCashOutTier];
        GameEvents.OnCashOut?.Invoke(currentCashOutTier);
    }

    private void ToggleCashOutButtonEnabled()
    {
        //cashOutButton.interactable = cash > currentCashOutPrice;
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
        currentCashOutTier++;
        currentCashOutTier = (AugmentTier) Mathf.Min((int) currentCashOutTier, Enum.GetValues(typeof(AugmentTier)).Length - 1);
        currentCashOutPrice = UpgradesManager.Instance.PriceOfCashOutTier(currentCashOutTier);
    }
    
    public void DecreaseCashOutTier()
    {
        currentCashOutTier--;
        currentCashOutTier = (AugmentTier) Mathf.Max((int) currentCashOutTier, 0);
        currentCashOutPrice = UpgradesManager.Instance.PriceOfCashOutTier(currentCashOutTier);
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
        currentOrderQuantity = Mathf.Clamp(currentOrderQuantity, 0.1f, maxOrderQuantity);
    }

    public void PlayPriceMoveEvent(PriceMoveEvent priceMoveEvent)
    {
        if (priceMoveEvent.targetPricePercentIncrease != 0)
        {
            priceMoveEvent.targetPrice = Mathf.Max(minPrice, (1 + priceMoveEvent.targetPricePercentIncrease) * price);
        }
        Debug.Log($"{priceMoveEvent.name} event targetPrice: {priceMoveEvent.targetPrice}");
        activeEvent.data = priceMoveEvent;

        activeEvent.startPrice = price;
        activeEvent.elapsed = 0;

        activeEvent.active = true;
    }

    private void HandleCombo()
    {
        if (comboTimer > PlayerStats.Instance.maxComboDuration)
        {
            comboAmount = 1;
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
    
    public void SpawnTextDamageNumbers(string text, RectTransform position = null, Vector2 anchor = new Vector2(), RectTransform canvasParent = null, DamageNumber damageNumberPrefab = null, Color? color = null, bool spawnAtLatestCandle = false, bool scatterTextOnSpawn = false, bool spawnComboText = false, float number = 0)
    {
        if (position == null) position = cashText.rectTransform;
        if (spawnAtLatestCandle) position = currentCandle;
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
        else
        {
            newDamageNumber = damageNumberPrefab.SpawnGUI(canvasParent, position, anchor, text);
        }
        if (color.HasValue)
        {
            newDamageNumber.SetColor(color.Value);
        }
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
        switch (currentCashOutTier)
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
                return GameConstants.legendaryTierColor;
                break;
            default:
                return GameConstants.whiteColor;
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
