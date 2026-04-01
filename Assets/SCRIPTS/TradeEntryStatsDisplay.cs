using TMPro;
using UnityEngine;
using System.Globalization;
using DamageNumbersPro;

public class TradeEntryStatsDisplay : MonoBehaviour
{
    public TradeType tradeType;
    private float timeOfPurchase;
    public float quantity;
    public float entryPrice;
    public float leverage = 1f;

    private float profitReal;

    public GameObject tradeEntryIndicator;

    public Color greenColor;
    public Color redColor;

    [SerializeField] private DamageNumber profitDamageNumbersPrefab;
    [SerializeField] private DamageNumber lossDamageNumbersPrefab;
    
    [SerializeField] private TMP_Text tradeTypeText;
    [SerializeField] private TMP_Text timeOfPurchaseText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text entryPriceText;
    [SerializeField] private TMP_Text currentPriceText;
    [SerializeField] private TMP_Text profitPercentText;
    [SerializeField] private TMP_Text profitRealText;
    [SerializeField] private TMP_Text closeTradeText;

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance == null) return;
        currentPriceText.text = NumberFormatter.FormatDecimalNumber(LevelManager.Instance.price) + "$";
        
        
        if (tradeType == TradeType.Buy)
        {
            profitReal = (LevelManager.Instance.price - entryPrice) * quantity;
        }
        else if (tradeType == TradeType.Sell)
        {
            profitReal = (entryPrice - LevelManager.Instance.price) * quantity;
        }
        profitReal *= leverage;
        if (profitReal > 0) profitReal *= PlayerStats.Instance.moneyGainMultiplier;
        profitRealText.text = NumberFormatter.FormatDecimalNumber(profitReal) + "$";
        
        
        float profitPrcnt= (profitReal / (entryPrice * quantity)) * 100f;
        profitPercentText.text = NumberFormatter.FormatDecimalNumber(profitPrcnt) + "%";

        
        if (IsInProfit())
        {
            profitPercentText.color = greenColor;
            profitRealText.color = greenColor;
        }
        else
        {
            profitPercentText.color = redColor;
            profitRealText.color = redColor;
        }
        tradeTypeText.text = tradeType.ToString();
        timeOfPurchaseText.text = timeOfPurchase.ToString();
        quantityText.text = quantity.ToString();
        entryPriceText.text = $"{NumberFormatter.FormatDecimalNumber(entryPrice)}$";
    }

    public void Setup(TradeData data)
    {
        tradeType = data.tradeType;
        quantity = data.quantity;
        entryPrice = data.entryPrice;
        leverage = data.leverage;
        
        tradeTypeText.text = data.tradeType.ToString();
        timeOfPurchaseText.text = data.timeOfPurchase.ToString();
        quantityText.text = data.quantity.ToString();
        entryPriceText.text = $"{data.entryPrice}$";
    }

    public void Close()
    {
        if (LevelManager.Instance.isInputBlocked) return;
        float realizedProfit = entryPrice * quantity + profitReal;
        LevelManager.Instance.cash += realizedProfit;
        LevelManager.Instance.CloseTrade(this);
        SpawnDamageNumbers();
        Destroy(tradeEntryIndicator);
        Destroy(gameObject);
    }

    public float GetUnrealizedProfit()
    {
        return profitReal;
    }

    private bool IsInProfit()
    {
        if (tradeType == TradeType.Buy)
        {
            return LevelManager.Instance.price >= entryPrice;
        }
        else if (tradeType == TradeType.Sell)
        {
            return LevelManager.Instance.price < entryPrice;
        }
        return true;
    }
    
    public void LinkTradeEntryIndicator(GameObject indicator)
    {
        tradeEntryIndicator = indicator;
    }

    public float GetLossBeyondMargin()
    {
        float margin = entryPrice * quantity;

        return Mathf.Min(0, margin + profitReal);
    }

    private void SpawnDamageNumbers()
    {
        RectTransform gameCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
        if (GetUnrealizedProfit() > 0)
        {
            DamageNumber newDamageNumber = profitDamageNumbersPrefab.SpawnGUI(gameCanvas, closeTradeText.rectTransform, Vector2.zero, GetUnrealizedProfit());
            //newDamageNumber.SetToMousePosition(gameCanvas, Camera.main);
            Debug.Log($"Spawned Profit PopUp at {closeTradeText.rectTransform}");
        }
        else if (GetUnrealizedProfit() < 0)
        {
            DamageNumber newDamageNumber = lossDamageNumbersPrefab.SpawnGUI(gameCanvas, closeTradeText.rectTransform, Vector2.zero, GetUnrealizedProfit());
            //newDamageNumber.SetToMousePosition(gameCanvas, Camera.main);
            Debug.Log($"Spawned Loss PopUp at {closeTradeText.rectTransform}");
        }
    }
}
