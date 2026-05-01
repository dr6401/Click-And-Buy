using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrencyItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerCurrencies.Currency currency;
    [SerializeField] private float unlockCost;
    [SerializeField] public CurrencyStats currencyStats;
    
    [SerializeField] private bool isUnlocked;
    
    [SerializeField] private TMP_Text unlockCostText;
    [SerializeField] private Image icon;
    [SerializeField] private Image lockedOverlay;

    
    [SerializeField] public CurrencyItemTooltip tooltip;
    //[SerializeField] private MMF_Player hoverFeedback;

    private void Awake()
    {
        currencyStats = Resources.Load<CurrencyStats>("Currency/CurrencyStats");
    }

    void Start()
    {
        icon.sprite = currencyStats.GetIconOfCurrency(currency);
        if (PlayerCurrencies.Instance != null)
        {
            isUnlocked = PlayerCurrencies.Instance.IsCurrencyUnlocked(currency);   
            unlockCost = PlayerCurrencies.Instance.GetCurrencyUnlockAmount(currency);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUnlocked)
        {
            lockedOverlay.gameObject.SetActive(true);
        }
        else
        {
            lockedOverlay.gameObject.SetActive(false);
        }
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        //Debug.Log($"Tooltip Time!");
        if (PauseManager.Instance.ShouldInputBeBlocked()) return;
        tooltip.gameObject.SetActive(true);
        //hoverFeedback?.PlayFeedbacks();
    }
    public void OnPointerExit(PointerEventData data)
    {
        tooltip.gameObject.SetActive(false);
    }

    public void SwitchDisplayToThisCurrency()
    {
        if (!isUnlocked)
        {
            TryToUnlockCurrency();
        }
        CommodityDisplay.Instance?.ShowDisplay(currency);
    }

    public void TryToUnlockCurrency()
    {
        PlayerCurrencies.Currency currencyToUnlockWith = currency - 1;
        if (PlayerCurrencies.Instance.GetTokensAmount(currencyToUnlockWith) >= unlockCost)
        {
            PlayerCurrencies.Instance.AddCurrency(-unlockCost, currencyToUnlockWith);
            PlayerCurrencies.Instance.UnlockCurrency(currency);
            isUnlocked = true;
            SwitchDisplayToThisCurrency();
        }
        else
        {
            GameEvents.onNotEnoughTokens?.Invoke();
        }
    }
}
