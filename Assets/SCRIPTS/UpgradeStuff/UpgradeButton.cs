using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text augmentName;
    [SerializeField] private TMP_Text augmentDescription;
    [SerializeField] private Image iconImage;

    [Header("Color Correction")]
    [SerializeField] private Image gradient;
    [SerializeField] private Image top;

    private Augment augment;
    private UpgradesSelectionUI upgradesSelectionUI;
    private Button button;

    
    void Start()
    {
        if (augmentName == null)
        {
            augmentName = GetComponentInChildren<TMP_Text>();
        }
        if (iconImage == null)
        {
            iconImage = GetComponent<Image>();
        }
        button = GetComponent<Button>();
        button.interactable = false;
        StartCoroutine(EnableButtonInteraction(0.75f));

    }
    public void Setup(Augment aug, UpgradesSelectionUI parentUI)
    {
        augment = aug;
        upgradesSelectionUI = parentUI;

        augmentName.text = augment.augmentName;
        augmentDescription.text = augment.description;
        iconImage.sprite = augment.icon;
        
        gradient.color = augment.color;
        Color transparentColor = gradient.color;
        transparentColor.a = gradient.color.a / 2f;
        top.color = transparentColor;//DarkenColor(augment.color, 0.7f);
    }
    
    public void Setup(Augment aug)
    {
        augment = aug;

        augmentName.text = augment.augmentName;
        augmentDescription.text = augment.description;
        iconImage.sprite = augment.icon;

    }

    public void SelectAugment()
    {
        upgradesSelectionUI.StoreChosenAugment(augment);
        if (augment.autoApply)
        {
            augment.Apply();
        }
        else
        {
            PowerUpInventoryManager.Instance.AddPowerUp(augment);
        }
        upgradesSelectionUI.CloseUI();
        Debug.Log("Selected " + augment.augmentName + "!");
        GameEvents.OnUpgradeChosen?.Invoke();
        //GameEvents.OnHasSettingsUICoveredUpAugmentUI?.Invoke(false);
    }

    public Color DarkenColor(Color color, float percentage)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        v *= percentage;
        v = Mathf.Clamp01(v);
        
        Color darker = Color.HSVToRGB(h, s, v);
        darker.a = color.a;
        
        return darker;
    }

    private IEnumerator EnableButtonInteraction(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        button.interactable = true;
    }
}
