using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Suspect : MonoBehaviour
{
    public Action<SuspectData> OnSelect;

    private SuspectData suspectData = null;

    [SerializeField]
    private List<Image> indicators = new List<Image>();

    private bool locationMatched = false;
    private bool toolMatched = false;
    private bool crimeMatched = false;
    private bool featureMatched = false;

    private int progress = 0;

    public void SetupData(SuspectData data)
    {
        if (!indicators.Any())
        {
            Debug.LogError($"Cannot continue without assigning {nameof(indicators)} in Inspector!");
            return;
        }

        foreach (var indicator in indicators)
        {
            indicator.color = Color.gray;
        }

        suspectData = data;
        GameManager.Instance.MergeEvent.AddListener(OnCardsCombined);
    }

    public void SelectSuspect()
    {
        OnSelect?.Invoke(suspectData);
    }

    // Example for now, not currently hooked up to anything.
    // Potentially hooked up to static Action on Cards, triggered when cards are combined,
    // so Suspect UI doesn't need to know about each card spawned?
    private void OnCardsCombined(Location location, Tool tool, Crime crime, Feature feature)
    {
        if (!locationMatched && location == suspectData.Location)
        {
            locationMatched = true;
            TurnOnNextIndicator();
        }

        if (!toolMatched && tool == suspectData.Tool)
        {
            toolMatched = true;
            TurnOnNextIndicator();
        }

        if (!crimeMatched && crime == suspectData.Crime)
        {
            crimeMatched = true;
            TurnOnNextIndicator();
        }

        if (!featureMatched && feature == suspectData.Feature)
        {
            featureMatched = true;
            TurnOnNextIndicator();
        }
    }

    private void TurnOnNextIndicator()
    {
        if (indicators.Count > progress)
        {
            indicators[progress].color = Color.white;
            progress++;
        }
    }

    private void OnDestroy()
    {
        OnSelect = null;
    }
}
