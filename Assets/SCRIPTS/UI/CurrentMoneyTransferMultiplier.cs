using TMPro;
using UnityEngine;

public class CurrentMoneyTransferMultiplier : MonoBehaviour
{
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (PlayerStats.Instance == null) return;
        text.text = $"<color=#77FF41>{NumberFormatter.FormatDecimalNumber(PlayerStats.Instance.moneyTransferMultiplier * 100f)}%</color> Money Transfer";
    }
}
