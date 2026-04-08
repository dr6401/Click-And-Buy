using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnDragGhost : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject ghost;
    private RectTransform ghostRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"Began Dragging");
        ghost = new GameObject("Ghost");
        
        Image img = ghost.AddComponent<Image>();
        img.raycastTarget = false;

        ghostRect = ghost.GetComponent<RectTransform>();
        ghostRect.sizeDelta = new Vector2(100f, 100f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"Dragging Blabla sdfs");
        ghostRect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"Stopped Dragging");
        Destroy(ghost);
    }
}
