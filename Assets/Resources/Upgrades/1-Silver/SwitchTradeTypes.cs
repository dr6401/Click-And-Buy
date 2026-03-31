using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/SwitchTradeTypes")]
public class SwitchTradeTypes : Augment
{
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
            if (aliveTrade.tradeType == TradeType.Buy) aliveTrade.tradeType = TradeType.Sell;
            else if (aliveTrade.tradeType == TradeType.Sell) aliveTrade.tradeType = TradeType.Buy;
        }
    }
}