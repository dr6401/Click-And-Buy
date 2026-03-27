using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CandleSpawner : MonoBehaviour
{
    public static CandleSpawner Instance;
    
    [SerializeField] private RectTransform priceChart;
    [SerializeField] private GameObject candlePrefab;
    
    [SerializeField] private RectTransform tradePanel;
    [SerializeField] private GameObject tradeEntryPrefab;

    public float currentMoney = 100f;
    
    public float price;
    private float previousPrice;
    private float decimals = 0.01f;

    public float minPrice = 10;
    public float maxPrice = 20;
    
    [Header("Candle Spawn Settings")]
    private float xPos = 10;
    private float xStep = 50; // Distance between candles
    
    private float candleSpawnTimer;
    public float candleSpawnInterval;

    public Color GreenColor;
    public Color RedColor;
    
    [Header("Canvas stuff")] [SerializeField]
    private TMP_Text currentMoneyText;

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
        currentMoneyText.text = $"Money: {currentMoney}$";
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
    
    public void SpendMoney()
    {
        if (currentMoney < price)
        {
            Debug.Log("Not enough money");
            return;
        }
        Debug.Log($"Money: {currentMoney} - Price: {price} = Current Money: {currentMoney - price}");
        currentMoney -= price;
        currentMoney = Mathf.Clamp(currentMoney, 0f, currentMoney);
        GameObject candle = Instantiate(tradeEntryPrefab, tradePanel);
        if (currentMoney <= 0f)
        {
            LoseGame();
        }
    }
    
    public void GainMoney(float amount)
    {
        currentMoney += amount;
    }

    public void LoseGame()
    {
        // Lose game or sum
    }
}
