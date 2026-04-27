using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrencyItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PlayerCurrencies.Currency currency;
    [SerializeField] private float unlockCost;
    [SerializeField] private CurrencyStats currencyStats;
    
    [SerializeField] private bool isUnlocked;
    
    [SerializeField] private TMP_Text unlockCostText;
    [SerializeField] private Image icon;
    
    [SerializeField] public CurrencyItemTooltip tooltip;


    private void Awake()
    {
        currencyStats = Resources.Load<CurrencyStats>("Currency/CurrencyStats");
    }

    void Start()
    {
        icon.sprite = currencyStats.GetIconOfCurrency(currency);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        //Debug.Log($"Tooltip Time!");
        //if (PauseManager.Instance.ShouldInputBeBlocked()) return;
        tooltip.gameObject.SetActive(true);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO Switch currency chart
        SoundManager.Instance?.PlayClickedButtonSFX();
    }

    public void OnPointerExit(PointerEventData data)
    {
        tooltip.gameObject.SetActive(false);
    }
}
