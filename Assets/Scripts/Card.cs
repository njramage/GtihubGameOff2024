using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDragging;

    private Location _location;
    [SerializeField]
    private List<Sprite> _locationImages;
    private Tool _tool;
    [SerializeField]
    private List<Sprite> _toolImages;
    private Crime _crime;
    [SerializeField]
    private List<Sprite> _crimeImages;
    private Feature _feature;
    [SerializeField]
    private List<Sprite> _featureImages;
    private bool dragged = false;
    [SerializeField]
    float speed;
    [SerializeField]
    private int category;
    [SerializeField]
    private int cardIndex;
    private Sprite cardSprite;
    private string cardName;

    public void Setup(Category category, int value)
    {
        switch (category)
        {
            case Category.Location:
                _location = (Location)value;
                cardName = _location.ToString();
                cardSprite = _locationImages[value - 1];
                break;
            case Category.Tool:
                _tool = (Tool)value;
                cardName = _tool.ToString();
                cardSprite = _toolImages[value - 1];
                break;
            case Category.Crime:
                _crime = (Crime)value;
                cardName = _crime.ToString();
                cardSprite = _crimeImages[value - 1];
                break;
            case Category.Feature:
                _feature = (Feature)value;
                cardName = _feature.ToString();
                cardSprite = _featureImages[value - 1];
                break;
            default:
                cardName = "";
                cardSprite = null;
                break;
        }

        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "<size=60%>" + cardName.Replace("_", " ");
        if(cardSprite is not null) gameObject.GetComponent<Image>().sprite = cardSprite;
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
        var results = new List<RaycastResult>();
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