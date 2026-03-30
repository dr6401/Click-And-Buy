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
    private GameObject player;
    private UpgradesSelectionUI upgradesSelectionUI;

    
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
    }
    public void Setup(Augment aug, GameObject playerRef, UpgradesSelectionUI parentUI)
    {
        augment = aug;
        player = playerRef;
        upgradesSelectionUI = parentUI;

        augmentName.text = augment.augmentName;
        augmentDescription.text = augment.description;
        iconImage.sprite = augment.icon;
        
        gradient.color = augment.color;
        Color transparentColor = gradient.color;
        transparentColor.a = gradient.color.a / 2f;
        top.color = transparentColor;//DarkenColor(augment.color, 0.7f);
    }
    
    public void Setup(Augment aug, GameObject playerRef)
    {
        augment = aug;
        player = playerRef;

        augmentName.text = augment.augmentName;
        augmentDescription.text = augment.description;
        iconImage.sprite = augment.icon;

    }

    public void SelectAugment()
    {
        upgradesSelectionUI.StoreChosenAugment(augment);
        augment.Apply(player);
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
}
