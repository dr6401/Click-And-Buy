using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PermaUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text upgradeText;
    
    public PermaUpgrade permaUpgrade;
    public PermaUpgradeTooltip tooltip;

    private void Start()
    {
        if (permaUpgrade == null) return;
        upgradeText.text = permaUpgrade.augmentName;
        tooltip.Setup(permaUpgrade);
    }

    public void UseUpgrade()
    {
        if (permaUpgrade.IsUpgradeMaxedOut())
        {
            Debug.Log($"Upgrade: {permaUpgrade.augmentName} is maxed out");
            return;
        }
        //Debug.Log($"Upgrade: {permaUpgrade.augmentName} current lvl: {permaUpgrade.GetCurrentRuntimeLevel()}");
        float cost = permaUpgrade.GetCurrentRuntimeCost();
        if (LevelManager.Instance.faith >= cost)
        {
            permaUpgrade.Apply();
            LevelManager.Instance.faith -= cost;
            tooltip.Setup(permaUpgrade);
        }
        else
        {
            GameEvents.OnNotEnoughFaith?.Invoke();
            tooltip?.PlayNotEnoughFaithFeedback();
        }
    }
    public void OnPointerEnter(PointerEventData data)
    {
        //Debug.Log($"Tooltip Time!");
        if (PauseManager.Instance.ShouldInputBeBlocked()) return;
        tooltip.gameObject.SetActive(true);
        tooltip?.PlayShowCurrentFeedback();
        //hoverFeedback?.PlayFeedbacks();
    }
    public void OnPointerExit(PointerEventData data)
    {
        tooltip.gameObject.SetActive(false);
    }
    
}
