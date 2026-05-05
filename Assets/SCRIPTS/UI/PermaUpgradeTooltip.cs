using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PermaUpgradeTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text price;
    [SerializeField] private TMP_Text currentStats;
    [SerializeField] private GameObject faithIcon;
    [SerializeField] private HorizontalLayoutGroup priceLayoutGroup;
    [SerializeField] private GameObject wingLeftIcon;
    [SerializeField] private GameObject wingRightIcon;

    [SerializeField] private MMF_Player openTooltipFeedback;
    [SerializeField] private MMF_Player notEnoughFaithFeedback;
    [SerializeField] private MMF_Player showCurrentFeedback;

    public void Setup(PermaUpgrade powerUp)
    {
        if (powerUp == null) return;
        name.text = powerUp.augmentName;
        name.color = powerUp.color;
        description.text = powerUp.GetDescription();
        if (!powerUp.IsUpgradeMaxedOut())
        {
            price.text = $"PRICE:<color=#FFF390> {NumberFormatter.FormatNumber(powerUp.GetCurrentRuntimeCost())}</color>";   
        }
        else
        {
            price.text = $"<color=#FFF390>MAX</color>";
            CenterMaxText();
        }

        currentStats.text = $"<color=#{ColorUtility.ToHtmlStringRGB(powerUp.color)}>{powerUp.leftText}{NumberFormatter.FormatNumber(powerUp.GetCurrentRuntimeValue())}{powerUp.rightText}";
    }

    private void CenterMaxText()
    {
        faithIcon?.SetActive(false);
        price.alignment = TextAlignmentOptions.Center;
        priceLayoutGroup.spacing = -25;
        priceLayoutGroup.padding.right = 0;
        wingLeftIcon?.SetActive(true);
        wingRightIcon?.SetActive(true);
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