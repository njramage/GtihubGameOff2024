using System;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    public Action<SuspectData> OnSelect;

    private SuspectData suspectData = null;

    public void SetupData(SuspectData data)
    {
        suspectData = data;
    }

    public void SelectSuspect()
    {
        OnSelect?.Invoke(suspectData);
    }

    private void OnDestroy()
    {
        OnSelect = null;
    }
}
