using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DamageNumbersPro;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [SerializeField] private RectTransform priceChart;
    [SerializeField] private GameObject candlePrefab;
    [SerializeField] private GameObject tradeEntryIndicatorPrefab;
    
    [SerializeField] private RectTransform tradePanel;
    [SerializeField] private GameObject tradeEntryPrefab;
    
    public float cash = 1000f;
    public float effectiveCash;
    public float equity;
    [Header("-----------------PRICE----------------")]
    public float price;
    [Header("-----------------PRICE----------------")]
    public float upgradeOfferTarget = 300;
    private float upgradeThresholdIncrease = 500;
    public float leverage = 1;
    private float previousPrice;
    private float decimals = 0.01f;
    private float currentOrderQuantity = 1f;

    private float generatePriceTimer;
    private float genetartePriceInterval = 0.1f;
    private float marginCallTimer;
    private float marginCallInterval = 0.1f;

    [Header("Upgrade System")]
    public AugmentTier currentCashOutTier = AugmentTier.Common;
    public float currentCashOutPrice = 300;

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

    public Color GreenColor;
    public Color RedColor;

    private RectTransform currentCandle;
    private float candleOpen;
    private float candleHigh;
    private float candleLow;
    private float candleTimer;
    
    [Header("Chart Settings")]
    [SerializeField] private GameObject gridLinePrefab;
    [SerializeField] private RectTransform gridLinesParent;
    
    [SerializeField] private int lastNPrices = 50;
    [SerializeField] private float yChartPadding = 0.1f;

    private float chartMinVisible;
    private float chartMaxVisible;
    private float gridStep = 10f;
    
    private List<float> recentPrices = new List<float>();
    
    [Header("DamageNumbersPro")]
    [SerializeField] private DamageNumber profitDamageNumbersPrefab;
    [SerializeField] private DamageNumber lossDamageNumbersPrefab;
    [SerializeField] private DamageNumber textDamageNumbersPrefab;

    public bool hasLevelEnded = false;
    public bool isInputBlocked = false;
    
    [Header("Canvas stuff")]
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text equityText;
    [SerializeField] private TMP_Text openProfitLossText;
    [SerializeField] private TMP_Text cashOutText;
    [SerializeField] private Button cashOutButton;
    [SerializeField] private TMP_Text leverageText;
    [SerializeField] private TMP_Text quantityOrderText;
    [SerializeField] private Slider timeScaleSlider;

    [Header("Power Ups")]
    public int numberOfFutureFreebieTrades;


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
        recentPrices.Add(price);
        GenerateNewPrice();
        StartNewCandle();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLevelEnded) return;

        candleTimer += Time.deltaTime;
        generatePriceTimer += Time.deltaTime;
        marginCallTimer += Time.deltaTime;
        
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

        float openProfit = 0f;
        float invested = 0f;
        effectiveCash = cash;
        foreach (var trade in activeTrades)
        {
            openProfit += trade.GetUnrealizedProfit();
            invested += trade.entryPrice * trade.quantity;
            effectiveCash += trade.GetLossBeyondMargin();
            //Debug.Log($"Open position: {trade.name} with P/L: {trade.GetUnrealizedProfit()}");
        }
        
        effectiveCash = Mathf.Max(0, effectiveCash);
        
        equity = cash + openProfit + invested;

        priceText.text = $"Price: {NumberFormatter.FormatDecimalNumber(price)}$";
        cashText.text = $"Cash: " + NumberFormatter.FormatDecimalNumber(effectiveCash) + "$";
        equityText.text = $"Equity: {NumberFormatter.FormatDecimalNumber(equity)}$";
        openProfitLossText.text = $"Open P/L: {NumberFormatter.FormatDecimalNumber(openProfit)}$";
        cashOutText.text = $"{currentCashOutTier}: {NumberFormatter.FormatDecimalNumber(UpgradesManager.Instance.cashOutTierPrices.GetValueOrDefault(currentCashOutTier, 300))}$";
        leverageText.text = $"Current: {NumberFormatter.FormatDecimalNumber(leverage)}X\n" +
                            $"Max: {NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.maxLeverage)}x";
        quantityOrderText.text = $"{NumberFormatter.FormatDecimalNumber(currentOrderQuantity)}";
        if (openProfit > 0)
        {
            openProfitLossText.color = GreenColor;
        }
        else if (openProfit < 0)
        {
            openProfitLossText.color = RedColor;
        }
        else openProfitLossText.color = Color.white;

        ToggleCashOutButtonEnabled();
        DrawGridLines();
        
        
        
        if (!hasLevelEnded)
        {
            if (effectiveCash <= 0f && activeTrades.Count > 0 && marginCallTimer >= marginCallInterval)
            {
                marginCallTimer = 0;
                MarginCall(); // Deletes trade with biggest loss
                if (effectiveCash < 0 && activeTrades.Count <= 0)
                {
                    hasLevelEnded = true;
                    isInputBlocked = true;
                    LoseGame();
                }
            }
        }
    }

    private void GenerateNewPrice()
    {
        previousPrice = price;
        //Debug.Log($"Old Price: {previousPrice}");
        price += Random.Range(-5f, 5f);
        price = Mathf.Round(price / decimals) * decimals;
        //Debug.Log($"New price: {price}");
        
        recentPrices.Add(price);
        if (recentPrices.Count > lastNPrices)
        {
            recentPrices.RemoveAt(0);
        }

        UpdateChartRange();
    }

    public void StartNewCandle()
    {
        float width = priceChart.rect.width;
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
        
        GameObject candle = Instantiate(candlePrefab, priceChart);
        currentCandle = candle.GetComponent<RectTransform>();

        candleOpen = price;
        candleHigh = price;
        candleLow = price;

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
        candleImage.color = price >= candleOpen ? GreenColor : RedColor;
    }

    private void FinalizeCandle()
    {
        Image candleImage = currentCandle.GetComponent<Image>();
        candleImage.color = price >= candleOpen ? GreenColor : RedColor;
        candles.Add(currentCandle);

        xPos += xStep;
        StartNewCandle();
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
        
        float range = maxPriceInWindow - minPriceInWindow;
        if (range <= 0) range = 1f; // Fallback

        float padding = range * yChartPadding;
        chartMinVisible = Mathf.Lerp(chartMinVisible, minPriceInWindow - padding, 0.1f);
        chartMaxVisible = Mathf.Lerp(chartMaxVisible, maxPriceInWindow + padding, 0.1f);
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
            float y = PriceToY(priceLine);
            DrawHorizontalGridLine(y, priceLine);
        }
    }

    private void DrawHorizontalGridLine(float y, float priceLine)
    {
        GameObject line = Instantiate(gridLinePrefab, gridLinesParent);
        RectTransform rect = line.GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(0, y);
        rect.sizeDelta = new Vector2(priceChart.rect.width, rect.sizeDelta.y);
        
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
            Debug.Log($"Max amount of alive trades reached: {PlayerStats.Instance.maxAliveTrades}! ");
            return;
        }

        if (IsNextTradeFree())
        {
            numberOfFutureFreebieTrades--;
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
        
        GameObject tradeEntryIndicator = Instantiate(tradeEntryIndicatorPrefab, priceChart);
        RectTransform rectTransform = tradeEntryIndicator.GetComponent<RectTransform>();

        //Debug.Log($"Normalized price: {normalizedPrice}");

        rectTransform.anchoredPosition = new Vector2(xPos, PriceToY(price));
        
        Image indicatorImage = tradeEntryIndicator.GetComponent<Image>();
        if (tradeType == TradeType.Buy)
        {
            indicatorImage.color = GreenColor;
        }
        else if (tradeType == TradeType.Sell)
        {
            indicatorImage.color = RedColor;
        }
        
        stats.LinkTradeEntryIndicator(tradeEntryIndicator);
        tradeEntryIndicators.Add(rectTransform);
        
        stats.Setup(data);
        activeTrades.Add(stats);
        GameEvents.onMoneySpent?.Invoke();
    }
    
    public void GainMoney(float amount)
    {
        cash += amount;
    }

    public void CloseTrade(TradeEntryStatsDisplay trade)
    {
        if (isInputBlocked) return;
        if (trade.GetUnrealizedProfit() > 0) GameEvents.onMoneyEarned?.Invoke();
        else if (trade.GetUnrealizedProfit() < 0) GameEvents.onMoneyLost?.Invoke();
        activeTrades.Remove(trade);
        if (trade.tradeEntryIndicator != null)
        {
            tradeEntryIndicators.Remove(trade.tradeEntryIndicator.GetComponent<RectTransform>());
        }
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
        Debug.Log($"Gonna close trade in position {activeTrades.IndexOf(worstTrade)}");
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
        if (cash < currentCashOutPrice)
        {
            GameEvents.onNotEnoughMoney?.Invoke();
            return;
        }
        float currentTierCashOutPrice =
            UpgradesManager.Instance.cashOutTierPrices.GetValueOrDefault(currentCashOutTier, 300);
        cash -= currentTierCashOutPrice;
        cash = Mathf.Max(0, cash);
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

    /*public void DecreaseNewPriceInterval()
    {
        if (isInputBlocked) return;
        Time.timeScale += 0.5f;
    }
    
    public void IncreaseNewPriceInterval()
    {
        if (isInputBlocked || Time.timeScale <= 0.5f) return;
        Time.timeScale -= 0.5f;
    }*/

    public void ChangeTimeScale()
    {
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
        leverage += 2f;
        leverage = Mathf.Clamp(leverage, 0.5f, PlayerStats.Instance.maxLeverage);
    }
    public void DecreaseLeverage()
    {
        if (isInputBlocked) return;
        leverage -= 2f;
        leverage = Mathf.Clamp(leverage, 0.5f, PlayerStats.Instance.maxLeverage);
    }

    public void IncreaseCashOutTier()
    {
        currentCashOutTier++;
        currentCashOutTier = (AugmentTier) Mathf.Min((int) currentCashOutTier, Enum.GetValues(typeof(AugmentTier)).Length - 1);
    }
    
    public void DecreaseCashOutTier()
    {
        currentCashOutTier--;
        currentCashOutTier = (AugmentTier) Mathf.Max((int) currentCashOutTier, 0);
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
        currentOrderQuantity = Mathf.Max(0.1f, currentOrderQuantity);
    }

    public void ToggleInputBlocked()
    {
        isInputBlocked = !isInputBlocked;
    }

    public void SpawnReceivedMoneyDamageNumbers(float amount)
    {
        RectTransform gameCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
        DamageNumber newDamageNumber = profitDamageNumbersPrefab.SpawnGUI(gameCanvas, cashText.rectTransform, Vector2.zero, amount);
    }
    public void SpawnLostMoneyDamageNumbers(float amount)
    {
        RectTransform gameCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
        DamageNumber newDamageNumber = lossDamageNumbersPrefab.SpawnGUI(gameCanvas, cashText.rectTransform, Vector2.zero, amount);
    }
    
    public void SpawnTextDamageNumbers(string text, RectTransform position = null, Vector2 anchor = new Vector2())
    {
        if (position == null) position = cashText.rectTransform;
        RectTransform gameCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
        DamageNumber newDamageNumber = textDamageNumbersPrefab.SpawnGUI(gameCanvas, position, anchor, text);
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

    private void OnEnable()
    {
        GameEvents.OnUpgradesOffered += ToggleInputBlocked;
        GameEvents.OnUpgradeChosen += ToggleInputBlocked;
    }
    
    private void OnDisable()
    {
        GameEvents.OnUpgradesOffered -= ToggleInputBlocked;
        GameEvents.OnUpgradeChosen -= ToggleInputBlocked;
    }
}
