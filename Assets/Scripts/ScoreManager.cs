using UnityEngine;

public class ScoreManager
{
    public int AllTimeHighScore
    {
        get
        {
            return PlayerPrefs.GetInt(nameof(AllTimeHighScore));
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
            return PlayerPrefs.GetInt(nameof(CurrentSessionScore));
        }
        set
        {
            PlayerPrefs.SetInt(nameof(AllTimeHighScore), value);
        }
    }

    public void ClearCurrentSessionScore()
    {
        PlayerPrefs.DeleteKey(nameof(CurrentSessionScore));
    }
}