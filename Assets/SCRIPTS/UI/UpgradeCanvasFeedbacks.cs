using MoreMountains.Feedbacks;
using UnityEngine;

public class UpgradeCanvasFeedbacks : MonoBehaviour
{
    [SerializeField] private MMF_Player fadeInUpgradeCanvasFeedback;
    [SerializeField] private MMF_Player fadeOutUpgradeCanvasFeedback;

    private void PlayFadeInUpgradeCanvasFeedback()
    {
        fadeInUpgradeCanvasFeedback?.PlayFeedbacks();
    }
    private void PlayFadeOutUpgradeCanvasFeedback()
    {
        fadeOutUpgradeCanvasFeedback?.PlayFeedbacks();
    }

    private void Awake()
    {
        GameEvents.OnLevelUp += PlayFadeInUpgradeCanvasFeedback;
    }
    
    private void OnDestroy()
    {
        GameEvents.OnLevelUp -= PlayFadeInUpgradeCanvasFeedback;
    }
}
