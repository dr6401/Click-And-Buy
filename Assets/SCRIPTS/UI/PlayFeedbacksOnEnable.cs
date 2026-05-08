using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayFeedbacksOnEnable : MonoBehaviour
{
    [SerializeField] private MMF_Player openFeedback;

    private void PlayOpenFeedback()
    {
        openFeedback?.PlayFeedbacks();
    }

    private void OnEnable()
    {
        PlayOpenFeedback();
    }
}
