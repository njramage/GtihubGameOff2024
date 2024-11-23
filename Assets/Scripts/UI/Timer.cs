using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action<float> OnEnd = null;

    private float timeRemaining = 0.0f;
    private TMP_Text textChild = null;

    private bool timeExpiredInvoked = false;

    public void Setup(float maxTime)
    {
        // Pad by 1 so that the timer text actually starts at the specified round length
        timeRemaining = maxTime + 1.0f;

        textChild = GetComponentInChildren<TMP_Text>();
        if (textChild == null)
        {
            Debug.LogError($"Cannot continue without {nameof(textChild)} on child GameObject!");
            return;
        }
    }

    public void OnSuspectSelectedSuccessfully()
    {
        OnEnd?.Invoke(timeRemaining);
    }

    private void Update()
    {
        if (!GameManager.Instance.GameplayPaused && timeRemaining > 1)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }

        if (timeRemaining <= 1 && !timeExpiredInvoked)
        {
            OnEnd?.Invoke(timeRemaining);
            timeExpiredInvoked = true;
        }
    }

    private void UpdateTimerText()
    {
        if (timeRemaining > 1)
        {
            var minutes = Mathf.FloorToInt(timeRemaining / 60);
            var seconds = Mathf.FloorToInt(timeRemaining % 60);

            textChild.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            textChild.text = "00:00";
        }
    }

    private void OnDestroy()
    {
        OnEnd = null;
    }
}
