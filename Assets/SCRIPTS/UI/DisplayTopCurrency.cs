using UnityEngine;
using UnityEngine.UI;

public class DisplayTopCurrencyIcon : MonoBehaviour
{
    private Image icon;
    private CurrencyStats currencyStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        icon = GetComponent<Image>();
        currencyStats = Resources.Load<CurrencyStats>("Currency/CurrencyStats");
    }

    // Update is called once per frame
    void Update()
    {
        icon.sprite = currencyStats.GetIconOfCurrency(LevelManager.Instance.currentFund.highestUnlockedCurrency);
    }
}