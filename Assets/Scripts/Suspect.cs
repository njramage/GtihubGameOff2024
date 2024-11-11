using System;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    public Action<Suspect> OnSelect;

    private SuspectData suspectData = null;

    public void SetupData(SuspectData data)
    {
        suspectData = data;
    }

    public void SelectSuspect()
    {
        OnSelect?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnSelect = null;
    }
}
