using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Action<SuspectData> OnSelectYesPressed = null;

    [SerializeField]
    private GameObject suspectPanel = null;
    [SerializeField]
    private GameObject suspectPrefab = null;

    [SerializeField]
    private GameObject selectPanel = null;

    [SerializeField]
    private GameObject pausePanel = null;

    private List<Suspect> suspects = new List<Suspect>();
    private SuspectData selectedSuspect = null;

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

        if (suspectPanel == null)
        {
            Debug.LogError($"Cannot continue without {nameof(suspectPanel)} assigned in Inspector!");
            return;
        }

        if (suspectPrefab == null)
        {
            Debug.LogError($"Cannot continue without {nameof(suspectPrefab)} assigned in Inspector!");
            return;
        }

        if (selectPanel == null)
        {
            Debug.LogError($"Cannot continue without {nameof(selectPanel)} assigned in Inspector!");
            return;
        }

        if (pausePanel == null)
        {
            Debug.LogError($"Cannot continue without {nameof(pausePanel)} assigned in Inspector!");
            return;
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
            suspectComponent.SetupData(suspect);
            instantiatedSuspect.SetActive(true);
        }
    }

    private void OnSuspectSelect(SuspectData suspectData)
    {
        if (selectedSuspect == suspectData)
        {
            OnSelectNo();
            return;
        }

        selectedSuspect = suspectData;
        selectPanel?.SetActive(true);
        selectPanel?.transform.SetAsLastSibling();
    }

    public void OnSelectYes()
    {
        if (selectedSuspect == null)
        {
            Debug.LogError($"Something went wrong and {nameof(selectedSuspect)} was null!");
            return;
        }

        OnSelectYesPressed?.Invoke(selectedSuspect);
        selectPanel?.SetActive(false);
    }

    public void OnSelectNo()
    {
        selectedSuspect = null;
        selectPanel?.SetActive(false);
    }

    public void OnReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitApplication()
    {
        Application.Quit();
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

        OnSelectYesPressed = null;
    }
}
