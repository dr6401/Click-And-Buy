using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/TimedExit")]
public class TimedExit : Augment
{
    public override void Apply()
    {
        if (LevelManager.Instance == null || CoroutineManager.Instance == null) return;
        List<TradeEntryStatsDisplay> tempList = new List<TradeEntryStatsDisplay>();
        
        foreach (TradeEntryStatsDisplay trade in LevelManager.Instance.activeTrades)
        {
            if (trade.GetUnrealizedProfit() > 0)
            {
                tempList.Add(trade);
            }
        }
        CoroutineManager.Instance.StartCoroutine(CloseAllOpenTrades(tempList));
    }

    private IEnumerator CloseAllOpenTrades(List<TradeEntryStatsDisplay> list)
    {
        foreach (TradeEntryStatsDisplay trade in list)
        {
            trade.Close();
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}