using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    public Action<Suspect> OnSelect;

    [SerializeField]
    private List<Card> cards = new List<Card>();

    public void SelectSuspect()
    {
        OnSelect?.Invoke(this);
    }
}
