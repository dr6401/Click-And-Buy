using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
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
    
    public float price;
    public float upgradeOfferTarget = 300;
    private float upgradeThresholdIncrease = 500;
    public float leverage = 1;
    private float previousPrice;
    private float decimals = 0.01f;

    private float generatePriceTimer;
    private float genetartePriceInterval = 0.1f;
    private float marginCallTimer;
    private float marginCallInterval = 0.1f;

    public float minPrice = 10;
    public float maxPrice = 20;

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

    public bool hasLevelEnded = false;
    public bool isInputBlocked = false;
    
    [Header("Canvas stuff")]
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text equityText;
    [SerializeField] private TMP_Text openProfitLossText;
    [SerializeField] private TMP_Text upgradeOfferTargetText;
    [SerializeField] private TMP_Text leverageText;


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

        priceText.text = $"Price: {NumberFormatter.FormatDecimalNumber(price)}";
        cashText.text = $"Cash: " + NumberFormatter.FormatDecimalNumber(effectiveCash) + "$";
        equityText.text = $"Equity: {NumberFormatter.FormatDecimalNumber(equity)}$";
        openProfitLossText.text = $"Open P/L: {NumberFormatter.FormatDecimalNumber(openProfit)}$";
        upgradeOfferTargetText.text = $"Cash Target: {NumberFormatter.FormatDecimalNumber(upgradeOfferTarget)}$";
        leverageText.text = $"Current: {NumberFormatter.FormatDecimalNumber(leverage)}X\n" +
                            $"Max: {NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.maxLeverage)}x";
        if (openProfit > 0)
        {
            openProfitLossText.color = GreenColor;
        }
        else if (openProfit < 0)
        {
            openProfitLossText.color = RedColor;
        }
        else openProfitLossText.color = Color.white;
        
        
        
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

            if (cash >= upgradeOfferTarget)
            {
                PlayerStats.Instance.level++;
                GameEvents.OnLevelUp?.Invoke();
                upgradeOfferTarget += upgradeThresholdIncrease;
                if (PlayerStats.Instance.level >= 5) upgradeThresholdIncrease = 1000;
                else if (PlayerStats.Instance.level >= 10) upgradeThresholdIncrease = 2500;
                else if (PlayerStats.Instance.level >= 15) upgradeThresholdIncrease = 5000;
                else if (PlayerStats.Instance.level >= 20) upgradeThresholdIncrease = 10000;
                else if (PlayerStats.Instance.level % 10 == 0)
                {
                    upgradeThresholdIncrease *= 2;
                }
                //WinGame();
                //hasLevelEnded = true;
                //isInputBlocked = true;
            }
        }
    }

    private void GenerateNewPrice()
    {
        previousPrice = price;
        //Debug.Log($"Old Price: {previousPrice}");
        price += Random.Range(-5f, 5f);
        price = Mathf.Clamp(price, minPrice, maxPrice);
        price = Mathf.Round(price / decimals) * decimals;
        //Debug.Log($"New price: {price}");
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

        if (PriceToY(price) >= PriceToY(candleOpen))
        {
            currentCandle.localRotation = Quaternion.Euler(0, 0, 0);
            currentCandle.sizeDelta = new Vector2(currentCandle.sizeDelta.x, height);   
        }
        else
        {
            Quaternion inverseRotation = Quaternion.Euler(0f, 0f, 180f);
            currentCandle.localRotation = inverseRotation;
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
        float normalizedPrice = (p - minPrice) / (maxPrice - minPrice);
        return normalizedPrice * priceChart.rect.height;
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
        if (cash < price)
        {
            GameEvents.onNotEnoughMoney?.Invoke();
            Debug.Log("Not enough money");
            return;
        }
        Debug.Log($"Money: {cash} - Price: {price} = Current Money: {cash - price}");
        cash -= price;
        cash = Mathf.Clamp(cash, 0f, cash);
        GameObject tradeEntry = Instantiate(tradeEntryPrefab, tradePanel);
        TradeEntryStatsDisplay stats = tradeEntry.GetComponent<TradeEntryStatsDisplay>();
        TradeData data = new TradeData(tradeType, System.DateTime.Now.ToString("HH:mm:ss"), 1, price, leverage);
        
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

    public void DecreaseNewPriceInterval()
    {
        if (isInputBlocked) return;
        Time.timeScale += 0.5f;
    }
    
    public void IncreaseNewPriceInterval()
    {
        if (isInputBlocked || Time.timeScale <= 0.5f) return;
        Time.timeScale -= 0.5f;
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

    public void ToggleInputBlocked()
    {
        isInputBlocked = !isInputBlocked;
    }

    private void OnEnable()
    {
        GameEvents.OnLevelUp += ToggleInputBlocked;
        GameEvents.OnUpgradeChosen += ToggleInputBlocked;
    }
    
    private void OnDisable()
    {
        GameEvents.OnLevelUp -= ToggleInputBlocked;
        GameEvents.OnUpgradeChosen -= ToggleInputBlocked;
    }
}
