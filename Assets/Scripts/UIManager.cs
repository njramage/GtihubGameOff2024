using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject suspectPanel = null;
    [SerializeField]
    private GameObject suspectPrefab = null;

    [SerializeField]
    private GameObject selectPanel = null;

    private List<Suspect> suspects = new List<Suspect>();
    private Suspect selectedSuspect = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Setup(List<SuspectData> suspectData)
    {
        foreach(var suspect in suspectData)
        {
            var instantiatedSuspect = Instantiate(suspectPrefab, suspectPanel.transform);
            var suspectComponent = instantiatedSuspect.GetComponent<Suspect>();
            suspects.Add(suspectComponent);
            suspectComponent.OnSelect += OnSuspectSelect;
            instantiatedSuspect.SetActive(true);
        }
    }

    private void OnSuspectSelect(Suspect suspect)
    {
        selectedSuspect = selectPanel.activeInHierarchy ? suspect : null;

        if (selectPanel != null)
        {
            selectPanel.SetActive(!selectPanel.activeInHierarchy);
        }
    }

    public void OnYesPressed()
    {
        if (selectedSuspect == null)
        {
            Debug.LogError("Something went wrong and selectedSuspect was null!");
            return;
        }
    }

    private void OnDestroy()
    {
        foreach(var suspect in suspects)
        {
            if (suspect != null)
            {
                suspect.OnSelect -= OnSuspectSelect;
            }
        }
    }
}
