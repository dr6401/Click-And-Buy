using TMPro;
using UnityEngine;

public class SellFundDisclaimer : MonoBehaviour
{
    private TMP_Text disclaimerText;
    
    private void Start()
    {
        disclaimerText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if (disclaimerText == null && LevelManager.Instance == null) return;
        disclaimerText.text =
            $"This will automatically <color=#FF414A><b>close all open positions</b></color>\n\nYour sold fund will generate <color=#77FF41>{NumberFormatter.FormatDecimalNumber(LevelManager.Instance.currentFund.passiveIncome)}$/s</color>\n";
    }
}
