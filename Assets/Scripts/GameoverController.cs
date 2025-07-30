using UnityEngine;

public class GameoverController : MonoBehaviour
{
    public GameObject Content;
    public GameObject WinText;
    public GameObject LoseText;
    public GameObject ExitButton;
    public GameObject ContinueButton;

    // Hide on start
    void Start()
    {
        Hide();
    }

    public void Hide()
    {
        Content.SetActive(false);
    }

    // Set up UI to show correct gameover sequence. Do not show continue button when there are no more levels remaining
    public void Setup(bool win, bool endGame)
    {
        WinText.SetActive(win);
        LoseText.SetActive(!win);

        ExitButton.SetActive(true);
        ContinueButton.SetActive(!endGame);

        Content.SetActive(true);
    }
}
