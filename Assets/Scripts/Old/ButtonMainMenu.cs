using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ButtonNextLevel is responsible for starting the next level when the button is clicked.
/// </summary>
public class ButtonMainMenu : MonoBehaviour
{
    public void OnClick()
    {
        GameManagerOld.Instance.ReturnToMainMenu();
    }
}
