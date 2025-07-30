using UnityEngine;

/// <summary>
/// ButtonNextLevel is responsible for starting the next level when the button is clicked.
/// </summary>
public class ButtonNextLevel : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.StartNextLevel();
    }
}
