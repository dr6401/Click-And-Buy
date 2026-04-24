using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommodityDisplay : MonoBehaviour
{
    public List<GameObject> commodityDisplays;
    public int index;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            commodityDisplays.Add(transform.GetChild(i).gameObject);
        }
        index = 0;
    }

    void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame && GameConstants.isDevHacksEnabled)
        {
            CycleRight();
        }
        else if (Keyboard.current.jKey.wasPressedThisFrame && GameConstants.isDevHacksEnabled)
        {
            CycleLeft();
        }
    }

    private void CycleRight()
    {
        int currencyCount = Enum.GetValues(typeof(PlayerCurrencies.Currency)).Length;
        int next = ((int)LevelManager.Instance.currentCurrency + 1 + currencyCount) % currencyCount;
        LevelManager.Instance.currentCurrency = (PlayerCurrencies.Currency)next;
        ClearDisplay();
        ShowDisplayOnIndex(next);
    }
    
    private void CycleLeft()
    {
        int currencyCount = Enum.GetValues(typeof(PlayerCurrencies.Currency)).Length;
        int prev = ((int)LevelManager.Instance.currentCurrency - 1 + currencyCount) % currencyCount;
        LevelManager.Instance.currentCurrency = (PlayerCurrencies.Currency)prev;
        ClearDisplay();
        ShowDisplayOnIndex(prev);
    }

    private void ShowDisplayOnIndex(int displayIndex)
    {
        commodityDisplays[displayIndex].SetActive(true);
    }

    private void ClearDisplay()
    {
        foreach (GameObject display in commodityDisplays)
        {
            display.SetActive(false);
        }
    }
}
