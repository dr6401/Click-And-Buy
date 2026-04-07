using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/ConvertAllTradesType")]
public class ConvertAllTradesType : Augment
{
    
    public TradeType tradeType;
    public override void Apply(GameObject player)
    {
        if (LevelManager.Instance == null) return;
        SwitchTrades(LevelManager.Instance.activeTrades);
    }

    private void SwitchTrades(List<TradeEntryStatsDisplay> aliveTrades)
    {
        if (aliveTrades.Count == 0) return;
        foreach (var aliveTrade in aliveTrades)
        {
            aliveTrade.tradeType = tradeType;
        }
    }
}