using System;
using System.Collections.Generic;
using System.Globalization;
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
    public float equity;
    
    public float price;
    public float amountToWin = 2000f;
    public float leverage = 1;
    private float previousPrice;
    private float decimals = 0.01f;

    private float generatePriceTimer;
    private float genetartePriceInterval = 0.1f;

    public float minPrice = 10;
    public float maxPrice = 20;

    private List<RectTransform> candles = new List<RectTransform>();
    private List<RectTransform> tradeEntryIndicators = new List<RectTransform>();
    private List<TradeEntryStatsDisplay> activeTrades = new List<TradeEntryStatsDisplay>();
    
    [Header("Candle Spawn Settings")]
    private float xPos = 10;
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
    
    [Header("Canvas stuff")]
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text equityText;
    [SerializeField] private TMP_Text openProfitLossText;
    [SerializeField] private TMP_Text amountToWinText;
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
        price = (minPrice + maxPrice) / 2;
        price = Mathf.Round(price / decimals) * decimals;
        previousPrice = price;
        StartNewCandle();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLevelEnded) return;
        /*candleSpawnTimer += Time.deltaTime;
        if (candleSpawnTimer >= candleSpawnInterval)
        {
            GenerateNewPrice();
            SpawnNewCandle();
            candleSpawnTimer = 0;
        }*/

        candleTimer += Time.deltaTime;
        generatePriceTimer += Time.deltaTime;
        if (generatePriceTimer >= genetartePriceInterval)
        {
            generatePriceTimer = 0;
            GenerateNewPrice();    
        }
        candleHigh = Mathf.Max(candleHigh, price);
        candleLow = Mathf.Min(candleLow, price);

        UpdateCurrentCandle();

        if (candleTimer >= candleSpawnInterval)
        {
            candleTimer = 0;
            FinalizeCandle();
        }

        float openProfit = 0f;
        float invested = 0f;
        foreach (var trade in activeTrades)
        {
            openProfit += trade.GetUnrealizedProfit();
            invested += trade.entryPrice;
            //Debug.Log($"Open position: {trade.name} with P/L: {trade.GetUnrealizedProfit()}");
        }
        
        
        equity = cash + openProfit + invested;
        
        
        cashText.text = $"Cash: " + cash.ToString("0.00", CultureInfo.InvariantCulture) + "$";
        equityText.text = $"Equity: {equity.ToString("0.00", CultureInfo.InvariantCulture)}$";
        openProfitLossText.text = $"Open P/L: {openProfit.ToString("0.00", CultureInfo.InvariantCulture)}$";
        amountToWinText.text = $"Target: {amountToWin}$";
        leverageText.text = $"{leverage}X";
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
            if (equity <= 0f)
            {
                LoseGame();
                hasLevelEnded = true;
            }

            if (cash >= amountToWin)
            {
                WinGame();
                hasLevelEnded = true;
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
        float top = PriceToY(candleHigh);
        float bottom = PriceToY(candleLow);

        float height = top - bottom;

        currentCandle.sizeDelta = new Vector2(currentCandle.sizeDelta.x, height);
        currentCandle.anchoredPosition = new Vector2(xPos, bottom);
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
        SpendMoney(TradeType.Buy);

    }
    
    public void Sell()
    {
        SpendMoney(TradeType.Sell);
    }
    
    public void SpendMoney(TradeType tradeType)
    {
        if (cash < price)
        {
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
    }
    
    public void GainMoney(float amount)
    {
        cash += amount;
    }

    public void CloseTrade(TradeEntryStatsDisplay trade)
    {
        activeTrades.Remove(trade);
        if (trade.tradeEntryIndicator != null)
        {
            tradeEntryIndicators.Remove(trade.tradeEntryIndicator.GetComponent<RectTransform>());
        }
    }
    
    public void WinGame()
    {
        Debug.Log($"Win Game");
        victoryCanvas.SetActive(true);
    }
    public void LoseGame()
    {
        Debug.Log($"Lost Game");
        loseCanvas.SetActive(true);
    }

    public void DecreaseNewPriceInterval()
    {
        /*candleSpawnInterval -= 0.5f;
        candleSpawnInterval = Mathf.Clamp(candleSpawnInterval, 0.25f, 5f);
        generatePriceTimer -= 0.05f;
        generatePriceTimer = Mathf.Clamp(generatePriceTimer, 0.001f, 0.5f);*/
        Time.timeScale += 0.5f;
    }
    
    public void IncreaseNewPriceInterval()
    {
        /*candleSpawnInterval += 0.5f;
        candleSpawnInterval = Mathf.Clamp(candleSpawnInterval, 0.25f, 5f);
        generatePriceTimer += 0.05f;
        generatePriceTimer = Mathf.Clamp(generatePriceTimer, 0.001f, 0.5f);*/
        Time.timeScale -= 0.5f;
    }

    public void IncreaseLeverage()
    {
        leverage += 0.5f;
        leverage = Mathf.Clamp(leverage, 0.5f, 5f);
    }
    public void DecreaseLeverage()
    {
        leverage -= 0.5f;
        leverage = Mathf.Clamp(leverage, 0.5f, 5f);
    }
    
}
