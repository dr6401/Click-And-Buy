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
        index++;
        index = (index + commodityDisplays.Count) % commodityDisplays.Count;
        ClearDisplay();
        ShowDisplayOnIndex(index);
    }
    
    private void CycleLeft()
    {
        index--;
        index = (index + commodityDisplays.Count) % commodityDisplays.Count;
        ClearDisplay();
        ShowDisplayOnIndex(index);
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
