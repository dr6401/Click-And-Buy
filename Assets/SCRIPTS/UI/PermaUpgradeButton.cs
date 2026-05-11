using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PermaUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text upgradeText;
    
    public PermaUpgrade permaUpgrade;
    public PermaUpgradeTooltip tooltip;
    
    public PermaUpgradeButton previousUpgrade;
    public bool isPreviousUpgradeUnlocked = false;

    public MMF_Player unlockPreviousFeedback;

    private void Start()
    {
        if (permaUpgrade == null) return;
        if (!isPreviousUpgradeUnlocked)
        {
            upgradeText.text = "LOCKED";
        }
        else
        {
            upgradeText.text = permaUpgrade.augmentName;
        }
        tooltip.Setup(permaUpgrade);
    }

    private void Update()
    {
        if (previousUpgrade == null) isPreviousUpgradeUnlocked = true;
        else
        {
            isPreviousUpgradeUnlocked = !(previousUpgrade.permaUpgrade.GetCurrentRuntimeLevel() <= 0); 
        }
        if (!isPreviousUpgradeUnlocked)
        {
            upgradeText.text = "LOCKED";
        }
        else
        {
            upgradeText.text = permaUpgrade.augmentName;
        }
    }

    public void UseUpgrade()
    {
        if (permaUpgrade.IsUpgradeMaxedOut())
        {
            Debug.Log($"Upgrade: {permaUpgrade.augmentName} is maxed out");
            return;
        }

        if (!isPreviousUpgradeUnlocked)
        {
            Debug.Log($"Previous upgrade ({previousUpgrade.permaUpgrade.augmentName}) not unlocked yet ({previousUpgrade.permaUpgrade.augmentName} currentRuntimeLevel: {previousUpgrade.permaUpgrade.GetCurrentRuntimeLevel()})!");
            unlockPreviousFeedback?.PlayFeedbacks();
            return;
        }
        //Debug.Log($"Upgrade: {permaUpgrade.augmentName} current lvl: {permaUpgrade.GetCurrentRuntimeLevel()}");
        float cost = permaUpgrade.GetCurrentRuntimeCost();
        if (PlayerStats.Instance.faith >= cost)
        {
            permaUpgrade.Apply();
            PlayerStats.Instance.faith -= cost;
            tooltip.Setup(permaUpgrade);
            GameEvents.OnPermaUpgradeUpgraded?.Invoke();
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
        if (PauseManager.Instance.ShouldInputBeBlocked() || !isPreviousUpgradeUnlocked) return;
        tooltip.gameObject.SetActive(true);
        tooltip?.PlayShowCurrentFeedback();
        //hoverFeedback?.PlayFeedbacks();
    }
    public void OnPointerExit(PointerEventData data)
    {
        tooltip.gameObject.SetActive(false);
    }
    
}
