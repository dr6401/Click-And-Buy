using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/ReturnPolicy")]
public class ReturnPolicy : Augment
{
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        TradeEntryStatsDisplay worstTrade = LevelManager.Instance.GetWorstTrade();
        if (worstTrade == null)
        {
            Debug.Log($"ReturnPolicy: worstTrade is null");
            return;
        }

        if (worstTrade.GetUnrealizedProfit() < 0)
        {
            worstTrade.CloseWithoutLosses();   
        }
        else
        {
            worstTrade.Close();
        }
    }
}