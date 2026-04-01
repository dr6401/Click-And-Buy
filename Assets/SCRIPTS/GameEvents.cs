using System;
using UnityEngine;

public static class GameEvents
{
    
    public static Action<bool> OnGamePaused;

    public static Action onMoneySpent;
    public static Action onNotEnoughMoney;
    public static Action onMoneyEarned;
    public static Action onMoneyLost;
    public static Action onNotEnoughAliveTrades;
    
    public static Action onVictory;
    public static Action onDefeat;
    
    public static Action<AugmentTier> OnCashOut;
    public static Action OnPlayerDeath;
    public static Action OnUpgradesOffered;
    public static Action OnUpgradeChosen;
}
