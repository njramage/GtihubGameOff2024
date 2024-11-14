using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDragging;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Make sure that the card being dragged is always on top
        parentAfterDragging = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDragging);
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var ray in results)
        {
            if (ray.gameObject != this.gameObject && ray.gameObject.CompareTag("Card"))
            {
                Debug.Log(ray.gameObject.name);
            }
        }
    }
}