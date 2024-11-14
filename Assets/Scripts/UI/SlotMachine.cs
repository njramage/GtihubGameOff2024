using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SlotMachine : MonoBehaviour
{
    public Action OnSpinComplete = null;

    private SuspectData correctSuspect = null;
    private bool setupCorrectly = false;
    private int slotsToHide = 0;

    [SerializeField]
    private Button spinButton = null;

    [Header("Slots")]
    [SerializeField]
    private Slot locationSlot = null;
    [SerializeField]
    private Slot toolSlot = null;
    [SerializeField]
    private Slot crimeSlot = null;
    [SerializeField]
    private Slot featureSlot = null;
    private List<Slot> slots = new List<Slot>();

    public void Setup(SuspectData suspectData, int hiddenSlots = 1, bool spinAutomatically = false)
    { 
        if (spinButton == null)
        {
            Debug.LogError($"Cannot continue without {nameof(spinButton)} assigned in Inspector!");
            return;
        }

        if (locationSlot == null)
        {
            Debug.LogError($"Cannot continue without {nameof(locationSlot)} assigned in Inspector!");
            return;
        }

        if (toolSlot == null)
        {
            Debug.LogError($"Cannot continue without {nameof(toolSlot)} assigned in Inspector!");
            return;
        }

        if (crimeSlot == null)
        {
            Debug.LogError($"Cannot continue without {nameof(crimeSlot)} assigned in Inspector!");
            return;
        }

        if (featureSlot == null)
        {
            Debug.LogError($"Cannot continue without {nameof(featureSlot)} assigned in Inspector!");
            return;
        }

        if (suspectData == null)
        {
            Debug.LogError($"{nameof(suspectData)} cannot be null!");
            return;
        }

        slots = new List<Slot>
        {
            locationSlot, toolSlot, crimeSlot, featureSlot
        };
        correctSuspect = suspectData;
        slotsToHide = hiddenSlots;
        setupCorrectly = true;

        if (spinAutomatically)
        {
            spinButton.gameObject.SetActive(false);
            Spin();
        }
    }

    public async void Spin()
    {
        if (!setupCorrectly)
        {
            return;
        }

        spinButton.gameObject.SetActive(false);

        SetupSlots();

        // Trigger animation (eventually (hopefully))

        // Delay so the player can read clues (can be replaced later)
        await Task.Delay(3000);

        OnSpinComplete?.Invoke();

        OnSpinComplete = null;
        Destroy(gameObject);
    }

    private void SetupSlots()
    {
        var hiddenSlots = DetermineHiddenSlots();

        for (int i = 0; i < slots.Count; i++)
        {
            var spritePath = $"Cards/";
            var slotText = string.Empty;

            if (hiddenSlots.Contains(i))
            {
                spritePath += $"Hidden";
                slotText = "?";
            }
            else
            {
                switch (slots[i].SlotType)
                {
                    case Type.Location:
                        spritePath += $"Location/{correctSuspect.Location}";
                        slotText = correctSuspect.Location.ToString();
                        break;
                    case Type.Tool:
                        spritePath += $"Tool/{correctSuspect.Tool}";
                        slotText = correctSuspect.Tool.ToString();
                        break;
                    case Type.Crime:
                        spritePath += $"Crime/{correctSuspect.Crime}";
                        slotText = correctSuspect.Crime.ToString();
                        break;
                    case Type.Feature:
                        spritePath += $"Feature/{correctSuspect.Feature}";
                        slotText = correctSuspect.Feature.ToString();
                        break;
                }
            }

            Debug.Log($"{spritePath}, \"{slotText}\"");
            slots[i].Setup(Resources.Load<Sprite>(spritePath), slotText);
        }
    }

    private List<int> DetermineHiddenSlots()
    {
        var hiddenSlots = new List<int>();

        for (int i = 0; i < slotsToHide; i++)
        {
            var slotToHide = Random.Range(0, 4);
            if (hiddenSlots.Contains(slotToHide))
            {
                i--;
            }
            else
            {
                hiddenSlots.Add(slotToHide);
            }
        }

        return hiddenSlots;
    }

    private void OnDestroy()
    {
        OnSpinComplete = null;
    }
}
