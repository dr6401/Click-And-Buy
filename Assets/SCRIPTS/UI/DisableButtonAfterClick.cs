using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisableButtonAfterClick : MonoBehaviour,IPointerClickHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null)
        {
            button.interactable = false;   
        }
    }
}
