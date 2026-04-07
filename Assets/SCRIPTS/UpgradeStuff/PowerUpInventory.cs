using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpInventory : MonoBehaviour
{
    private List<UsablePowerUp> inventory = new();
    public UsablePowerUp[] hotbar = new UsablePowerUp[9];
    
    private int selectedSlot = 0;

    private void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Keyboard.current.Num)
        }
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            UsePowerUp();
        }
    }

    public void AddPowerUp(Augment augment)
    {
        UsablePowerUp powerUp = new UsablePowerUp { data = augment };

        for (int i = 0; i < hotbar.Length; i++)
        {
            if (hotbar[i] == null)
            {
                hotbar[i] = powerUp;
                return;
            }
        }
        
        // If no space in hotbar
        inventory.Add(powerUp);
    }

    public void UsePowerUp()
    {
        hotbar[selectedSlot]?.Use();
        hotbar[selectedSlot].charges--;
        if (hotbar[selectedSlot].charges <= 0)
        {
            hotbar[selectedSlot] = null;
        }
    }
}
