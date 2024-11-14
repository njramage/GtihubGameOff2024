using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Type SlotType { get { return slotType; } }

    [SerializeField]
    private Type slotType = Type.Location;

    [SerializeField]
    private Image image = null;
    [SerializeField]
    private TMP_Text textMeshPro = null;

    public void Setup(Sprite sprite, string name)
    {
        if (image == null)
        {
            Debug.LogError($"Cannot continue without {nameof(image)} assigned in Inspector of {gameObject.name}!");
            return;
        }

        if (textMeshPro == null)
        {
            Debug.LogError($"Cannot continue without {nameof(textMeshPro)} assigned in Inspector of {gameObject.name}!");
            return;
        }

        image.sprite = sprite;
        textMeshPro.text = name;
    }
}
