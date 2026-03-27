using TMPro;
using UnityEngine;

public class TradeEntryStatsDisplay : MonoBehaviour
{

    [SerializeField] private TMP_Text currentPrice;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CandleSpawner.Instance == null) return;
        currentPrice.text = $"{CandleSpawner.Instance.price}$";
    }
}
