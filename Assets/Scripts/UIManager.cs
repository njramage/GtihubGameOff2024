using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject silencePanel = null;

    [SerializeField]
    private GameObject infoPanel = null;

    private List<Snitch> snitches = new List<Snitch>();
    private Snitch selectedSnitch = null;

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

        snitches = GetComponentsInChildren<Snitch>().ToList();
        snitches.ForEach(snitch => snitch.OnSelect += OnSnitchSelect);
    }

    public void OnSnitchSelect(Snitch snitch)
    {
        selectedSnitch = silencePanel.activeInHierarchy ? snitch : null;

        if (silencePanel != null)
        {
            silencePanel.SetActive(!silencePanel.activeInHierarchy);
        }
    }

    private void OnDestroy()
    {
        snitches.ForEach(snitch => snitch.OnSelect -= OnSnitchSelect);
    }
}
