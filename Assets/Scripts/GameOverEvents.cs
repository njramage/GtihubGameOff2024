using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverEvents : MonoBehaviour
{
    private UIDocument document;

    private Button playAgainButton;

    private void Start()
    {
        document = GetComponent<UIDocument>();

        playAgainButton = document.rootVisualElement.Q("PlayAgainButton") as Button;
        playAgainButton.RegisterCallback<ClickEvent>(OnPlayAgainClick);
    }

    private void OnPlayAgainClick(ClickEvent clickEvent)
    {
        Debug.Log("Clicked play again");
        //SceneManager.LoadScene("GameScene");
    }
}
