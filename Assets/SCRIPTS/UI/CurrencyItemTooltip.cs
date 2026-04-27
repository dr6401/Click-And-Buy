using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class CurrencyItemTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text unlockStatus;

    [SerializeField] private MMF_Player openTooltipFeedback;

    private void OnEnable()
    {
        openTooltipFeedback.PlayFeedbacks();
    }
}