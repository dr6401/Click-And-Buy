using System;
using TMPro;
using UnityEngine;

public class CurrentFundValuation : MonoBehaviour
{
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (LevelManager.Instance.currentFund == null) return;
        text.text = NumberFormatter.FormatDecimalNumber(LevelManager.Instance.currentFund.valuation);
    }
}
