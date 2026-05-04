using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class PermaUpgradeTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text currentStats;

    [SerializeField] private MMF_Player openTooltipFeedback;

    public void Setup(UsablePowerUp powerUp)
    {
        Augment data = powerUp.data;
        name.text = data.name;
        name.color = powerUp.data.color;
        description.text = data.description;
        currentStats.text = NumberFormatter.FormatNumber(powerUp.charges) + " left";
    }

    private void OnEnable()
    {
        openTooltipFeedback.PlayFeedbacks();
    }
}