using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpInventory : MonoBehaviour
{
    public GameObject hotbarItemPrefab;
    public RectTransform hotbarParentTransform;
    
    private List<UsablePowerUp> inventory = new();
    public List<HotbarItem> hotbarItems = new List<HotbarItem>();
    
    private int selectedSlot = 0;
    
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        PopulateHotbar();
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
            //RectTransform rectTransform = hotbarSlot.GetComponent<RectTransform>();
            //rectTransform.anchoredPosition = new Vector2(50f * i, rectTransform.anchoredPosition.y);
        }
    }

    public void AddPowerUp(Augment augment)
    {
        UsablePowerUp powerUp = new UsablePowerUp { data = augment };

        foreach (var hotbarItem in hotbarItems)
        {
            if (hotbarItem.usablePowerUp.data == augment)
            {
                hotbarItem.usablePowerUp.charges++;
                return;
            }
            if (hotbarItem.usablePowerUp == null)
            {
                hotbarItem.Setup(powerUp);
                return;
            }
        }
        inventory.Add(powerUp);

    }

    public void UsePowerUp()
    {
        if (hotbarItems[selectedSlot].usablePowerUp == null) return;
        hotbarItems[selectedSlot].usablePowerUp.Use();
        hotbarItems[selectedSlot].usablePowerUp.charges--;
        if (hotbarItems[selectedSlot].usablePowerUp.charges <= 0)
        {
            hotbarItems[selectedSlot].usablePowerUp = null;
            hotbarItems[selectedSlot].CleanUp();
        }
    }

    private void HandleHotbarInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) selectedSlot = 0;
        else if (Keyboard.current.digit2Key.wasPressedThisFrame) selectedSlot = 1;
        else if (Keyboard.current.digit3Key.wasPressedThisFrame) selectedSlot = 2;
        else if (Keyboard.current.digit4Key.wasPressedThisFrame) selectedSlot = 3;
        else if (Keyboard.current.digit5Key.wasPressedThisFrame) selectedSlot = 4;
        else if (Keyboard.current.digit6Key.wasPressedThisFrame) selectedSlot = 5;
        else if (Keyboard.current.digit7Key.wasPressedThisFrame) selectedSlot = 6;
        else if (Keyboard.current.digit8Key.wasPressedThisFrame) selectedSlot = 7;
        else if (Keyboard.current.digit9Key.wasPressedThisFrame) selectedSlot = 8;
    }
}
