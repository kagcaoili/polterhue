using UnityEngine;

/// <summary>
/// ButtonStart is responsible for starting the game when the button is clicked in TitleScreen
/// </summary>
public class ButtonStart : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.StartGame();
    }
}
