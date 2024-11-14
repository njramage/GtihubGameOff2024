using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDragging;

    private Location? _location;
    private Tool? _tool;
    private Crime? _crime;
    private Feature? _feature;

    public void Setup(Location? location, Tool? tool, Crime? crime, Feature? feature)
    {
        _location = location;
        _tool = tool;
        _crime = crime;
        _feature = feature;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Make sure that the card being dragged is always on top
        parentAfterDragging = transform.parent;
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
            if (ray.gameObject.CompareTag("Card"))
            {
                ray.gameObject.GetComponent<Card>().OnCardMerge();
            }
        }
    }

    public void OnCardMerge()
    {
        GameManager.Instance.MergeEvent.Invoke(_location, _tool, _crime, _feature);
        Destroy(gameObject);
    }
}