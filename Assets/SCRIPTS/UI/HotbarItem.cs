using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text chargesLeftText;
    public UsablePowerUp usablePowerUp;
    public Image icon;
    public Image coloredBackground;
    public HotbarItemTooltip tooltip;

    public bool hasPowerUp = false;

    private void Start()
    {
        CleanUp();
    }

    public void Setup(UsablePowerUp powerUp)
    {
        usablePowerUp = powerUp;
        chargesLeftText.text = NumberFormatter.FormatNumber(powerUp.charges);
        icon.sprite = usablePowerUp.data.icon;
        
        Color spriteColor = icon.color;
        spriteColor.a = 1;
        icon.color = spriteColor;
        
        Color backgroundColor = usablePowerUp.data.color;
        backgroundColor.a = 0.5f;
        coloredBackground.color = backgroundColor;
        
        hasPowerUp = true;
        
        tooltip.Setup(powerUp);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log($"Tooltip Time!");
        if (!hasPowerUp) return;
        tooltip.gameObject.SetActive(true);
        Debug.Log($"Showed Tooltip");
    }

    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log($"Tooltip Time!");
        if (!hasPowerUp) return;
        tooltip.gameObject.SetActive(false);
        Debug.Log($"Hidden Tooltip");
    }

    public void CleanUp()
    {
        chargesLeftText.text = "";
        Debug.Log($"chargesLeftText.text: {chargesLeftText.text}");
        usablePowerUp = null;
        icon.sprite = null;
        
        Color spriteColor = icon.color;
        spriteColor.a = 0;
        icon.color = spriteColor;
        
        Color backgroundColor = coloredBackground.color;
        backgroundColor.a = 0;
        coloredBackground.color = backgroundColor;
        
        hasPowerUp = false;
        Debug.Log($"Shit cleaned up");
    }
}
