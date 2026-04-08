using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public TMP_Text chargesLeftText;
    public UsablePowerUp usablePowerUp;
    public RectTransform iconParent;
    public Image icon;
    public Image coloredBackground;
    public HotbarItemTooltip tooltip;

    public bool hasPowerUp = false;

    public RectTransform hotbarRoot;
    private GameObject ghost;
    private RectTransform ghostRect;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player hotbarItemSelectedFeedback;
    [SerializeField] private MMF_Player hotbarItemDeselectedFeedback;
    private void Start()
    {
        RectTransform tempTransform = GetComponent<RectTransform>();
        Transform temp = tempTransform.transform;
        while (temp != null && !temp.CompareTag("HotbarRoot"))
        {
            temp = temp.parent;
        }

        hotbarRoot = temp.GetComponent<RectTransform>();
        
        CleanUp();
    }

    public void Setup(UsablePowerUp powerUp)
    {
        if (powerUp == null)
        {
            CleanUp();
            return;
        }
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

        if (PowerUpInventoryManager.Instance.IsThisHotbarSlotSelected(this))
        {
            tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        //Debug.Log($"Tooltip Time!");
        if (!hasPowerUp) return;
        tooltip.gameObject.SetActive(true);
        GameEvents.OnHotbarItemTooltipShowed(this);
        PowerUpInventoryManager.Instance.SetCurrentSlot(this);
        //Debug.Log($"Showed Tooltip");
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!hasPowerUp) return;
        PowerUpInventoryManager.Instance.UsePowerUp();
    }

    public void OnPointerExit(PointerEventData data)
    {
        //Debug.Log($"Tooltip Time!");
        if (!hasPowerUp) return;
        tooltip.gameObject.SetActive(false);
        //Debug.Log($"Hidden Tooltip");
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
        
        tooltip.gameObject.SetActive(false);
        Debug.Log($"Shit cleaned up");
    }

    private void PlayHotbarItemSelectedFeedback()
    {
        hotbarItemDeselectedFeedback?.StopFeedbacks();
        hotbarItemSelectedFeedback?.PlayFeedbacks();
    }
    
    private void PlayHotbarItemDeselectedFeedback()
    {
        hotbarItemSelectedFeedback?.StopFeedbacks();
        hotbarItemDeselectedFeedback?.PlayFeedbacks();
    }

    private void RespondToHotbarSelectedItemChanged(HotbarItem current)
    {
        if (current == this)
        {
            PlayHotbarItemSelectedFeedback();
            if (hasPowerUp) tooltip.gameObject.SetActive(true);
        }
        else if (iconParent.localScale.x > 1)
        {
            PlayHotbarItemDeselectedFeedback();
            tooltip.gameObject.SetActive(false);
        }
        else
        {
            tooltip.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnCurrentHotbarSlotChanged += RespondToHotbarSelectedItemChanged;
        GameEvents.OnHotbarItemTooltipShowed += RespondToHotbarSelectedItemChanged;
    }
    
    private void OnDisable()
    {
        GameEvents.OnCurrentHotbarSlotChanged -= RespondToHotbarSelectedItemChanged;
        GameEvents.OnHotbarItemTooltipShowed -= RespondToHotbarSelectedItemChanged;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!hasPowerUp) return;
        
        Debug.Log($"Began Dragging");

        CreateGhostDragImage();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!hasPowerUp) return;
        
        ghostRect.position = eventData.position;
        Debug.Log($"Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"Stopped dragging");
        Destroy(ghost);
    }

    public void OnDrop(PointerEventData eventData)
    {
        HotbarItem dragged = eventData.pointerDrag.GetComponent<HotbarItem>();
        if (dragged.usablePowerUp == null) return;
        
        PowerUpInventoryManager.Instance.Swap(this, dragged);
    }

    private void CreateGhostDragImage()
    {
        ghost = new GameObject("Ghost");
        ghost.transform.SetParent(hotbarRoot, false);
        
        ghostRect = ghost.AddComponent<RectTransform>();
        ghostRect.anchorMin = new Vector2(0.5f, 0.5f);
        ghostRect.anchorMax = new Vector2(0.5f, 0.5f);
        ghostRect.pivot = new Vector2(0.5f, 0.5f);
        
        ghostRect.sizeDelta = new Vector2(75f, 75f);
        
        // Background
        GameObject backgroundGO = new GameObject("Icon");
        backgroundGO.transform.SetParent(ghost.transform, false);
        
        Image imgBg = backgroundGO.AddComponent<Image>();
        imgBg.color = coloredBackground.color;
        imgBg.raycastTarget = false;
        imgBg.preserveAspect = true;
        
        RectTransform backgroundRect = backgroundGO.GetComponent<RectTransform>();
        backgroundRect.anchorMin = new Vector2(0f, 0f);
        backgroundRect.anchorMax = new Vector2(1f, 1f);
        backgroundRect.offsetMin = new Vector2(0f, 0f);
        backgroundRect.offsetMax = new Vector2(0f, 0f);
        
        // Icon
        GameObject iconGO = new GameObject("Icon");
        iconGO.transform.SetParent(ghost.transform, false);
        
        Image imgIcon = iconGO.AddComponent<Image>();
        imgIcon.sprite = icon.sprite;
        imgIcon.raycastTarget = false;
        imgIcon.preserveAspect = true;
        
        RectTransform iconRect = iconGO.GetComponent<RectTransform>();
        iconRect.anchorMin = new Vector2(0f, 0f);
        iconRect.anchorMax = new Vector2(1f, 1f);
        iconRect.offsetMin = new Vector2(0f, 0f);
        iconRect.offsetMax = new Vector2(0f, 0f);
        
    }
}
