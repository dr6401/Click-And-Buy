using UnityEngine;
using UnityEngine.UI;

public class CandleSpawner : MonoBehaviour
{

    [SerializeField] private RectTransform priceChart;
    [SerializeField] private GameObject candlePrefab;

    public float price;
    private float previousPrice;

    public float minPrice = 10;
    public float maxPrice = 20;
    
    [Header("Candle Spawn Settings")]
    private float xPos = 10;
    private float xStep = 50; // Distance between candles
    
    private float candleSpawnTimer;
    public float candleSpawnInterval;

    public Color GreenColor;
    public Color RedColor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        price = (minPrice + maxPrice) / 2;
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
    }

    private void GenerateNewPrice()
    {
        previousPrice = price;
        Debug.Log($"Old Price: {previousPrice}");
        price += Random.Range(-0.5f, 0.5f);
        price = Mathf.Clamp(price, minPrice, maxPrice);
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
}
