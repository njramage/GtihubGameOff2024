using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject uiManagerPrefab = null;
    private UIManager uiManager = null;

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

        RandomiseSuspects();

        uiManager.Setup(suspectData);
    }

    private void RandomiseSuspects()
    {
        // More fine-tuning will likely need to go into this for difficulty reasons.
        // Change as you see fit.
        correctSuspect = new SuspectData
        {
            Location = (Location)Random.Range(0, 3),
            Tool = (Tool)Random.Range(0, 3),
            Crime = (Crime)Random.Range(0, 3),
            Feature = (Feature)Random.Range(0, 3)
        };
        suspectData.Add(correctSuspect);

        var similarSuspectAdded = false;
        for (int i = 1; i < numberOfSuspects; i++) 
        {
            var similarSuspect = similarSuspectAdded ? false : Random.Range(0, 9) % 2 == 0;

            suspectData.Add(new SuspectData
            {
                Location = (Location)Random.Range(0, 3),
                Tool = (Tool)Random.Range(0, 3),
                Crime = (Crime)Random.Range(0, 3),
                Feature = (Feature)Random.Range(0, 3)
            });

            if (similarSuspect)
            {
                similarSuspectAdded = true;
            }
        }
    }
}
