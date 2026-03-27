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
    
    [SerializeField] private RectTransform tradePanel;
    [SerializeField] private GameObject tradeEntryPrefab;

    public float cash = 100f;
    public float equity;
    
    public float price;
    private float previousPrice;
    private float decimals = 0.01f;

    public float minPrice = 10;
    public float maxPrice = 20;
    
    private List<TradeEntryStatsDisplay> activeTrades = new List<TradeEntryStatsDisplay>();
    
    [Header("Candle Spawn Settings")]
    private float xPos = 10;
    private float xStep = 50; // Distance between candles
    
    private float candleSpawnTimer;
    public float candleSpawnInterval;

    public Color GreenColor;
    public Color RedColor;
    
    [Header("Canvas stuff")]
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text equityText;
    [SerializeField] private TMP_Text openProfitLossText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        equity = cash;
        price = (minPrice + maxPrice) / 2;
        price = Mathf.Round(price / decimals) * decimals;
        previousPrice = price;
        SpawnNewCandle();
    }

    // Update is called once per frame
    void Update()
    {
        candleSpawnTimer += Time.deltaTime;
        if (candleSpawnTimer >= candleSpawnInterval)
        {
            GenerateNewPrice();
            SpawnNewCandle();
            candleSpawnTimer = 0;
        }

        float openProfit = 0f;
        float invested = 0f;
        foreach (var trade in activeTrades)
        {
            openProfit += trade.GetUnrealizedProfit();
            invested += trade.entryPrice;
            Debug.Log($"Open position: {trade.name} with P/L: {trade.GetUnrealizedProfit()}");
        }
        
        
        equity = cash + openProfit + invested;
        
        
        cashText.text = $"Cash: " + cash.ToString("0.00", CultureInfo.InvariantCulture) + "$";
        equityText.text = $"Equity: {equity.ToString("0.00", CultureInfo.InvariantCulture)}$";
        openProfitLossText.text = $"Open P/L: {openProfit.ToString("0.00", CultureInfo.InvariantCulture)}$";
        if (openProfit > 0)
        {
            openProfitLossText.color = GreenColor;
        }
        else if (openProfit < 0)
        {
            openProfitLossText.color = RedColor;
        }
        else openProfitLossText.color = Color.white;
        
        
        
        if (equity <= 0f)
        {
            LoseGame();
        }
    }

    private void GenerateNewPrice()
    {
        previousPrice = price;
        Debug.Log($"Old Price: {previousPrice}");
        price += Random.Range(-0.5f, 0.5f);
        price = Mathf.Clamp(price, minPrice, maxPrice);
        price = Mathf.Round(price / decimals) * decimals;
        Debug.Log($"New price: {price}");
    }

    private void SpawnNewCandle()
    {
        GameObject candle = Instantiate(candlePrefab, priceChart);
        RectTransform rectTransform = candle.GetComponent<RectTransform>();
        
        float normalizedPrice = (price - minPrice) / (maxPrice - minPrice);
        float y = normalizedPrice * priceChart.rect.height;
        //Debug.Log($"Normalized price: {normalizedPrice}");

        rectTransform.anchoredPosition = new Vector2(xPos, y);
        xPos += xStep;
        //Debug.Log($"Spawned new candle at {rectTransform.anchoredPosition}");

        Image candleImage = candle.GetComponent<Image>();
        if (price < previousPrice)
        {
            candleImage.color = RedColor;
        }
        else
        {
            candleImage.color = GreenColor;
        }
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
        TradeData data = new TradeData(tradeType, System.DateTime.Now.ToString("HH:mm:ss"), 1, price);
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
    }

    public void LoseGame()
    {
        // Lose game or sum
    }
}
