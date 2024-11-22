using UnityEngine;
using UnityEngine.UIElements;

public class GameWinEvents : GameOverEvents
{
    private Label timeText;

    protected override void Awake()
    {
        base.Awake();

        timeText = document.rootVisualElement.Q("timeText") as Label;
        timeText.text = GameManager.Instance.FinalTimeText;
    }
}
