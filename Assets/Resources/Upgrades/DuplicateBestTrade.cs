using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/DuplicateBestTrade")]
public class DuplicateBestTrade : Augment
{
    public override void Apply()
    {
        if (LevelManager.Instance == null || LevelManager.Instance.activeTrades.Count <= 0) return;
        List<TradeEntryStatsDisplay> activeTrades = LevelManager.Instance.activeTrades;
        TradeEntryStatsDisplay bestTrade = activeTrades[0];
        foreach (TradeEntryStatsDisplay tradeEntry in activeTrades)
        {
            if (tradeEntry.GetUnrealizedProfit() > bestTrade.GetUnrealizedProfit())
            {
                bestTrade = tradeEntry;
            }
        }
        Debug.Log($"Gonna duplicate best trade (trade with profit: {bestTrade.GetUnrealizedProfit()})");
        LevelManager.Instance.DuplicateTrade(bestTrade);
    }
}