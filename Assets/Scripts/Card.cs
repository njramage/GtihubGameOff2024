using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDragging;

    private Location _location;
    private Tool _tool;
    private Crime _crime;
    private Feature _feature;
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

    public void Setup(Sprite sprite, Category category, int value)
    {
        switch (category)
        {
            case Category.Location:
                _location = (Location)value;
                gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _location.ToString();
                break;
            case Category.Tool:
                _tool = (Tool)value;
                gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _tool.ToString();
                break;
            case Category.Crime:
                _crime = (Crime)value;
                gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _crime.ToString();
                break;
            case Category.Feature:
                _feature = (Feature)value;
                gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _feature.ToString();
                break;
        }

        if(sprite is not null) gameObject.GetComponentInChildren<Image>().sprite = sprite;
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