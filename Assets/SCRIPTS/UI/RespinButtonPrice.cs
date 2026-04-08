using TMPro;
using UnityEngine;

public class RespinButtonPrice : MonoBehaviour
{
    [SerializeField] private TMP_Text respinButtonText;
    
    void Update()
    {
        if (LevelManager.Instance == null) return;
        respinButtonText.text = $"New Offer: " + NumberFormatter.FormatDecimalNumber(LevelManager.Instance.currentRespinPrice) + "$";
    }
}
