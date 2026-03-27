using TMPro;
using UnityEngine;
using System.Globalization;

public class TradeEntryStatsDisplay : MonoBehaviour
{
    private TradeType tradeType;
    private float timeOfPurchase;
    private float quantity;
    public float entryPrice;

    private float profitReal;

    public Color greenColor;
    public Color redColor;
    
    [SerializeField] private TMP_Text tradeTypeText;
    [SerializeField] private TMP_Text timeOfPurchaseText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text entryPriceText;
    [SerializeField] private TMP_Text currentPriceText;
    [SerializeField] private TMP_Text profitPercentText;
    [SerializeField] private TMP_Text profitRealText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance == null) return;
        currentPriceText.text = LevelManager.Instance.price.ToString("0.00", CultureInfo.InvariantCulture) + "$";
        
        
        if (tradeType == TradeType.Buy)
        {
            profitReal = (LevelManager.Instance.price - entryPrice) * quantity;   
        }
        else if (tradeType == TradeType.Sell)
        {
            profitReal = (entryPrice - LevelManager.Instance.price) * quantity;
        }
        profitRealText.text = profitReal.ToString("0.00", CultureInfo.InvariantCulture) + "$";
        
        
        float profitPrcnt= (profitReal / (entryPrice * quantity)) * 100f;
        profitPercentText.text = profitPrcnt.ToString("0.00",  CultureInfo.InvariantCulture) + "%";

        
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
    }

    public void Setup(TradeData data)
    {
        tradeType = data.tradeType;
        quantity = data.quantity;
        entryPrice = data.entryPrice;
        
        tradeTypeText.text = data.tradeType.ToString();
        timeOfPurchaseText.text = data.timeOfPurchase.ToString();
        quantityText.text = data.quantity.ToString();
        entryPriceText.text = $"{data.entryPrice}$";
    }

    public void Close()
    {
        float realizedProfit = entryPrice + profitReal;
        LevelManager.Instance.cash += realizedProfit;
        LevelManager.Instance.CloseTrade(this);
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
}
