using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private MMF_Player notEnoughMoneyFeedback;

    private void PlayNotEnoughMoneyFeedback()
    {
        notEnoughMoneyFeedback?.PlayFeedbacks();
    }

    private void OnEnable()
    {
        GameEvents.onNotEnoughMoney += PlayNotEnoughMoneyFeedback;
    }
    
    private void OnDisable()
    {
        GameEvents.onNotEnoughMoney -= PlayNotEnoughMoneyFeedback;
    }
}
