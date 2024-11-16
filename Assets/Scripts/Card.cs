using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDragging;

    private Location? _location;
    private Tool? _tool;
    private Crime? _crime;
    private Feature? _feature;
    private bool dragged = false;
    [SerializeField]
    float speed;
    [SerializeField]
    private string cardName;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private int category;
    [SerializeField]
    private int cardIndex;

    public void Setup(string cardName, Sprite sprite, Location? location, Tool? tool, Crime? crime, Feature? feature)
    {
        _location = location;
        _tool = tool;
        _crime = crime;
        _feature = feature;
        gameObject.GetComponentInChildren<Image>().sprite = sprite;
        gameObject.GetComponentInChildren<TextMeshPro>().text = cardName;
    }

    void Awake()
    {
        switch ((Category)category)
        {
            case Category.Location:
                _location = (Location)cardIndex;
                Debug.Log($"Location: {_location}");
                break;
            case Category.Tool:
                _tool = (Tool)cardIndex;
                Debug.Log($"Tool: {_tool}");
                break;
            case Category.Crime:
                _crime = (Crime)cardIndex;
                Debug.Log($"Crime: {_crime}");
                break;
            case Category.Feature:
            default:
                _feature = (Feature)cardIndex;
                Debug.Log($"Feature: {_feature}");
                break;
        }
        gameObject.GetComponentInChildren<Image>().sprite = sprite;
        // Null reference
        // gameObject.GetComponentInChildren<TextMeshPro>().text = cardName;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Make sure that the card being dragged is always on top
        parentAfterDragging = transform.parent;
        transform.SetAsLastSibling();
        dragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.parent.position.z;
        transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDragging);
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var ray in results)
        {
            if (ray.gameObject != gameObject && ray.gameObject.CompareTag("Card"))
            {
                ray.gameObject.GetComponent<Card>().OnCardMerge();
                OnCardMerge();
            }
        }
    }

    void FixedUpdate()
    {
        if (!dragged)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (speed * Time.fixedDeltaTime), transform.position.z); 
            if (transform.position.y <= -11)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnCardMerge()
    {
        GameManager.Instance.MergeEvent.Invoke(_location, _tool, _crime, _feature);
        Destroy(gameObject);
    }
}