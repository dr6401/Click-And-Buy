using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class PermaUpgradeTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text price;
    [SerializeField] private TMP_Text currentStats;

    [SerializeField] private MMF_Player openTooltipFeedback;
    [SerializeField] private MMF_Player notEnoughFaithFeedback;
    [SerializeField] private MMF_Player showCurrentFeedback;

    public void Setup(PermaUpgrade powerUp)
    {
        if (powerUp == null) return;
        name.text = powerUp.name;
        name.color = powerUp.color;
        description.text = powerUp.description;
        if (!powerUp.IsUpgradeMaxedOut())
        {
            price.text = $"PRICE: <color=#FFF390> {NumberFormatter.FormatNumber(powerUp.GetCurrentRuntimeCost())}</color>";   
        }
        else
        {
            price.text = $"PRICE: <color=#FFF390>MAX</color>";
        }
        currentStats.text = powerUp.leftText + NumberFormatter.FormatNumber(powerUp.GetCurrentRuntimeValue()) + powerUp.rightText;
    }

    public void PlayNotEnoughFaithFeedback()
    {
        notEnoughFaithFeedback?.PlayFeedbacks();
    }

    public void PlayShowCurrentFeedback()
    {
        showCurrentFeedback?.PlayFeedbacks();
    }

    private void OnEnable()
    {
        openTooltipFeedback.PlayFeedbacks();
    }
}