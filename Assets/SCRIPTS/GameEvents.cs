using System;
using UnityEngine;

public static class GameEvents
{
    
    public static Action<bool> OnGamePaused;

    public static Action onMoneySpent;
    public static Action onNotEnoughMoney;
    public static Action onNotEnoughTokens;
    public static Action OnNotEnoughFaith;
    public static Action onMoneyEarned;
    public static Action onMoneyLost;
    public static Action onNotEnoughAliveTrades;
    
    public static Action onVictory;
    public static Action onDefeat;

    public static Action OnFTUETriggered;
    public static Action OnFTUEEnded;
    
    public static Action<AugmentTier> OnCashOut;
    public static Action OnDivineCashOut;
    public static Action OnPlayerDeath;
    public static Action OnUpgradesOffered;
    public static Action OnUpgradeChosen;

    public static Action<HotbarItem> OnCurrentHotbarSlotChanged;
    public static Action<HotbarItem> OnHotbarItemTooltipShowed;

    public static Action<GameObject> OnMainPanelChanged;
}
