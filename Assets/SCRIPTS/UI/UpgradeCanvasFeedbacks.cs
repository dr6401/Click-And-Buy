using MoreMountains.Feedbacks;
using UnityEngine;

public class UpgradeCanvasFeedbacks : MonoBehaviour
{
    [SerializeField] private MMF_Player fadeInUpgradeCanvasFeedback;
    [SerializeField] private MMF_Player fadeOutUpgradeCanvasFeedback;

    private void PlayFadeInUpgradeCanvasFeedback()
    {
        fadeInUpgradeCanvasFeedback?.PlayFeedbacks();
        Debug.Log($"Playing FadeIn Upgrade Canvas");
    }
    private void PlayFadeOutUpgradeCanvasFeedback()
    {
        fadeOutUpgradeCanvasFeedback?.PlayFeedbacks();
    }

    private void OnEnable()
    {
        GameEvents.OnUpgradesOffered += PlayFadeInUpgradeCanvasFeedback;
    }
    
    private void OnDisable()
    {
        GameEvents.OnUpgradesOffered -= PlayFadeInUpgradeCanvasFeedback;
    }
}
