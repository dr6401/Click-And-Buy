using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class PermaUpgradeTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text currentStats;

    [SerializeField] private MMF_Player openTooltipFeedback;

    public void Setup(PermaUpgrade powerUp)
    {
        if (powerUp == null) return;
        name.text = powerUp.name;
        name.color = powerUp.color;
        description.text = powerUp.description;
        currentStats.text = powerUp.leftText + NumberFormatter.FormatNumber(powerUp.GetCurrentRuntimeValue()) + powerUp.rightText;
    }

    private void OnEnable()
    {
        openTooltipFeedback.PlayFeedbacks();
    }
}