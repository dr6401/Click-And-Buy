using System;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    public TMP_Text tooltipText;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowTooltip(String text, Vector3 position)
    {
        tooltipText.text = text;
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
