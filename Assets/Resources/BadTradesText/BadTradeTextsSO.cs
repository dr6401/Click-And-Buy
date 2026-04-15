using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Bad Trades Texts", fileName = "Bad Trades Texts SO")]

public class BadTradeTextsSO : ScriptableObject
{
    public List<string> badTradeTexts;

    public string GetRandomBadTradesText()
    {
        return badTradeTexts[Random.Range(0, badTradeTexts.Count)];
    }
}
