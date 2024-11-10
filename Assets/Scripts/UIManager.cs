using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject selectPanel = null;

    [SerializeField]
    private GameObject infoPanel = null;

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

        suspects = GetComponentsInChildren<Suspect>().ToList();
        suspects.ForEach(suspect => suspect.OnSelect += OnSuspectSelect);
    }

    public void OnSuspectSelect(Suspect suspect)
    {
        selectedSuspect = selectPanel.activeInHierarchy ? suspect : null;

        if (selectPanel != null)
        {
            selectPanel.SetActive(!selectPanel.activeInHierarchy);
        }
    }

    private void OnDestroy()
    {
        suspects.ForEach(suspect => suspect.OnSelect -= OnSuspectSelect);
    }
}
