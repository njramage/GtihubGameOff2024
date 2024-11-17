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
    private UIDocument correctRevealIndicatorPanel;

    private struct UIIndicators
    {
        private VisualElement locationIndicator;
        private VisualElement toolIndicator;
        private VisualElement crimeIndicator;
        private VisualElement featureIndicator;

        public UIIndicators(VisualElement locationIndicator, VisualElement toolIndicator, VisualElement crimeIndicator, VisualElement featureIndicator)
        {
            this.locationIndicator = locationIndicator;
            this.toolIndicator = toolIndicator;
            this.crimeIndicator = crimeIndicator;
            this.featureIndicator = featureIndicator;
        }

        public void disableAll()
        {
            locationIndicator.SetEnabled(false);
            toolIndicator.SetEnabled(false);
            crimeIndicator.SetEnabled(false);
            featureIndicator.SetEnabled(false);
        }

        public void showCorrectIndicator(Category category)
        {
            switch (category)
            {
                case Category.Location:
                    if (!locationIndicator.enabledSelf)
                    {
                        locationIndicator.SetEnabled(true);
                    }
                    break;
                case Category.Tool:
                    if (!toolIndicator.enabledSelf)
                    {
                        toolIndicator.SetEnabled(true);
                    }
                    break;
                case Category.Crime:
                    if (!crimeIndicator.enabledSelf)
                    {
                        crimeIndicator.SetEnabled(true);
                    }
                    break;
                case Category.Feature:
                    if (!featureIndicator.enabledSelf)
                    {
                        featureIndicator.SetEnabled(true);
                    }
                    break;
                default:
                    break;
            }
        }
    };

    private UIIndicators correctIndicators;

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

        correctRevealIndicatorPanel = transform.parent.GetComponent<UIDocument>();
        VisualElement locationIndicator = correctRevealIndicatorPanel.rootVisualElement.Q("LocationIndicator");
        VisualElement toolIndicator = correctRevealIndicatorPanel.rootVisualElement.Q("ToolIndicator");
        VisualElement crimeIndicator = correctRevealIndicatorPanel.rootVisualElement.Q("CrimeIndicator");
        VisualElement featureIndicator = correctRevealIndicatorPanel.rootVisualElement.Q("FeatureIndicator");
        correctIndicators = new UIIndicators(locationIndicator, toolIndicator, crimeIndicator, featureIndicator);
        correctIndicators.disableAll();
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

        GameManager.Instance.MergeEvent.AddListener(updateCorrectIndicators);
    }

    private void updateCorrectIndicators(Location location, Tool tool, Crime crime, Feature feature)
    {
        var correctData = GameManager.Instance.correctSuspect;
        if (location == correctData.Location)
        {
            correctIndicators.showCorrectIndicator(Category.Location);
        }

        if (tool == correctData.Tool)
        {
            correctIndicators.showCorrectIndicator(Category.Tool);
        }

        if (crime == correctData.Crime)
        {
            correctIndicators.showCorrectIndicator(Category.Crime);
        }

        if (feature == correctData.Feature)
        {
            correctIndicators.showCorrectIndicator(Category.Feature);
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