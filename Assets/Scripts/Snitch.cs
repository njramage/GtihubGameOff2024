using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Snitch : MonoBehaviour
{
    public Action<Snitch> OnSelect;

    [SerializeField]
    private List<Card> cards = new List<Card>();

    private void Awake()
    {
        
    }

    public void SelectSnitch()
    {
        OnSelect?.Invoke(this);
    }
}
