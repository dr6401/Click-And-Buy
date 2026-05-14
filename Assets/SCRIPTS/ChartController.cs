using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChartController : MonoBehaviour
{
    [Header("Chart Specifics")]
    public bool hasChartBeenUnlocked = false;
    [SerializeField] private float startingPrice;
    [SerializeField] private float chartVolatilityMultiplier = 1f;
    [Header("--------------------------------------------------------------------------------------------------------------------------")]
    
    public RectTransform priceChart;
    [SerializeField] private RectTransform candleArea;
    public RectTransform tradeIndicatorArea;
    [SerializeField] private GameObject candlePrefab;
    public GameObject tradeEntryIndicatorPrefab;
    
    [Header("-----------------PRICE----------------")]
    public float price;
    [Header("-----------------PRICE----------------")]
    private List<FutureTick> futurePrices;
    private int amountOfFuturePrices = 200;
    private float decimals = 0.01f;
    
    public PlayerCurrencies.Currency chartCurrency;
    
    public float minPrice = 10f;
    public float maxPrice = 200f;
    
    private float maxPriceIncreaseTimer;
    public float maxPriceIncreaseInterval = 10f;
    public float maxPriceIncreaseAmount = 1f;
    
    public float trend;
    public float maxTrendStrength = 0.2f;
    private float generateNewTrendTimer;
    private float trendInterval = 5f;
    
    private float generatePriceTimer;
    private float genetartePriceInterval = 0.1f;
    
    [Header("DEBUG")]
    public bool stopGeneratingPrice;

    private List<RectTransform> candles = new List<RectTransform>();
    public List<RectTransform> tradeEntryIndicators = new List<RectTransform>();
    
    [Header("Candle Spawn Settings")]
    public float xPos = 30;
    public float xStep = 50; // Distance between candles
    
    public float candleSpawnInterval = 5f;

    public RectTransform currentCandle;
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
    public ActivePriceEvent activeEvent = new ActivePriceEvent();
    
    void Start()
    {
        price = startingPrice;
        price = Mathf.Round(price / decimals) * decimals;
        recentPrices.Add(price);
        chartMinVisible = Mathf.Min(maxPrice, price + 50);
        chartMaxVisible = Mathf.Max(minPrice, price - 50);
        
        futurePrices = new List<FutureTick>();
        GenerateFuturePrices(amountOfFuturePrices);
        
        SpawnNewCandle();
    }

    // Update is called once per frame
    public void Tick()
    {
        if (LevelManager.Instance.hasLevelEnded || !hasChartBeenUnlocked) return;

        candleTimer += Time.deltaTime;
        generatePriceTimer += Time.deltaTime;
        generateNewTrendTimer += Time.deltaTime;
        if (Time.timeScale > 0)
        {
            maxPriceIncreaseTimer +=
                Time.unscaledDeltaTime; // If game isn't paused/powerups picking: increase this slowly, independently of time scale
        }
        
        UpdateCurrentCandle();

        if (candleTimer >= candleSpawnInterval)
        {
            candleTimer = 0;
            FinalizeCandle();
        }
        
        if (generatePriceTimer >= genetartePriceInterval) // ----------------PRICE GENERATION----------------
        {
            generatePriceTimer = 0;
            if (!stopGeneratingPrice)
            {
                FutureTick tick = futurePrices[0];
                futurePrices.RemoveAt(0);
                //Debug.Log($"Price: {price} boutta become: {tick.price}");
                price = tick.price;

                FutureTick latestTick = futurePrices.Count > 0 ? futurePrices.Last() : new FutureTick(price, 0f); // Fallback if futurePrices is empty
                FutureTick newGeneratedTick = GenerateNewPriceTick(latestTick);
                futurePrices.Add(newGeneratedTick);
                
                recentPrices.Add(price);
                if (recentPrices.Count > lastNPrices)
                {
                    recentPrices.RemoveAt(0);
                }
                
                UpdateChartRange();
            }
        }
        candleHigh = Mathf.Max(candleHigh, price);
        candleLow = Mathf.Min(candleLow, price);

        if (maxPriceIncreaseTimer >= maxPriceIncreaseInterval)
        {
            maxPriceIncreaseTimer = 0;
            maxPrice += maxPriceIncreaseAmount;
        }

        if (generateNewTrendTimer >= trendInterval)
        {
            trend = Random.Range(-maxTrendStrength, maxTrendStrength);
            generateNewTrendTimer = 0;
            //Debug.Log($"New TREND: {trend}");
        }
        DrawGridLines();
    }

    private FutureTick GenerateNewPriceTick(FutureTick previousTick)
    {
        float newPrice = previousTick.price;
        // Price Move Event
        /*if (activeEvent.active && activeEvent != null)
        {
            PriceMoveEvent priceEvent = activeEvent.data;

            activeEvent.elapsed += genetartePriceInterval;

            float t = Mathf.Clamp01(activeEvent.elapsed / priceEvent.duration);
            float curveT = priceEvent.animationCurve.Evaluate(t);

            float desiredPrice = Mathf.Lerp(activeEvent.startPrice, priceEvent.targetPrice, curveT);

            float move = desiredPrice - previousTick.price;
            move = Mathf.Clamp(move, -priceEvent.maxStep, priceEvent.maxStep);
            
            float noise = Random.Range(-priceEvent.maxNoise,  priceEvent.maxNoise);
            newPrice += move + noise;

            if (t >= 1) // If elapsedTime >= priceEvent.duration
            {
                newPrice = priceEvent.targetPrice;
                activeEvent.active = false;
                Debug.Log($"Event: {priceEvent.name} finished at price: {newPrice} on Chart: {chartCurrency} (priceEvent.targetPrice was: {priceEvent.targetPrice})!");
            }

            if (activeEvent.isTargetHigherThanCurrentPrice && price > activeEvent.data.targetPrice ||
                activeEvent.isTargetHigherThanCurrentPrice && price >= maxPrice ||
                !activeEvent.isTargetHigherThanCurrentPrice && price < activeEvent.data.targetPrice ||
                !activeEvent.isTargetHigherThanCurrentPrice && price <= minPrice)
            {
                activeEvent.active = false;
                Debug.Log($"Event: {priceEvent.name} finished at price: {price} on Chart: {chartCurrency} because the price was over the target ({priceEvent.targetPrice})");
            }
        }*/
        if (false){} // Do nothing
        else
        {
            float move = trend + Random.Range(-5f, 5f) * chartVolatilityMultiplier * PlayerStats.Instance.volatility;
            //Debug.Log($"Move: {move}");
            newPrice += move;
        }
        
        newPrice = Mathf.Round(newPrice / decimals) * decimals;
        newPrice = Mathf.Clamp(newPrice, minPrice, maxPrice);
        //Debug.Log($"New price: {price}");
        
        return new FutureTick(newPrice, previousTick.timestamp + genetartePriceInterval); // * Time.timeScale);
    }

    private void GenerateFuturePrices(int steps)
    {
        futurePrices.Clear();
        FutureTick lastTick = new FutureTick(price, 0f);
        for (int i = 0; i < steps; i++)
        {
            FutureTick newTick = GenerateNewPriceTick(lastTick);
            futurePrices.Add(newTick);

            lastTick = newTick;
        }
    }

    public void ChangePrice(float amount)
    {
        price += amount;
        GenerateFuturePrices(amountOfFuturePrices);
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
        if (currentCandle == null) return;
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

    public float PriceToY(float p)
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

        float range = chartMaxVisible - chartMinVisible;
        float estimatedLines = range / gridStep;

        float stepToUse = gridStep;

        if (estimatedLines > 15)
        {
            stepToUse = GetNiceGridStep(range, 15); 
        }
        
        float minLine = Mathf.Floor(chartMinVisible / stepToUse) * stepToUse;
        float maxLine = Mathf.Ceil(chartMaxVisible / stepToUse) * stepToUse;

        for (float priceLine = minLine; priceLine <= maxLine; priceLine += stepToUse)
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

    private float GetNiceGridStep(float range, int targetLines)
    {
        float roughStep = range / targetLines;
        float magnitude = Mathf.Pow(10, Mathf.Floor(Mathf.Log10(roughStep)));
        float normalized = roughStep / magnitude;

        float niceNormalized;

        if (normalized < 1.5f) niceNormalized = 1f;
        else if (normalized < 3f) niceNormalized = 2f;
        else if (normalized < 7f) niceNormalized = 5f;
        else niceNormalized = 10f;

        return niceNormalized * magnitude;
    }
    
    public void ResetChartValuesAtFundSell()
    {
        currentCandle = null;
        recentPrices.Clear();
        candleTimer = 0;
        generatePriceTimer = 0;
        
        price = startingPrice;
        price = Mathf.Round(price / decimals) * decimals;
        recentPrices.Add(price);
        
        SpawnNewCandle();

        xPos = 30;
        
        chartMinVisible = Mathf.Min(maxPrice, price + 50);
        chartMaxVisible = Mathf.Max(minPrice, price - 50);
    }
    
    public void CloseAndClearAllCandles()
    {
        foreach (Transform candle in candleArea)
        {
            Destroy(candle.gameObject);
        }
        candles.Clear();
    }

    public void PredictPriceAfterSeconds(float seconds)
    {
        int indexOfPrediction = Mathf.RoundToInt(seconds / genetartePriceInterval);
        float futurePrice = futurePrices[indexOfPrediction].price;
        //Debug.Log($"Going to take {indexOfPrediction}th element in future prices ({futurePrices[indexOfPrediction]})");
        Debug.Log($"{chartCurrency} chart price after {seconds}s: {futurePrice}");
    }
}
[System.Serializable]
public class FutureTick
{
    public float price;
    public float timestamp;
    
    public FutureTick(float price, float timestamp)
    {
        this.price = price;
        this.timestamp = timestamp;
    }
}
