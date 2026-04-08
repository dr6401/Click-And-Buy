using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollBlocker : MonoBehaviour, IScrollHandler
{
    public static int lastScrollFrame;

    public void OnScroll(PointerEventData eventData)
    {
        lastScrollFrame = Time.frameCount;
    }

    public static bool IsScrollingUI()
    {
        return lastScrollFrame == Time.frameCount;
    }
}
