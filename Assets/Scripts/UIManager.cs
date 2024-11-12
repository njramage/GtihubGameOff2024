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

    [SerializeField]
    private GameObject pausePanel = null;

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

    private void Update()
    {
        if (Input.GetAxis("Cancel") >= 0.5)
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(!pausePanel.activeInHierarchy);
            }
            else
            {
                Debug.LogError("You have forgotten to assign the pause panel in the inspector");
            }
        }
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
