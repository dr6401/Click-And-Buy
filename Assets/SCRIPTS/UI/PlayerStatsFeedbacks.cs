using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerStatsFeedbacks : MonoBehaviour
{
    [SerializeField] private MMF_Player notEnoughMoneyFeedback;
    [SerializeField] private MMF_Player notEnoughTokensFeedback;
    [SerializeField] private MMF_Player notEnoughFaithFeedback;

    private void PlayNotEnoughMoneyFeedback()
    {
        notEnoughMoneyFeedback?.PlayFeedbacks();
    }
    
    private void PlayNotEnoughTokensFeedback()
    {
        notEnoughTokensFeedback?.PlayFeedbacks();
    }

    private void PlayNotEnoughFaithFeedback()
    {
        notEnoughFaithFeedback?.PlayFeedbacks();
    }

    private void OnEnable()
    {
        GameEvents.onNotEnoughMoney += PlayNotEnoughMoneyFeedback;
        GameEvents.onNotEnoughTokens += PlayNotEnoughTokensFeedback;
        GameEvents.OnNotEnoughFaith += PlayNotEnoughFaithFeedback;
    }
    
    private void OnDisable()
    {
        GameEvents.onNotEnoughMoney -= PlayNotEnoughMoneyFeedback;
        GameEvents.onNotEnoughTokens -= PlayNotEnoughTokensFeedback;
        GameEvents.OnNotEnoughFaith -= PlayNotEnoughFaithFeedback;
    }
}
