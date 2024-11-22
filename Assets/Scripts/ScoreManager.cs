using UnityEngine;

public class ScoreManager
{
    public int AllTimeHighScore
    {
        get
        {
            return PlayerPrefs.GetInt(nameof(AllTimeHighScore), 0);
        }
        set
        {
            PlayerPrefs.SetInt(nameof(AllTimeHighScore), value);
        }
    }

    public int CurrentSessionScore
    {
        get
        {
            return PlayerPrefs.GetInt(nameof(CurrentSessionScore), 0);
        }
        set
        {
            PlayerPrefs.SetInt(nameof(CurrentSessionScore), value);
        }
    }
}