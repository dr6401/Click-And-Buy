using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpInventoryManager : MonoBehaviour
{
    public GameObject hotbarItemPrefab;
    public RectTransform hotbarParentTransform;
    public RectTransform inventoryParentTransform;

    private List<UsablePowerUp> inventory = new();
    public List<HotbarItem> hotbarItems = new List<HotbarItem>();
    private int inventoryAmount = 10;

    public int selectedSlot = 0;

    public static PowerUpInventoryManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    private void Start()
    {
        PopulateHotbar();
        PopulateInventory();
        GameEvents.OnCurrentHotbarSlotChanged(hotbarItems[0]);
    }

    private void Update()
    {
        HandleHotbarInput();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            UsePowerUp();
        }
    }

    public void PopulateHotbar()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject hotbarSlot = Instantiate(hotbarItemPrefab, hotbarParentTransform);
            hotbarItems.Add(hotbarSlot.GetComponent<HotbarItem>());
            //RectTransform rectTransform = hotbarSlot.GetComponent<RectTransform>();
            //rectTransform.anchoredPosition = new Vector2(50f * i, rectTransform.anchoredPosition.y);
        }
    }
    
    public void PopulateInventory()
    {
        for (int i = 0; i < inventoryAmount; i++)
        {
            GameObject hotbarSlot = Instantiate(hotbarItemPrefab, inventoryParentTransform);
            hotbarItems.Add(hotbarSlot.GetComponent<HotbarItem>());
            //inventoryItems.Add(hotbarSlot.GetComponent<HotbarItem>());
            //RectTransform rectTransform = hotbarSlot.GetComponent<RectTransform>();
            //rectTransform.anchoredPosition = new Vector2(50f * i, rectTransform.anchoredPosition.y);
        }
    }

    public void AddPowerUp(Augment augment)
    {
        UsablePowerUp powerUp = new UsablePowerUp { data = augment };

        foreach (HotbarItem hotbarItem in hotbarItems)
        {
            if (hotbarItem.usablePowerUp == null)
            {
                hotbarItem.Setup(powerUp);
                Debug.Log($"Setup at index {hotbarItems.IndexOf(hotbarItem)} with powerUp: {powerUp.data.name}");
                return;
            }

            if (hotbarItem.usablePowerUp.data == augment)
            {
                hotbarItem.usablePowerUp.charges++;
                hotbarItem.Setup(hotbarItem.usablePowerUp);
                Debug.Log(
                    $"Increased charges at index {hotbarItems.IndexOf(hotbarItem)} with powerUp: {powerUp.data.name}");
                return;
            }
        }

        //AddToInventory(powerUp);
        //inventory.Add(powerUp);
        Debug.Log($"Added PowerUp {augment.name} to inventory");

    }

    public void UsePowerUp()
    {
        if (hotbarItems[selectedSlot].usablePowerUp == null) return;
        hotbarItems[selectedSlot].usablePowerUp.Use();
        hotbarItems[selectedSlot].usablePowerUp.charges--;
        hotbarItems[selectedSlot].Setup(hotbarItems[selectedSlot].usablePowerUp);
        if (hotbarItems[selectedSlot].usablePowerUp.charges <= 0)
        {
            hotbarItems[selectedSlot].usablePowerUp = null;
            hotbarItems[selectedSlot].CleanUp();
        }
    }
    
private void HandleHotbarInput()
    {
        bool wasInputGiven = true;
        float scroll = Mouse.current.scroll.ReadValue().y;
        
        if (Keyboard.current.digit1Key.wasPressedThisFrame) selectedSlot = 0;
        else if (Keyboard.current.digit2Key.wasPressedThisFrame) selectedSlot = 1;
        else if (Keyboard.current.digit3Key.wasPressedThisFrame) selectedSlot = 2;
        else if (Keyboard.current.digit4Key.wasPressedThisFrame) selectedSlot = 3;
        else if (Keyboard.current.digit5Key.wasPressedThisFrame) selectedSlot = 4;
        else if (Keyboard.current.digit6Key.wasPressedThisFrame) selectedSlot = 5;
        else if (Keyboard.current.digit7Key.wasPressedThisFrame) selectedSlot = 6;
        else if (Keyboard.current.digit8Key.wasPressedThisFrame) selectedSlot = 7;
        else if (Keyboard.current.digit9Key.wasPressedThisFrame) selectedSlot = 8;
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasReleasedThisFrame || (!ScrollBlocker.IsScrollingUI() && scroll > 0)) selectedSlot = (selectedSlot + 1) % 9;
        else if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasReleasedThisFrame || (!ScrollBlocker.IsScrollingUI() && scroll < 0)) selectedSlot = (selectedSlot - 1 + 9) % 9;
        else
        {
            wasInputGiven = false;
        }
        if (wasInputGiven)
        {
            GameEvents.OnCurrentHotbarSlotChanged(hotbarItems[selectedSlot]);
        }

        //Debug.Log($"Current Selected Slot: {selectedSlot}");
    }

    public bool IsThisHotbarSlotSelected(HotbarItem hotbarItem)
    {
        return hotbarItems[selectedSlot] == hotbarItem;
    }

    public void SetCurrentSlot(HotbarItem hotbarItem)
    {
        int index = hotbarItems.IndexOf(hotbarItem);
        selectedSlot = index;
        GameEvents.OnCurrentHotbarSlotChanged(hotbarItems[selectedSlot]);
    }

    public void Swap(HotbarItem startHotbarItem, HotbarItem targetHotbarItem)
    {
        UsablePowerUp temp = startHotbarItem.usablePowerUp;
        
        startHotbarItem.Setup(targetHotbarItem.usablePowerUp);
        targetHotbarItem.Setup(temp);
                
        Debug.Log($"Swapped items from startIndex: {hotbarItems.IndexOf(targetHotbarItem)} to endIndex: {hotbarItems.IndexOf(startHotbarItem)}");
    }

    public bool AreAllSlotsFull()
    {
        foreach (HotbarItem hotbarItem in hotbarItems)
        {
            if (hotbarItem.usablePowerUp == null) return false;
        }
        return true;
    }
}
