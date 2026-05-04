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
        upgradeText.text = permaUpgrade.name;
        tooltip.Setup(permaUpgrade);
    }

    public void UseUpgrade()
    {
        if (LevelManager.Instance.faith >= permaUpgrade.baseCost)
        {
            permaUpgrade.Apply();
            LevelManager.Instance.faith -= permaUpgrade.baseCost;
            tooltip.Setup(permaUpgrade);
        }
        else
        {
            GameEvents.OnNotEnoughFaith?.Invoke();
        }
    }
    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log($"Tooltip Time!");
        if (PauseManager.Instance.ShouldInputBeBlocked()) return;
        tooltip.gameObject.SetActive(true);
        //hoverFeedback?.PlayFeedbacks();
    }
    public void OnPointerExit(PointerEventData data)
    {
        tooltip.gameObject.SetActive(false);
    }
    
}
