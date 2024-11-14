using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UnityEvent<Location?, Tool?, Crime?, Feature?> MergeEvent;

    [SerializeField]
    private GameObject uiManagerPrefab = null;
    private UIManager uiManager = null;

    [SerializeField]
    private GameObject slotMachinePrefab = null;
    private SlotMachine slotMachine = null;

    [SerializeField]
    private int numberOfSuspects = 6;
    private SuspectData correctSuspect = null;
    private List<SuspectData> suspectData = new List<SuspectData>();

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

        SetupGame();
    }

    private void SetupGame()
    {
        RandomiseSuspects();

        SetupSlotMachine();
    }

    private void SetupSlotMachine()
    {
        if (slotMachinePrefab == null)
        {
            Debug.LogError($"Cannot setup game without {nameof(slotMachinePrefab)} assigned in Inspector!");
            return;
        }

        if (slotMachine == null)
        {
            var spawnedSlotMachine = Instantiate(slotMachinePrefab, gameObject.transform);
            slotMachine = spawnedSlotMachine.GetComponent<SlotMachine>();
        }

        slotMachine.OnSpinComplete += SetupUI;
        slotMachine.Setup(correctSuspect, 3, true);
    }

    private void SetupUI()
    {
        if (uiManagerPrefab == null)
        {
            Debug.LogError($"Cannot setup game without {nameof(uiManagerPrefab)} assigned in Inspector!");
            return;
        }

        if (uiManager == null)
        {
            var spawnedUiManager = Instantiate(uiManagerPrefab, gameObject.transform);
            uiManager = spawnedUiManager.GetComponent<UIManager>();
        }

        uiManager.Setup(suspectData);
        uiManager.OnSelectYesPressed += OnSuspectSelected;
    }

    private void RandomiseSuspects()
    {
        // More fine-tuning will likely need to go into this for difficulty reasons.
        // Change as you see fit. Have not yet added functionality to set similar data on other suspects.
        // (also obviously the correct suspect shouldn't be the first one every time lol)
        correctSuspect = new SuspectData
        {
            Location = (Location)Random.Range(0, 4),
            Tool = (Tool)Random.Range(0, 4),
            Crime = (Crime)Random.Range(0, 4),
            Feature = (Feature)Random.Range(0, 4)
        };
        suspectData.Add(correctSuspect);

        var similarSuspectAdded = false;
        for (int i = 1; i < numberOfSuspects; i++)
        {
            var similarSuspect = similarSuspectAdded ? false : Random.Range(0, 9) % 2 == 0;

            suspectData.Add(new SuspectData
            {
                Location = (Location)Random.Range(0, 4),
                Tool = (Tool)Random.Range(0, 4),
                Crime = (Crime)Random.Range(0, 4),
                Feature = (Feature)Random.Range(0, 4)
            });

            if (similarSuspect)
            {
                similarSuspectAdded = true;
            }
        }
    }

    private void OnSuspectSelected(SuspectData suspectData)
    {
        Debug.Log($"Selected suspect with " +
            $"Location: {suspectData.Location} " +
            $"Tool: {suspectData.Tool} " +
            $"Crime: {suspectData.Crime} " +
            $"Feature: {suspectData.Feature}");

        bool correctGuess = suspectData == correctSuspect;
        Debug.Log($"Correct suspect? {correctGuess}");

        if (correctGuess)
            SceneManager.LoadScene("GameWin", LoadSceneMode.Additive);
        else
            SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }

    private void OnDestroy()
    {
        if (slotMachine != null)
        {
            slotMachine.OnSpinComplete -= SetupUI;
        }

        if (uiManager != null)
        {
            uiManager.OnSelectYesPressed -= OnSuspectSelected;
        }
    }
}
