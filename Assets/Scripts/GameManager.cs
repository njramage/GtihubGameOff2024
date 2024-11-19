using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UnityEvent<Location, Tool, Crime, Feature> MergeEvent;

    [SerializeField]
    private UIManager uiManagerPrefab;
    private UIManager uiManager = null;

    [SerializeField]
    private int numberOfSuspects = 6;
    public SuspectData correctSuspect {get; private set;}
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
        if (uiManager == null)
        {
            uiManager = Instantiate(uiManagerPrefab, gameObject.transform);
        }

        RandomiseSuspects();

        uiManager.Setup(suspectData);
        uiManager.OnSelectYesPressed += OnSuspectSelected;
    }

    private void RandomiseSuspects()
    {
        // More fine-tuning will likely need to go into this for difficulty reasons.
        // Change as you see fit. Have not yet added functionality to set similar data on other suspects.
        // (also obviously the correct suspect shouldn't be the first one every time lol)
        var correctSuspectIndex = Random.Range(0, numberOfSuspects);
        for (int i = 0; i < numberOfSuspects; i++)
        {
            var newSuspectData = new SuspectData
            {
                Location = (Location)Random.Range(1, 5),
                Tool = (Tool)Random.Range(1, 5),
                Crime = (Crime)Random.Range(1, 5),
                Feature = (Feature)Random.Range(1, 5)
            };

            if (i == correctSuspectIndex)
            {
                correctSuspect = newSuspectData;
            }

            suspectData.Add(newSuspectData);
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
        if (uiManager != null)
        {
            uiManager.OnSelectYesPressed -= OnSuspectSelected;
        }
    }
}
