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

    public void Setup(UsablePowerUp powerUp)
    {
        usablePowerUp = powerUp;
        chargesLeftText.text = NumberFormatter.FormatNumber(powerUp.charges);
        icon.sprite = usablePowerUp.data.icon;
        Color augmentColor =  usablePowerUp.data.color;
        coloredBackground.color = augmentColor;
        hasPowerUp = true;
        
        tooltip.Setup(powerUp);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (!hasPowerUp) return;
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (!hasPowerUp) return;
        tooltip.gameObject.SetActive(false);
    }

    public void CleanUp()
    {
        chargesLeftText.text = "";
        usablePowerUp = null;
        icon.sprite = null;
        
        coloredBackground.color = Color.white;
        hasPowerUp = false;
    }
}
