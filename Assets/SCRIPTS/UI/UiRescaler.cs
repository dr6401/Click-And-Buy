using System;
using UnityEngine;

public class UiRescaler : MonoBehaviour
{
    [SerializeField] private Transform uiElement;
    [SerializeField] private float targetScale;

    private void Awake()
    {
        Vector3 newScale = new Vector3(targetScale, targetScale, targetScale);
        uiElement.localScale = newScale;
    }
}
